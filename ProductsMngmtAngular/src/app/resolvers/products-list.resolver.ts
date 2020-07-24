import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ProductService } from '../services/product.service';
import { Product } from '../models/product/product.model';
import { FilterParam } from '../shared/helpers/filter-param';

@Injectable()
export class ProductsListResolver implements Resolve<Product[]> {
  pageNumber = 1;
  pageSize = 12;
  categoriesIds = null;

  constructor(
    private productService: ProductService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): Observable<Product[]> {
    if (route.queryParams != null) {
      this.pageNumber = route.queryParams.pageNumber;
      this.pageSize = route.queryParams.pageSize;
      this.categoriesIds = route.queryParamMap.getAll('categoriesIds');
    }

    const otherFilters: FilterParam[] = [];

    this.categoriesIds.forEach((categoryId) => {
      otherFilters.push(new FilterParam('categoriesIds', categoryId));
    });

    return this.productService
      .getAllPaginated(this.pageNumber, this.pageSize, otherFilters)
      .pipe(
        catchError((error) => {
          this.alertify.error('Problem with retriving list of products');
          this.router.navigate(['/home']);
          return of(null);
        })
      );
  }
}

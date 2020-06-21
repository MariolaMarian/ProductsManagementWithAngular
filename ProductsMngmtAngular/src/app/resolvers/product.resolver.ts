import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../services/alertify.service';
import { of, forkJoin } from 'rxjs';
import { catchError, map } from 'rxjs/operators';
import { ProductService } from '../services/product.service';
import { Product } from '../products/models/product.model';
import { ExpirationDateService } from '../services/expiration-date.service';
import { FilterParam } from '../helpers/filter-param';

@Injectable()
export class ProductResolver implements Resolve<Product> {
  constructor(
    private productService: ProductService,
    private expirationDateService: ExpirationDateService,
    private router: Router,
    private alertify: AlertifyService
  ) {}

  resolve(route: ActivatedRouteSnapshot): any {
    const product = this.productService.get(route.params.id);
    const expirationDates = this.expirationDateService.getAllPaginated(
      null,
      null,
      [new FilterParam('productId', route.params.id), new FilterParam('withEmployees', 'true')]
    );
    const result = forkJoin([product, expirationDates]).pipe(
      map((allResponses) => {
        return {
          product: allResponses[0],
          expirationDates: allResponses[1],
        };
      }),
      catchError(() => {
        this.alertify.error('Problem with retriving product details');
        this.router.navigate(['/categories']);
        return of(null);
      })
    );
    return result;
  }
}

import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Category } from '../models/category/category.model';
import { CategoryService } from '../services/category.service';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class CategoryResolver implements Resolve<Category> {

    constructor(private categoryService: CategoryService, private router: Router, private alertify: AlertifyService){}

    resolve(route: ActivatedRouteSnapshot): Observable<Category> {
        return this.categoryService.getCategory(route.params.id).pipe(
            catchError(error => {
                this.alertify.error('Problem with retriving category details');
                this.router.navigate(['/categories']);
                return of(null);
            })
        );
    }
}

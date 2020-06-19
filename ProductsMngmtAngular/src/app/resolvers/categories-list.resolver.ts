import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { Category } from '../models/category.model';
import { CategoryService } from '../services/category.service';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';

@Injectable()
export class CategoriesListResolver implements Resolve<Category[]> {

    constructor(private categoryService: CategoryService, private router: Router, private alertify: AlertifyService){}

    resolve(route: ActivatedRouteSnapshot): Observable<Category[]> {
        return this.categoryService.getCategories().pipe(
            catchError(error => {
                this.alertify.error('Problem with retriving list of categories');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}

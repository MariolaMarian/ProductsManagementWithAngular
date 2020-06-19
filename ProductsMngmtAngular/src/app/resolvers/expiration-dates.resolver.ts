import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ExpirationDate } from '../models/expiration-date.model';
import { ExpirationDateService } from '../services/expiration-date.service';
import { FilterParam } from '../helpers/filter-param';

@Injectable()
export class ExpirationDatesListResolver implements Resolve<ExpirationDate[]> {
    pageNumber = 1;
    pageSize = 12;

    constructor(private expirationDateService: ExpirationDateService, private router: Router, private alertify: AlertifyService){}

    resolve(route: ActivatedRouteSnapshot): Observable<ExpirationDate[]> {
        if (route.queryParams != null){
            this.pageNumber = route.queryParams.pageNumber;
            this.pageSize = route.queryParams.pageSize;
        }
        return this.expirationDateService.getAllPaginated(this.pageNumber, this.pageSize).pipe(
            catchError(error => {
                this.alertify.error('Problem with retriving list of expiration dates');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}

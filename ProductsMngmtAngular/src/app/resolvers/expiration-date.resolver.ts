import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { ExpirationDateService } from '../services/expiration-date.service';
import { ExpirationDate } from '../models/expiration-date.model';

@Injectable()
export class ExpirationDateResolver implements Resolve<ExpirationDate> {

    constructor(private expirationDateService: ExpirationDateService, private router: Router, private alertify: AlertifyService){}

    resolve(route: ActivatedRouteSnapshot): Observable<ExpirationDate> {
        return this.expirationDateService.get(route.params.id).pipe(
            catchError(error => {
                this.alertify.error('Problem with retriving expiration date details');
                this.router.navigate(['/expirationDates']);
                return of(null);
            })
        );
    }
}

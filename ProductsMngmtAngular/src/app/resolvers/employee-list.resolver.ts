import { Injectable } from '@angular/core';
import { Resolve, Router, ActivatedRouteSnapshot } from '@angular/router';
import { AlertifyService } from '../services/alertify.service';
import { Observable, of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { UserService } from '../services/user.service';
import { Employee } from '../models/employee.model';

@Injectable()
export class EmployeeListResolver implements Resolve<Employee[]> {

    constructor(private userService: UserService, private router: Router, private alertify: AlertifyService){}

    resolve(route: ActivatedRouteSnapshot): Observable<Employee[]> {
        return this.userService.getUsers().pipe(
            catchError(error => {
                this.alertify.error('Problem with retriving list of users');
                this.router.navigate(['/home']);
                return of(null);
            })
        );
    }
}

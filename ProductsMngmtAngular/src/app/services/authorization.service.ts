import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { map } from 'rxjs/operators';
import { JwtHelperService } from '@auth0/angular-jwt';
import { environment } from 'src/environments/environment';
import { EmployeeLogin } from '../models/employee/employee-login.model';
import { EmployeeForRegistration } from '../models/employee/employee-register.model';

@Injectable({
  providedIn: 'root',
})
export class AuthorizationService {
  baseUrl = environment.apiUrl + 'auth/';
  jwtHelper = new JwtHelperService();

  get userName(): string {
    return localStorage.getItem('username');
  }

  constructor(private http: HttpClient) {}

  login(employeeModel: EmployeeLogin) {
    return this.http.post(this.baseUrl + 'login', employeeModel).pipe(
      map((response: any) => {
        const user = response;
        if (user) {
          localStorage.setItem('token', user.token);
          const decodedToken = this.jwtHelper.decodeToken(user.token);
          localStorage.setItem('username', decodedToken.nameid[1]);
          localStorage.setItem('roles', decodedToken.role);
        }
      })
    );
  }

  register(employeeModel: EmployeeForRegistration) {
    return this.http.post(this.baseUrl + 'register', employeeModel);
  }

  update(employeeToUpdate: EmployeeForRegistration) {
    return this.http.post(this.baseUrl + 'update', employeeToUpdate);
  }

  loggedIn() {
    const token = localStorage.getItem('token');
    return !this.jwtHelper.isTokenExpired(token);
  }

  matchesRoles(requiredRoles: string[]): boolean {
    let matches = false;
    const employeeRoles = (localStorage.getItem(
      'roles'
    ) as unknown) as string[];
    requiredRoles.forEach((role) => {
      if (employeeRoles.includes(role)) {
        matches = true;
      }
    });
    return matches;
  }
}

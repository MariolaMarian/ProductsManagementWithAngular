import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Employee } from '../models/employee/employee.model';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  baseUrl = environment.apiUrl + 'users';

  constructor(private httpClient: HttpClient) {}

  getUsers(): Observable<Employee[]> {
    return this.httpClient.get<Employee[]>(this.baseUrl);
  }

  getUser(id: string): Observable<Employee> {
    return this.httpClient.get<Employee>(this.baseUrl + '/' + id);
  }
}

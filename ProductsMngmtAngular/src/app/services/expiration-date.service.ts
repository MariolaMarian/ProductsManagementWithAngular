import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { ExpirationDate } from '../models/expiration-date.model';
import { PaginatedResult } from '../models/paginated-result';
import { map } from 'rxjs/operators';
import { BaseService } from './base-service';
import { ExpirationDateForCreate } from '../models/expiration-date-register.model';
import { ExpirationDateForUpdate } from '../models/expiration-date-for-update';

@Injectable({
  providedIn: 'root',
})
export class ExpirationDateService extends BaseService<ExpirationDate, ExpirationDateForCreate, ExpirationDateForUpdate> {

  constructor(httpClient: HttpClient) {
    super(httpClient, environment.apiUrl + 'expirationDates');
  }

}

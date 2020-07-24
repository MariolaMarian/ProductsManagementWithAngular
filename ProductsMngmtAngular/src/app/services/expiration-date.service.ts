import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { ExpirationDateForUpdate } from '../models/expiration-date/expiration-date-for-update';
import { ExpirationDate } from '../models/expiration-date/expiration-date.model';
import { ExpirationDateForCreate } from '../models/expiration-date/expiration-date-register.model';
import { BaseService } from './base-service';

@Injectable({
  providedIn: 'root',
})
export class ExpirationDateService extends BaseService<ExpirationDate, ExpirationDateForCreate, ExpirationDateForUpdate> {

  constructor(httpClient: HttpClient) {
    super(httpClient, environment.apiUrl + 'expirationDates');
  }

}

import { Injectable } from '@angular/core';
import { ExpirationDate } from '../models/expiration-date/expiration-date.model';

@Injectable({
  providedIn: 'root'
})
export class ExpDatesHelperService {

  constructor() { }

  nearestDate(expirationDates: ExpirationDate[]): ExpirationDate{
    const today = new Date();
    if (expirationDates == null || expirationDates.length === 0){
        return new ExpirationDate();
    }
    if (expirationDates.length === 1){
        return expirationDates[0];
    }
    const nearest = expirationDates.reduce((a, b) =>
        (new Date(a.endDate).getTime() - today.getTime()) < (new Date(b.endDate).getTime() - today.getTime()) ? a : b);
    return nearest;
}
}

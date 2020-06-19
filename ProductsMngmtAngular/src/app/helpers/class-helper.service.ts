import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ClassHelperService {

  constructor() { }

  setClass(collected: boolean, endDate: Date, maxDays: number): any {
    let classToReturn = 'collected';

    const timeSpan =
      Math.ceil(new Date(endDate).getTime() - new Date().getTime()) /
      (1000 * 60 * 60 * 24);

    if (collected) {
      return classToReturn;
    }

    classToReturn = 'safe';

    if (timeSpan <= 2 * maxDays) {
      classToReturn = 'comming';
    }

    if (timeSpan <= maxDays) {
      classToReturn = 'ending';
    }

    return classToReturn;
  }
}

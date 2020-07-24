import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { IItemWithId } from 'src/app/interfaces/item-with-id.interface';
import { PaginatedResult } from 'src/app/models/paginated-result';
import { FilterParam } from '../shared/helpers/filter-param';

export abstract class BaseService<
  T extends IItemWithId,
  TForCreate,
  TForUpdate extends IItemWithId
> {
  constructor(protected httpClient: HttpClient, protected actionUrl: string) {}

  getAllPaginated(
    page?,
    itemsPerPage?,
    otherParams?: FilterParam[]
  ): Observable<PaginatedResult<T[]>> {
    const paginatedResult: PaginatedResult<T[]> = new PaginatedResult<T[]>();

    let params = new HttpParams();
    if (page != null && itemsPerPage != null && itemsPerPage > 0) {
      params = params.append('pageNumber', page);
      params = params.append('pageSize', itemsPerPage);
    }

    if (otherParams != null) {
      otherParams.forEach((param) => {
        if (param !== undefined) {
          params = params.append(param.key, param.value);
        }
      });
    }

    return this.httpClient
      .get<T[]>(this.actionUrl, { observe: 'response', params })
      .pipe(
        map((response) => {
          paginatedResult.result = response.body;
          if (response.headers.get('Pagination') != null) {
            paginatedResult.pagination = JSON.parse(
              response.headers.get('Pagination')
            );
          }
          return paginatedResult;
        })
      );
  }

  getAll(otherParams?: FilterParam[]): Observable<T[]> {
    let params = new HttpParams();
    if (otherParams != null) {
      otherParams.forEach((param) => {
        params = params.append(param.key, param.value);
      });
    }
    return this.httpClient.get<T[]>(this.actionUrl, {
      observe: 'body',
      params,
    });
  }

  get(id: number): Observable<T> {
    return this.httpClient.get<T>(this.actionUrl + '/' + id);
  }

  post(item: TForCreate) {
    return this.httpClient.post(this.actionUrl, item);
  }

  put(item: TForUpdate) {
    return this.httpClient.put(this.actionUrl + '/' + item.id, item);
  }

  deleteExpirationDate(id: number) {
    return this.httpClient.delete(this.actionUrl + '/' + id);
  }
}

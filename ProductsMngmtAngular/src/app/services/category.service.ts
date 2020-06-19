import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Category } from '../models/category.model';
import { map } from 'rxjs/operators';

@Injectable({
  providedIn: 'root',
})
export class CategoryService {
  baseUrl = environment.apiUrl + 'categories';

  constructor(private httpClient: HttpClient) {}

  getCategories(): Observable<Category[]> {
    return this.httpClient.get<Category[]>(this.baseUrl + '/categories');
  }

  getCategoriesSimple(): Observable<Category[]>{
    return this.httpClient.get<Category[]>(this.baseUrl + '/categoriesSimple');
  }

  getCategory(id: number): Observable<Category> {
    return this.httpClient.get<Category>(this.baseUrl + '/' + id);
  }

  postCategory(category: Category) {
    return this.httpClient.post(this.baseUrl, category);
  }

  deleteCategory(id: number) {
    return this.httpClient.delete(this.baseUrl + '/' + id);
  }
}

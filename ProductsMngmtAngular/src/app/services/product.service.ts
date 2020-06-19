import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { HttpClient } from '@angular/common/http';
import { Product } from '../models/product.model';
import { Observable } from 'rxjs';
import { BaseService } from './base-service';

@Injectable({
  providedIn: 'root',
})
export class ProductService extends BaseService<Product, Product, Product> {

  constructor(httpClient: HttpClient) {
    super(httpClient, environment.apiUrl + 'products');
  }
}

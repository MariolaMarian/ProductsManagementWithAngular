import { Component, OnInit, Input } from '@angular/core';
import { Product } from 'src/app/products/models/product.model';
import { ExpDatesHelperService } from 'src/app/helpers/exp-dates-helper.service';
import { ExpirationDate } from 'src/app/models/expiration-date.model';
import { ClassHelperService } from 'src/app/helpers/class-helper.service';
import { ExpirationDateService } from 'src/app/services/expiration-date.service';
import { FilterParam } from 'src/app/helpers/filter-param';

@Component({
  selector: 'app-product-small',
  templateUrl: './product-small.component.html',
  styleUrls: ['./product-small.component.css'],
})
export class ProductSmallComponent implements OnInit {
  @Input() product: Product;
  productSmallClass: any;

  constructor(
    private classHelper: ClassHelperService,
  ) {}

  ngOnInit(): void {
    this.product = new Product(this.product);
    this.setClass();
  }

  setClass() {
    this.productSmallClass = this.classHelper.setClass(
      false,
      this.product.nearestDate,
      this.product.maxDays
    );
  }
}

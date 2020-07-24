import { Component, OnInit, Input } from '@angular/core';
import { Product } from 'src/app/models/product/product.model';
import { ClassHelperService } from 'src/app/shared/helpers/class-helper.service';

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

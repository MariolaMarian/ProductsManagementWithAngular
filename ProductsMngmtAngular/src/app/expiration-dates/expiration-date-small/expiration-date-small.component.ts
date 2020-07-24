import { Component, OnInit, Input } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ExpirationDate } from 'src/app/models/expiration-date/expiration-date.model';
import { Employee } from 'src/app/models/employee/employee.model';
import { Product } from 'src/app/models/product/product.model';
import { ClassHelperService } from 'src/app/shared/helpers/class-helper.service';

@Component({
  selector: 'app-expiration-date-small',
  templateUrl: './expiration-date-small.component.html',
  styleUrls: ['./expiration-date-small.component.css'],
})
export class ExpirationDateSmallComponent implements OnInit {
  @Input() showProductData: boolean;
  @Input() product: Product;
  @Input() expirationDate: ExpirationDate;
  expDateClass: any;

  constructor(
    private route: ActivatedRoute,
    private classHelper: ClassHelperService
  ) {}

  ngOnInit(): void {
    if (this.expirationDate === undefined) {
      this.route.data.subscribe((data) => {
        this.expirationDate = data.expirationDate;
      });
      this.showProductData = true;
    }
    if (this.product !== undefined) {
      this.expirationDate.product = this.product;
    }
    if (this.expirationDate.collectedBy === undefined) {
      this.expirationDate.collectedBy = new Employee();
    }
    this.expDateClass = this.classHelper.setClass(this.expirationDate.collected,
      this.expirationDate.endDate, this.expirationDate.product.maxDays);

    this.expirationDate.product = new Product(this.expirationDate.product);
  }
}

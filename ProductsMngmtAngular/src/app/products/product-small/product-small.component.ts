import { Component, OnInit, Input } from '@angular/core';
import { Product } from 'src/app/models/product.model';
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
  nearestDate: ExpirationDate;

  constructor(
    private expDatesHelper: ExpDatesHelperService,
    private classHelper: ClassHelperService,
    private expirationDatesService: ExpirationDateService
  ) {}

  ngOnInit(): void {
    this.product = new Product(this.product);
    this.loadDates();
  }

  findNearestDate() {
    this.nearestDate = this.expDatesHelper.nearestDate(
      this.product.expirationDates
    );
  }

  setClass() {
    this.productSmallClass = this.classHelper.setClass(
      this.nearestDate.collected,
      this.nearestDate.endDate,
      this.product.maxDays
    );
  }

  loadDates() {
    
    //TO DO: GET ONLY NEAREST DATE
    //
    this.expirationDatesService
      .getAll([new FilterParam('productId', this.product.id.toString())])
      .subscribe((resp) => {
        this.product.expirationDates = resp;

        this.findNearestDate();
        this.setClass();
      });
  }
}

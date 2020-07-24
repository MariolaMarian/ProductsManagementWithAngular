import { Component, OnInit, Input } from '@angular/core';
import { ExpirationDateService } from 'src/app/services/expiration-date.service';
import { ExpirationDateForUpdate } from 'src/app/models/expiration-date/expiration-date-for-update';
import { AlertifyService } from 'src/app/services/alertify.service';
import { ExpirationDate } from 'src/app/models/expiration-date/expiration-date.model';
import { ClassHelperService } from 'src/app/shared/helpers/class-helper.service';

@Component({
  selector: 'app-expiration-date-in-product',
  templateUrl: './expiration-date-in-product.component.html',
  styleUrls: ['./expiration-date-in-product.component.css'],
})
export class ExpirationDateInProductComponent implements OnInit {
  @Input() expirationDate: ExpirationDate;
  @Input() maxDays: number;
  expDateClass: any;

  constructor(
    private classHelper: ClassHelperService,
    private expDateService: ExpirationDateService,
    private alertify: AlertifyService
  ) {}

  ngOnInit(): void {
    this.setClass();
  }

  setClass() {
    this.expDateClass = this.classHelper.setClass(
      this.expirationDate.collected,
      this.expirationDate.endDate,
      this.maxDays
    );
  }

  collect() {
    const expirationDateForUpdate = new ExpirationDateForUpdate(
      this.expirationDate
    );

    this.expDateService.put(expirationDateForUpdate).subscribe(
      (res) => {
        this.alertify.success('Expiration date collected');
        this.refresh();
      },
      (error) => {
        this.alertify.error('Error during expiration date collecting');
      }
    );
  }

  refresh() {
    this.expDateService.get(this.expirationDate.id).subscribe((res) => {
      this.expirationDate = res;
      this.setClass();
    });
  }
}

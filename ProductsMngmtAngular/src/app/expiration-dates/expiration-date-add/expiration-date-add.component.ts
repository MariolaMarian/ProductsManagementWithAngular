import { Component, OnInit, Inject } from '@angular/core';
import { ProductService } from 'src/app/services/product.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ExpirationDateService } from 'src/app/services/expiration-date.service';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { DateLaterThanTodayValidator } from 'src/app/validators/dateLaterThanToday.validator';
import { ExpirationDate } from 'src/app/models/expiration-date/expiration-date.model';
import { ExpirationDateForCreate } from 'src/app/models/expiration-date/expiration-date-register.model';
import { Product } from 'src/app/models/product/product.model';

@Component({
  selector: 'app-expiration-date-add',
  templateUrl: './expiration-date-add.component.html',
  styleUrls: ['./expiration-date-add.component.css'],
})
export class ExpirationDateAddComponent implements OnInit {
  expDateForm: FormGroup;
  products: Product[];
  expirationDate: ExpirationDate;

  constructor(
    private productService: ProductService,
    private expirationDateService: ExpirationDateService,
    private alertify: AlertifyService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ExpirationDateAddComponent>,
    @Inject(MAT_DIALOG_DATA) public product: Product
  ) {}

  ngOnInit(): void {
    this.expirationDate = new ExpirationDate();

    this.initializeProductsInfo();
    this.initializeForm();
  }

  initializeForm() {
    this.expDateForm = this.fb.group({
      productId: [this.expirationDate.productId, [Validators.required]],
      endDate: [
        this.expirationDate.endDate,
        [Validators.required, DateLaterThanTodayValidator()],
      ],
    });
  }

  initializeProductsInfo() {
    if (this.product) {
      this.expirationDate.productId = this.product.id;
    } else {
      this.loadProducts();
    }
  }

  loadProducts() {
    this.productService.getAll().subscribe(
      (data) => {
        this.products = data;
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  save() {
    const expirationDateForCreate = new ExpirationDateForCreate(
      this.expDateForm.value.productId,
      new Date(this.expDateForm.value.endDate)
    );
    this.expirationDateService.post(expirationDateForCreate).subscribe(
      (resp: ExpirationDate) => {
        this.alertify.success('Expiration date succesfully added');
        console.log(resp);
        this.dialogRef.close(resp);
      },
      (error) => {
        this.alertify.error(error);
        console.log(error);
      }
    );
  }
}

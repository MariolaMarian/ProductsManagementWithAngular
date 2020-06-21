import { Component, OnInit, ViewChild } from '@angular/core';
import { Product } from 'src/app/products/models/product.model';
import { ActivatedRoute } from '@angular/router';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MatDialog } from '@angular/material/dialog';
import { ExpirationDateAddComponent } from 'src/app/expiration-dates/expiration-date-add/expiration-date-add.component';
import { PaginatedComponentBase } from 'src/app/helpers/paginated-component-base';
import { ExpirationDate } from 'src/app/models/expiration-date.model';
import { AlertifyService } from 'src/app/services/alertify.service';
import { ExpirationDateService } from 'src/app/services/expiration-date.service';
import { FilterParam } from 'src/app/helpers/filter-param';
import { ProductEditComponent } from '../product-edit/product-edit.component';
import { ProductService } from 'src/app/services/product.service';

@Component({
  selector: 'app-product',
  templateUrl: './product.component.html',
  styleUrls: ['./product.component.css'],
})
export class ProductComponent extends PaginatedComponentBase<ExpirationDate>
  implements OnInit {
  product: Product;

  constructor(
    private route: ActivatedRoute,
    private alertify: AlertifyService,
    private expDateService: ExpirationDateService,
    private dialog: MatDialog,
    private productService: ProductService
  ) {
    super();
  }

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.product = new Product(data.data.product);
      this.product.expirationDates = this.initializePagination(
        data.data.expirationDates
      );
    });
  }

  showEditProductDialog() {
    const editProductDialogRef = this.dialog.open(ProductEditComponent, {
      data: this.product,
    });

    editProductDialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.productService.get(this.product.id).subscribe((res) => {
          this.product = new Product(res);
        });
      }
    });
  }

  showAddExpDateDialog() {
    const addExpDateDialogRef = this.dialog.open(ExpirationDateAddComponent, {
      data: this.product,
    });

    addExpDateDialogRef.afterClosed().subscribe((result) => {
      console.log(result);
      if (result) {
        this.product.expirationDates.push(result);
      }
    });
  }

  loadExpirationDates(event: any) {
    if (event.page) {
      this.currentPage = event.page;
    }

    this.expDateService
      .getAllPaginated(this.currentPage, this.itemsPerPage, [
        new FilterParam('productId', this.product.id.toString()),
        new FilterParam('withEmployees', 'true'),
      ])
      .subscribe(
        (data) => {
          this.product.expirationDates = this.initializePagination(data);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
  }
}

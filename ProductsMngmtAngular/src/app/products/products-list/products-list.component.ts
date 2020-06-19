import { Component, OnInit, ViewChild } from '@angular/core';
import { Product } from 'src/app/models/product.model';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from 'src/app/services/product.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { ModalDirective } from 'ngx-bootstrap/modal/public_api';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { PaginatedComponentBase } from 'src/app/helpers/paginated-component-base';
import { MatDialog } from '@angular/material/dialog';
import { ProductEditComponent } from '../product-edit/product-edit.component';
import { ManageFiltersComponent } from 'src/app/filters/manage-filters.component';
import { AuthorizationService } from 'src/app/services/authorization.service';

@Component({
  selector: 'app-products-list',
  templateUrl: './products-list.component.html',
  styleUrls: ['./products-list.component.css'],
})
export class ProductsListComponent extends PaginatedComponentBase<Product>
  implements OnInit {
  @ViewChild('addProductModal', { static: false }) modal: ModalDirective;
  products: Product[];
  selectedCategories: number[];

  constructor(
    private route: ActivatedRoute,
    private productService: ProductService,
    private alertify: AlertifyService,
    private modalService: NgbModal,
    private dialog: MatDialog,
    public authService: AuthorizationService
  ) {
    super();
  }

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.products = this.initializePagination(data.list);
      this.selectedCategories = [];
    });
  }

  loadProducts(event: any) {
    if (event.page) {
      this.currentPage = event.page;
    }

    this.productService
      .getAllPaginated(this.currentPage, this.itemsPerPage)
      .subscribe(
        (data) => {
          this.products = this.initializePagination(data);
        },
        (error) => {
          this.alertify.error(error);
        }
      );

    this.modalService.dismissAll();
  }

  showFilterDialog() {
    const filtersDialogRef = this.dialog.open(ManageFiltersComponent, {});

    filtersDialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log(result);
      }
    });
  }

  showAddProductDialog() {
    const addProductDialogRef = this.dialog.open(ProductEditComponent, {});

    addProductDialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadProducts(this.currentPage);
      }
    });
  }
}

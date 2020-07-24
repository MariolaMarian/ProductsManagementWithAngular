import { Component, OnInit, ViewChild } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ProductService } from 'src/app/services/product.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { ModalDirective } from 'ngx-bootstrap/modal/public_api';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { MatDialog } from '@angular/material/dialog';
import { ProductEditComponent } from '../product-edit/product-edit.component';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { Product } from 'src/app/models/product/product.model';
import { IProductsFiltersForm } from 'src/app/shared/helpers/products-filters-form.interface';
import { PaginatedComponentBase } from 'src/app/shared/helpers/paginated-component-base';
import { ManageFiltersComponent } from 'src/app/shared/manage-filters/manage-filters.component';
import { ProductsFiltersParams } from 'src/app/shared/helpers/products-filters-params';

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
  filtersFromDialog: IProductsFiltersForm;
  public loading = false;

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

  loadProducts(event: any, parameters = []) {
    this.loading = true;

    if (event && event.page) {
      this.currentPage = event.page;
    }

    this.productService
      .getAllPaginated(this.currentPage, this.itemsPerPage, parameters)
      .subscribe(
        (data) => {
          this.products = this.initializePagination(data);
        },
        (error) => {
          this.alertify.error(error);
        },
        () => {
          this.loading = false;
        }
      );

    this.modalService.dismissAll();
  }

  showFilterDialog() {
    const filtersDialogRef = this.dialog.open(ManageFiltersComponent, {
      data: this.filtersFromDialog,
    });

    filtersDialogRef.afterClosed().subscribe((result: IProductsFiltersForm) => {
      if (result) {
        console.log(result);
        this.filtersFromDialog = result;
        const filters = new ProductsFiltersParams(
          result
        ).ReturnAsFilterParams();
        this.loadProducts(null, filters);
      }
    });
  }

  clearFilters() {
    this.loadProducts(null);
    this.filtersFromDialog = null;
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

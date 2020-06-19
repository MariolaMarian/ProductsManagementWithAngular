import { Component, OnInit, Input, Inject } from '@angular/core';
import { FormGroup, FormBuilder, FormArray } from '@angular/forms';
import { PropertyToOrderBy } from 'src/app/models/property-to-order-by';
import { CategorySimple } from 'src/app/models/category/category-simple';
import { MatDialogRef } from '@angular/material/dialog';
import { CategoryService } from '../services/category.service';
import { AlertifyService } from '../services/alertify.service';

@Component({
  selector: 'app-manage-filters',
  templateUrl: './manage-filters.component.html',
  styleUrls: ['./manage-filters.component.css'],
})
export class ManageFiltersComponent implements OnInit {
  properties: PropertyToOrderBy[];
  orders: string[];
  categories: CategorySimple[];
  mainForm: FormGroup;
  ordersError: string;

  constructor(
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ManageFiltersComponent>,
    private categoryService: CategoryService,
    private alertify: AlertifyService
  ) {}

  ngOnInit(): void {
    this.initializeProperties();
    this.initializeOrders();
    this.initializeCategories();

    const productForm = this.fb.group({
      barecode: '',
      name: '',
    });

    const categoriesForm = this.fb.group({
      selectedCategories: [],
    });

    this.mainForm = this.fb.group({
      productInfo: productForm,
      categoriesInfo: categoriesForm,
      ordersBy: this.fb.array([]),
    });
  }

  get orderByForms() {
    return this.mainForm.get('ordersBy') as FormArray;
  }

  addOrderBy() {
    if (this.isOrdersValid()) {
      const orderByForm = this.fb.group({
        property: this.properties[0],
        order: this.orders[1],
      });

      this.orderByForms.push(orderByForm);
    }
  }

  deleteOrderBy(i) {
    this.orderByForms.removeAt(i);
  }

  initializeProperties() {
    this.properties = [];
    this.properties.push(new PropertyToOrderBy(1, 'Product name'));
    this.properties.push(new PropertyToOrderBy(2, 'Category name'));
    this.properties.push(new PropertyToOrderBy(3, 'End date'));
  }

  initializeOrders() {
    this.orders = [];
    this.orders.push('asc');
    this.orders.push('desc');
  }

  initializeCategories() {
    this.categoryService.getCategoriesSimple().subscribe(
      (resp) => {
        this.categories = resp;
      },
      (error) => {
        this.alertify.error('Error occured while getting categories names');
      }
    );
  }

  applyFilters() {
    this.dialogRef.close(this.mainForm.value);
  }

  isOrdersValid(): boolean {
    const array = this.orderByForms.value;

    for (const element of array) {

      const lastOcc = array
        .map((el) => el.property.id)
        .lastIndexOf(element.property.id);

      const firstOcc = array
        .map((el) => el.property.id)
        .indexOf(element.property.id);

      if (firstOcc !== lastOcc) {
        this.ordersError = `${element.property.name} is already in orders list!`;
        return false;
      }
    }

    return true;
  }
}

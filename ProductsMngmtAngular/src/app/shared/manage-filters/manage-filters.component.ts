import { Component, OnInit, Inject } from '@angular/core';
import { FormGroup, FormBuilder, FormArray} from '@angular/forms';
import { CategorySimple } from 'src/app/models/category/category-simple';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CategoryService } from 'src/app/services/category.service';
import { AlertifyService } from '../../services/alertify.service';
import { PropertyToOrderBy, OrderProperty } from '../helpers/property-to-order-by';
import { OrderType, Order } from '../helpers/order-type';

@Component({
  selector: 'app-manage-filters',
  templateUrl: './manage-filters.component.html',
  styleUrls: ['./manage-filters.component.css'],
})
export class ManageFiltersComponent implements OnInit {
  constructor(
    @Inject(MAT_DIALOG_DATA) private data: any,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<ManageFiltersComponent>,
    private categoryService: CategoryService,
    private alertify: AlertifyService
  ) {}

  get orderByForms() {
    return this.mainForm.get('ordersBy') as FormArray;
  }

  properties: PropertyToOrderBy[];
  orders: OrderType[];
  categories: CategorySimple[];
  mainForm: FormGroup;
  ordersError: string;

  compareByValue(f1: any, f2: any) {
    return f1 && f2 && f1.id === f2.id;
  }

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

    if (this.data) {
      this.mainForm.patchValue(this.data);
      this.data.ordersBy.forEach(element => {
        const order = this.orders.find(x => x.id === element.order.id);
        this.addOrderBy(element.property, order);
      });
    }
  }


  addOrderBy(property = this.properties[0], order = this.orders[1]) {
    if (this.isOrdersValid()) {
      const orderByForm = this.fb.group({
        property,
        order
      });

      this.orderByForms.push(orderByForm);
    }
  }

  deleteOrderBy(i) {
    this.orderByForms.removeAt(i);
  }

  initializeProperties() {
    this.properties = [];
    this.properties.push(
      new PropertyToOrderBy(OrderProperty.ProductName, 'Product name')
    );
    this.properties.push(
      new PropertyToOrderBy(OrderProperty.CategoryName, 'Category name')
    );
    this.properties.push(
      new PropertyToOrderBy(OrderProperty.ExpirationDate, 'End date')
    );
  }

  initializeOrders() {
    this.orders = [];
    this.orders.push(new OrderType(Order.ASC, 'asc'));
    this.orders.push(new OrderType(Order.DESC, 'desc'));
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

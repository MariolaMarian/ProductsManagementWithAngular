import { OrderProperty } from './property-to-order-by';
import {
  IProductsFiltersForm,
  OrderBy,
} from './products-filters-form.interface';
import { FilterParam } from './filter-param';
import { Order } from './order-type';

export class ProductsFiltersParams {
  barecode: string;
  name: string;
  categories: number[];
  ordersBy: OrderBy[];

  constructor(productsFiltersForm: IProductsFiltersForm) {
    this.barecode = productsFiltersForm.productInfo.barecode;
    this.name = productsFiltersForm.productInfo.name;
    this.categories = productsFiltersForm.categoriesInfo.selectedCategories?.map(
      (c) => c.id
    );
    this.ordersBy = productsFiltersForm.ordersBy;
  }

  ReturnAsFilterParams(): FilterParam[] {
    const parameters: FilterParam[] = [];

    parameters.push(new FilterParam('barecode', this.barecode));
    parameters.push(new FilterParam('productName', this.name));
    this.categories?.forEach((item) => {
      parameters.push(new FilterParam('categoriesIds', item.toString()));
    });
    this.ordersBy?.forEach((item) => {
      let newParam: FilterParam;

      switch (item.property.id) {
        case OrderProperty.ProductName:
          parameters.push(new FilterParam('byProductName', 'true'));
          switch (item.order) {
            case Order.DESC:
              parameters.push(new FilterParam('byProductDesc', 'true'));
              break;
            case Order.DESC:
              parameters.push(new FilterParam('byProductDesc', 'false'));
              break;
          }
          break;
        case OrderProperty.CategoryName:
          parameters.push(new FilterParam('byCategoryName', 'true'));
          switch (item.order) {
            case Order.DESC:
              parameters.push(new FilterParam('byCategoryNameDesc', 'true'));
              break;
            case Order.DESC:
              parameters.push(new FilterParam('byCategoryNameDesc', 'false'));
              break;
          }
          break;
        case OrderProperty.ExpirationDate:
          parameters.push(new FilterParam('byExpDate', 'true'));
          switch (item.order) {
            case Order.DESC:
              parameters.push(new FilterParam('byExpDateDesc', 'true'));
              break;
            case Order.DESC:
              parameters.push(new FilterParam('byExpDateDesc', 'false'));
              break;
          }
          break;
      }

      parameters.push(newParam);
    });

    return parameters;
  }
}

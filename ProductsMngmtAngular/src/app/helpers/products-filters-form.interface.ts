import { PropertyToOrderBy } from './property-to-order-by';
import { CategorySimple } from '../models/category/category-simple';
import { Order } from './order-type';

export interface IProductsFiltersForm
{
    productInfo: ProductInfo;
    ordersBy: OrderBy[];
    categoriesInfo: CategoriesInfo;
}

interface ProductInfo
{
    barecode: string;
    name: string;
}

export interface OrderBy
{
    order: Order;
    property: PropertyToOrderBy;
}

interface CategoriesInfo
{
    selectedCategories: CategorySimple[];
}


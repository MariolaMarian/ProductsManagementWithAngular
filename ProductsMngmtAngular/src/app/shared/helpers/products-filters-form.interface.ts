import { PropertyToOrderBy } from './property-to-order-by';
import { Order } from './order-type';
import { CategorySimple } from 'src/app/models/category/category-simple';

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


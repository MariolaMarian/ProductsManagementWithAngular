import { IItemWithId } from '../../interfaces/item-with-id.interface';
import { Product } from '../product/product.model';

export class Category implements IItemWithId{
    id: number;
    name: string;
    products: Product[];
    productsCount: number;
    showProducts: boolean;

    constructor(){
    }
}

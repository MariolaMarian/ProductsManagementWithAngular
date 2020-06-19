import { Product } from './product.model';
import { IItemWithId } from '../interfaces/item-with-id.interface';

export class Category implements IItemWithId{
    id: number;
    name: string;
    products: Product[];
    productsCount: number;
    showProducts: boolean;

    constructor(){
    }
}

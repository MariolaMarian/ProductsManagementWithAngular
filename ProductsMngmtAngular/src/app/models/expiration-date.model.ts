import { Product } from '../products/models/product.model';
import { Employee } from './employee.model';
import { IItemWithId } from '../interfaces/item-with-id.interface';

export class ExpirationDate implements IItemWithId {
    id: number;
    product: Product;
    productId: number;
    endDate: Date;
    collected: boolean;
    collectedDate: Date;
    collectedBy: Employee;
    count: number;

    constructor(){
        this.endDate = new Date();
        this.endDate.setMonth(this.endDate.getMonth() + 3);
        this.collected = false;
    }
}

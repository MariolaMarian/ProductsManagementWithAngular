import { IItemWithId } from 'src/app/interfaces/item-with-id.interface';
import { Employee } from '../employee/employee.model';
import { Product } from '../product/product.model';


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

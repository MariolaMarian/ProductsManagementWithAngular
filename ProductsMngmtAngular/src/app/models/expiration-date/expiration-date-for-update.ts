import { IItemWithId } from '../../interfaces/item-with-id.interface';
import { ExpirationDate } from './expiration-date.model';

export class ExpirationDateForUpdate implements IItemWithId{
    id: number;
    count: number;

    constructor(expirationDate: ExpirationDate){
        this.id = expirationDate.id;
        this.count = expirationDate.count;
    }
}

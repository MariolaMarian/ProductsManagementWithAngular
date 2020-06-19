import { IItemWithId } from '../interfaces/item-with-id.interface';
import { ICategorySimple } from '../interfaces/category-simple.interface';

export class Employee implements IItemWithId {
    id: string;
    firstName: string;
    lastName: string;
    phone: string;
    userName: string;
    roles: string[];
    categories: ICategorySimple[];

    constructor(){
    }
}

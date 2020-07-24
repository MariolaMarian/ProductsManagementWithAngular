import { IItemWithId } from 'src/app/interfaces/item-with-id.interface';
import { ICategorySimple } from 'src/app/interfaces/category-simple.interface';

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

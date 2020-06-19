import { IItemWithId } from 'src/app/interfaces/item-with-id.interface';

export class CategorySimple implements IItemWithId {
    id: number;
    name: string;

    constructor(id: number, name: string){
        this.id = id;
        this.name = name;
    }
}

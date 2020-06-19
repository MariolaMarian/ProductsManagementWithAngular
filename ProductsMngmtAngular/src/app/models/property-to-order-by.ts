import { IItemWithId } from '../interfaces/item-with-id.interface';

export class PropertyToOrderBy implements IItemWithId {
  id: number;
  name: string;

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
  }
}

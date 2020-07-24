export class PropertyToOrderBy {
  id: OrderProperty;
  name: string;

  constructor(id: number, name: string) {
    this.id = id;
    this.name = name;
  }
}

export enum OrderProperty{
  ProductName = 1,
  CategoryName = 2,
  ExpirationDate = 3,
}

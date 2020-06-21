export class OrderType {
  id: Order;
  name: string;

  constructor(id: Order, name: string) {
    this.id = id;
    this.name = name;
  }
}

export enum Order {
  ASC = 1,
  DESC = 2,
}

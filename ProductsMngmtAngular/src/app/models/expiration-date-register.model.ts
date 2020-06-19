export class ExpirationDateForCreate {
  productId: number;
  endDate: string;

  constructor(productId: number, endDate: Date) {
    this.productId = productId;
    this.endDate = endDate.toDateString();
  }
}

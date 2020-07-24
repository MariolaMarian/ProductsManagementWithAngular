import { Category } from '../../models/category/category.model';
import { IItemWithId } from '../../interfaces/item-with-id.interface';
import { ExpirationDate } from 'src/app/models/expiration-date/expiration-date.model';

export class Product implements IItemWithId {
    id: number;
    name: string;
    barecode: string;
    categoryId: number;
    category: Category;
    image: string | ArrayBuffer;
    maxDays: number;
    nearestDate: Date; 
    expirationDates: ExpirationDate[];

    displayImage(): string
    {
        if (this.image == null)
        {
            return 'https://i5.walmartimages.ca/images/Large/799/2_r/6000196087992_R.jpg';
        }

        if (this.image.toString().includes('http'))
        {
            return this.image.toString();
        }

        return  'data:image/jpeg;base64,' + this.image;
    }

    constructor(product?: Product){
        this.maxDays = 7;
        this.barecode = '5901478006981';
        this.expirationDates = [];

        if (product){
            this.id = product.id;
            this.name = product.name;
            this.barecode = product.barecode;
            this.categoryId = product.categoryId;
            this.category = product.category;
            this.image = product.image;
            this.maxDays = product.maxDays;
            this.expirationDates = product.expirationDates;
            this.nearestDate = product.nearestDate;
        }
    }

}

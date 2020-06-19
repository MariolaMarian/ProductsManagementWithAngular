import { IEmployeePersonalData } from './employee-personal-data.interface';
import { IEmployeeRoles } from './employee-roles.interface';
import { ICategorySimple } from './category-simple.interface';

export interface IEmployee extends IEmployeePersonalData
{
    roles: string[];
    categories: ICategorySimple[];
}
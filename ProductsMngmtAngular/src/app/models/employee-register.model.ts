import { IEmployeeRoles } from '../interfaces/employee-roles.interface';
import { CategorySimple } from './category/category-simple';
import { IEmployeePersonalData } from '../interfaces/employee-personal-data.interface';
import { IEmployee } from '../interfaces/employee.interface';

export class EmployeeForRegistration {
  id: string;
  firstName: string;
  lastName: string;
  userName: string;
  password: string;
  phoneNumber: string;
  isManager: boolean;
  isTeamLeader: boolean;
  categories: number[];

  constructor() {}

  static createEmployeeFromRegisterData(
    personalData: IEmployeePersonalData,
    roles: IEmployeeRoles,
    selectedCategories: CategorySimple[]
  ): EmployeeForRegistration {
    const newEmployee: EmployeeForRegistration = {
      id: personalData.id,
      firstName: personalData.firstName,
      lastName: personalData.lastName,
      userName: personalData.userName,
      password: personalData.password,
      phoneNumber: personalData.phone,
      isManager: roles.isManager,
      isTeamLeader: roles.isTeamLeader,
      categories: selectedCategories.map((c) => c.id),
    };

    return newEmployee;
  }

  static CreateEmployeeFromEmployeeInterface(employee: IEmployee) {
    const newEmployee: EmployeeForRegistration = {
      id: employee.id,
      firstName: employee.firstName,
      lastName: employee.lastName,
      userName: employee.userName ?? '',
      password: '',
      phoneNumber: employee.phone ?? '',
      isManager: employee.roles.indexOf('Manager') > -1 ? true : false,
      isTeamLeader: employee.roles.indexOf('TeamLeader') > -1 ? true : false,
      categories: employee.categories.map((c) => c.id),
    };
    return newEmployee;
  }
}

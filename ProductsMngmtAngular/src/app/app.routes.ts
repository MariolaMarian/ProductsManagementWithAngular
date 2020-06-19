import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { ProductsListComponent } from './products/products-list/products-list.component';
import { CategoriesListComponent } from './categories/categories-list/categories-list.component';
import { ExpirationDatesListComponent } from './expiration-dates/expiration-dates-list/expiration-dates-list.component';
import { EmployeesListComponent } from './employees/employees-list/employees-list.component';
import { ReportsListComponent } from './reports/report/reports-list/reports-list.component';
import { AuthGuard } from './guards/auth.guard';
import { CategoryComponent } from './categories/category/category.component';
import { CategoryResolver } from './resolvers/category.resolver';
import { CategoriesListResolver } from './resolvers/categories-list.resolver';
import { ProductsListResolver } from './resolvers/products-list.resolver';
import { ProductComponent } from './products/product/product.component';
import { ProductResolver } from './resolvers/product.resolver';
import { ExpirationDatesListResolver } from './resolvers/expiration-dates.resolver';
import { ExpirationDateResolver } from './resolvers/expiration-date.resolver';
import { EmployeeListResolver } from './resolvers/employee-list.resolver';

export const appRoutes: Routes = [
  { path: '', component: HomeComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'products', component: ProductsListComponent, resolve: {list: ProductsListResolver}},
      { path: 'products/:id', component: ProductComponent, resolve: {data: ProductResolver}},
      { path: 'categories', component: CategoriesListComponent, resolve: {list: CategoriesListResolver}},
      { path: 'categories/:id', component: CategoryComponent, resolve: {category: CategoryResolver}},
      { path: 'expirationDates', component: ExpirationDatesListComponent, resolve: {list : ExpirationDatesListResolver}},
      { path: 'reports', component: ReportsListComponent},
      { path: 'employees', component: EmployeesListComponent, resolve: {employees: EmployeeListResolver}},
    ],
  },
  { path: '**', redirectTo: '', pathMatch: 'full'},
];

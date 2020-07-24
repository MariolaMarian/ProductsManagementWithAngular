import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { RouterModule } from '@angular/router';
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';
import { JwtModule } from '@auth0/angular-jwt';
import { ReactiveFormsModule } from '@angular/forms';

import { appRoutes } from './app.routes';
import { CategoryResolver } from './resolvers/category.resolver';

import { ErrorInterceptorProvider } from './services/error.interceptor';
import { AuthorizationService } from './services/authorization.service';

// import { NgxBarcodeModule } from 'ngx-barcode';
import { NgxBarcode6Module } from 'ngx-barcode6';
import { ZXingScannerModule } from '@zxing/ngx-scanner';
import { PaginationModule } from 'ngx-bootstrap/pagination';
import { NgxLoadingModule } from 'ngx-loading';

import { NgbModule } from '@ng-bootstrap/ng-bootstrap';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { HomeComponent } from './home/home.component';
import { CategoryComponent } from './categories/category/category.component';
import { CategoriesListComponent } from './categories/categories-list/categories-list.component';
import { ProductComponent } from './products/product/product.component';
import { ProductsListComponent } from './products/products-list/products-list.component';
import { ExpirationDatesListComponent } from './expiration-dates/expiration-dates-list/expiration-dates-list.component';
import { EmployeeComponent } from './employees/employee/employee.component';
import { EmployeesListComponent } from './employees/employees-list/employees-list.component';
import { ReportsListComponent } from './reports/report/reports-list/reports-list.component';
import { CategoriesListResolver } from './resolvers/categories-list.resolver';
import { ProductEditComponent } from './products/product-edit/product-edit.component';
import { CategoryEditComponent } from './categories/category-edit/category-edit.component';
import { ProductResolver } from './resolvers/product.resolver';
import { ProductsListResolver } from './resolvers/products-list.resolver';
import { ProductSmallComponent } from './products/product-small/product-small.component';
import { ExpirationDateAddComponent } from './expiration-dates/expiration-date-add/expiration-date-add.component';
import { ExpirationDatesListResolver } from './resolvers/expiration-dates.resolver';
import { ExpirationDateResolver } from './resolvers/expiration-date.resolver';
import { LoginComponent } from './login/login.component';
import { ExpirationDateSmallComponent } from './expiration-dates/expiration-date-small/expiration-date-small.component';
import { ExpirationDateInProductComponent } from './expiration-dates/expiration-date-in-product/expiration-date-in-product.component';
import { SafePipe } from './helpers/safe.pipe';
import { ContactMeComponent } from './contact-me/contact-me.component';
import { ConfirmationComponent } from './confirmation/confirmation.component';
import { ManageFiltersComponent } from './filters/manage-filters.component';
import { MAT_FORM_FIELD_DEFAULT_OPTIONS } from '@angular/material/form-field';
import { AppMaterialModule } from './app-material/app-material.module';
import { EmployeeForListComponent } from './employees/employee-for-list/employee-for-list.component';
import { EmployeeListResolver } from './resolvers/employee-list.resolver';
import { EmployeeRegistrationComponent } from './employees/employee-registration/employee-registration.component';

export function tokenGetter(){
   return localStorage.getItem('token');
}

@NgModule({
   declarations: [
      AppComponent,
      CategoryComponent,
      CategoriesListComponent,
      CategoryEditComponent,
      NavComponent,
      HomeComponent,
      ProductComponent,
      ProductsListComponent,
      ProductEditComponent,
      ExpirationDatesListComponent,
      EmployeeComponent,
      EmployeesListComponent,
      ReportsListComponent,
      ProductSmallComponent,
      ExpirationDateAddComponent,
      LoginComponent,
      ExpirationDateSmallComponent,
      ExpirationDateInProductComponent,
      ManageFiltersComponent,
      SafePipe,
      ContactMeComponent,
      ConfirmationComponent,
      EmployeeForListComponent,
      EmployeeRegistrationComponent,
   ],
   imports: [
      BrowserModule,
      HttpClientModule,
      FormsModule,
      ReactiveFormsModule,
      NgxBarcode6Module,
      ZXingScannerModule,
      NgxLoadingModule.forRoot({}),
      NgbModule,
      AppMaterialModule,
      PaginationModule.forRoot(),
      BrowserAnimationsModule,
      BsDropdownModule.forRoot(),
      RouterModule.forRoot(appRoutes),
      JwtModule.forRoot({
         config: {
            tokenGetter,
            whitelistedDomains: ['localhost:5000'],
            blacklistedRoutes: ['localhost:5000/api/auth']
         }
      })
   ],
   providers: [
      ErrorInterceptorProvider,
      AuthorizationService,
      CategoryResolver,
      CategoriesListResolver,
      ProductResolver,
      ProductsListResolver,
      ExpirationDateResolver,
      ExpirationDatesListResolver,
      EmployeeListResolver,
      {provide: MAT_FORM_FIELD_DEFAULT_OPTIONS, useValue: {floatLabel: 'auto'}}
   ],
   entryComponents: [
      ManageFiltersComponent,
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }

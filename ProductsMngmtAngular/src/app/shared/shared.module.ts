import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppMaterialModule } from '../app-material/app-material.module';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { ManageFiltersComponent } from './manage-filters/manage-filters.component';
import { SafePipe } from './helpers/safe.pipe';

@NgModule({
  declarations: [ManageFiltersComponent, SafePipe],
  imports: [CommonModule, AppMaterialModule, FormsModule, ReactiveFormsModule],
  entryComponents: [ManageFiltersComponent],
  exports: [
    ManageFiltersComponent,
    AppMaterialModule,
    FormsModule,
    ReactiveFormsModule,
    SafePipe,
  ],
})
export class SharedModule {}

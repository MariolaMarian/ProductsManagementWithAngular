import {
  Component,
  OnInit,
  ViewChild,
  Output,
  EventEmitter,
} from '@angular/core';
import { Category } from 'src/app/models/category/category.model';
import { CategoryService } from 'src/app/services/category.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { NgForm } from '@angular/forms';
import { MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-category-edit',
  templateUrl: './category-edit.component.html',
  styleUrls: ['./category-edit.component.css'],
})
export class CategoryEditComponent implements OnInit {
  @ViewChild('categoryForm') categoryForm: NgForm;
  category: Category;

  constructor(
    private categoryService: CategoryService,
    private alertify: AlertifyService,
    private addDialogRef: MatDialogRef<CategoryEditComponent>
  ) {}

  ngOnInit() {
    this.category = new Category();
  }

  saveCategory() {
    this.categoryForm.controls.name.setErrors(null);
    this.categoryService.getCategories().subscribe(
      (data: Category[]) => {
        if (data.find((c) => c.name === this.category.name)) {
          this.alertify.error('Category with this name already exists!');
          this.categoryForm.controls.name.setErrors({ incorrect: true });
        } else {
          this.categoryService.postCategory(this.category).subscribe(
            () => {
              this.alertify.success('Category registered!');
              this.addDialogRef.close(this.category);
            },
            (error) => {
              this.alertify.error(error);
            }
          );
        }
      },
      () => {
        this.alertify.error('Error during checking if category already exists');
      }
    );
  }
}

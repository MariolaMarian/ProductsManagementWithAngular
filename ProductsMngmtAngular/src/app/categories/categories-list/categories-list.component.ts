import { OnInit, Component } from '@angular/core';
import { Category } from 'src/app/models/category.model';
import { ActivatedRoute } from '@angular/router';
import { CategoryService } from 'src/app/services/category.service';
import { ThrowStmt } from '@angular/compiler';
import { AlertifyService } from 'src/app/services/alertify.service';
import { MatDialog } from '@angular/material/dialog';
import { ConfirmationComponent } from 'src/app/confirmation/confirmation.component';
import { CategoryEditComponent } from '../category-edit/category-edit.component';

@Component({
  selector: 'app-categories-list',
  templateUrl: './categories-list.component.html',
  styleUrls: ['./categories-list.component.css'],
})
export class CategoriesListComponent implements OnInit {
  categories: Category[];
  highlightedId: number;

  constructor(
    private route: ActivatedRoute,
    private categoryService: CategoryService,
    private alertify: AlertifyService,
    private dialog: MatDialog
  ) {}

  ngOnInit() {
    this.route.data.subscribe((data) => {
      this.categories = data.list;
    });
  }

  loadCategories() {
    this.categoryService.getCategories().subscribe((data) => {
      this.categories = data;
    });
  }

  deleteCategory(category: Category) {
    const deleteCategoryDialogRef = this.dialog.open(ConfirmationComponent, {
      data: `category ${category.name}`,
    });

    deleteCategoryDialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.deleteCategoryConfirmed(category);
      }
    });
  }

  deleteCategoryConfirmed(category: Category) {
    this.categoryService.deleteCategory(category.id).subscribe(
      () => {
        this.alertify.success(
          'Category ' + category.name + ' deleted succesfully'
        );
        this.categories = this.categories.filter((c) => c.id !== category.id);
      },
      (error) => {
        this.alertify.error(error);
      }
    );
  }

  highlight(categoryId: number) {
    this.highlightedId = categoryId;
  }

  showAddCategoryDialog() {
    const addCategoryDialogRef = this.dialog.open(CategoryEditComponent, {});

    addCategoryDialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.loadCategories();
      }
    });
  }
}

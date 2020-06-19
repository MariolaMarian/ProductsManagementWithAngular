import {
  Component,
  OnInit,
  ViewChild,
  Inject,
} from '@angular/core';
import { CategoryService } from 'src/app/services/category.service';
import { Category } from 'src/app/models/category.model';
import { AlertifyService } from 'src/app/services/alertify.service';
import { Product } from 'src/app/models/product.model';
import { ProductService } from 'src/app/services/product.service';
import { NgForm } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';

@Component({
  selector: 'app-product-edit',
  templateUrl: './product-edit.component.html',
  styleUrls: ['./product-edit.component.css'],
})
export class ProductEditComponent implements OnInit {
  product: Product;
  categories: Category[];
  imagePreview: string | ArrayBuffer;

  constructor(
    private categoryService: CategoryService,
    private alertify: AlertifyService,
    private productService: ProductService,
    private dialogRef: MatDialogRef<ProductEditComponent>,
    @Inject(MAT_DIALOG_DATA) public data: Product
  ) {
    this.categories = [];
  }

  ngOnInit() {
    this.product = new Product(this.data) ?? new Product();
    this.imagePreview = this.product.displayImage();
    this.loadCategories(); //
  }

  loadCategories() {
    this.categoryService.getCategoriesSimple().subscribe(
      (data) => {
        this.categories = data;
      },
      (error) => {
        this.alertify.error('Error while retriving categories');
      }
    );
  }

  saveProduct() {
    this.product.image = this.imagePreview;

    if (this.product.id) {
      this.productService.put(this.product).subscribe(
        (resp) => {
          this.alertify.success('Product edited');
          this.dialogRef.close(this.product);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
    } else {
      this.productService.post(this.product).subscribe(
        (resp) => {
          this.alertify.success('Product saved');
          this.dialogRef.close(this.product);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
    }
  }

  fileUploaded(event) {
    const files: FileList = event.target.files;
    if (files.length > 0) {
      const file: File = files[0];

      const reader = new FileReader();
      reader.onload = (e) => (this.imagePreview = reader.result);

      reader.readAsDataURL(file);
    }
  }
}

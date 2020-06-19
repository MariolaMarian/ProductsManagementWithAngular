import { Component, OnInit, Input, Inject } from '@angular/core';
import { CategorySimple } from 'src/app/models/category/category-simple';
import {
  FormGroup,
  FormBuilder,
  Validators,
  FormControl,
} from '@angular/forms';
import { EmployeeForRegistration } from 'src/app/models/employee-register.model';
import { AuthorizationService } from 'src/app/services/authorization.service';
import { CategoryService } from 'src/app/services/category.service';
import { AlertifyService } from 'src/app/services/alertify.service';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { cantContainValidator } from 'src/app/validators/cantContain.validator';
import { noEmptySpacesValidator } from 'src/app/validators/noEmptySpaces.validator';
import { IEmployee } from 'src/app/interfaces/employee.interface';
import { Employee } from 'src/app/models/employee.model';

@Component({
  selector: 'app-employee-registration',
  templateUrl: './employee-registration.component.html',
  styleUrls: ['./employee-registration.component.css'],
})
export class EmployeeRegistrationComponent implements OnInit {
  hide: boolean;
  categoriesToSelect: CategorySimple[];
  registerForm: FormGroup;

  constructor(
    private authService: AuthorizationService,
    private alertify: AlertifyService,
    private categoryService: CategoryService,
    private fb: FormBuilder,
    private dialogRef: MatDialogRef<EmployeeRegistrationComponent>,
    @Inject(MAT_DIALOG_DATA) public data: IEmployee
  ) {}

  ngOnInit() {
    this.hide = true;
    this.initializeCategories();

    const personalDataForm = this.fb.group({
      id: [''],
      firstName: ['', [Validators.required]],
      lastName: ['', [Validators.required]],
      phone: ['', [Validators.required, Validators.pattern('[0-9]{9}')]],
      userName: [
        '',
        [
          Validators.required,
          cantContainValidator(/admin/i),
          cantContainValidator(/root/i),
          noEmptySpacesValidator(),
        ],
      ],
      password: [
        '',
        [
          Validators.required,
          Validators.pattern(
            '(?=.*[a-z])(?=.*[A-Z])(?=.*[$@$!%*?&])[A-Za-zd$@$!%*?&].{7,}'
          ),
        ],
      ],
    });

    const categoriesForm = this.fb.group({
      categories: [],
    });

    const rolesForm = this.fb.group({
      isManager: false,
      isTeamLeader: false,
    });

    this.registerForm = this.fb.group({
      personalData: personalDataForm,
      selectedCategories: categoriesForm,
      roles: rolesForm,
    });

    this.isManager.valueChanges.subscribe((value) => {
      if (value) {
        this.selectedCategories.patchValue(this.categoriesToSelect);
        this.selectedCategories.disable();
      } else {
        this.selectedCategories.patchValue([]);
        this.selectedCategories.enable();
      }
    });

    if (this.data) {
      this.registerForm.controls.personalData.patchValue(this.data);
      const rolesTmp = {};
      if (this.data.roles.indexOf('Manager') > -1) {
        Object.assign(rolesTmp, { isManager: true });
      }
      if (this.data.roles.indexOf('TeamLeader') > -1) {
        Object.assign(rolesTmp, { isTeamLeader: true });
      }
      this.registerForm.controls.roles.patchValue(rolesTmp);
      this.selectedCategories.patchValue(this.data.categories);
      this.userName.clearValidators();
      this.password.clearValidators();
    }
  }

  register() {
    const formValue = this.registerForm.getRawValue();

    const newEmployee = EmployeeForRegistration.createEmployeeFromRegisterData(
      formValue.personalData,
      formValue.roles,
      formValue.selectedCategories.categories
    );

    if (this.data) {
      this.authService.update(newEmployee).subscribe(
        response => {
          this.alertify.success('Employee updated!');
          this.dialogRef.close(response as Employee);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
    } else {
      this.authService.register(newEmployee).subscribe(
        response => {
          this.alertify.success('Employee registered!');
          this.dialogRef.close(response as Employee);
        },
        (error) => {
          this.alertify.error(error);
        }
      );
    }
  }

  initializeCategories() {
    this.categoryService.getCategoriesSimple().subscribe(
      (resp) => {
        this.categoriesToSelect = resp;
      },
      (error) => {
        this.alertify.error('Error occured while getting categories names');
      }
    );
  }

  get firstName() {
    return this.registerForm.controls.personalData.get('firstName');
  }

  get lastName() {
    return this.registerForm.controls.personalData.get('lastName');
  }

  get userName() {
    return this.registerForm.controls.personalData.get('userName');
  }

  get password() {
    return this.registerForm.controls.personalData.get('password');
  }

  get phone() {
    return this.registerForm.controls.personalData.get('phone');
  }

  get isManager() {
    return this.registerForm.controls.roles.get('isManager');
  }

  get selectedCategories() {
    return this.registerForm.controls.selectedCategories.get('categories');
  }

  compareCategories(c1: CategorySimple, c2: CategorySimple) {
    return c1 && c2 ? c1.id === c2.id : c1 === c2;
  }
}

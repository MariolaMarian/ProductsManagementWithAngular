import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { EmployeeRegistrationComponent } from '../employee-registration/employee-registration.component';
import { Employee } from 'src/app/models/employee/employee.model';

@Component({
  selector: 'app-employees-list',
  templateUrl: './employees-list.component.html',
  styleUrls: ['./employees-list.component.css'],
})
export class EmployeesListComponent implements OnInit {
  constructor(private dialog: MatDialog, private route: ActivatedRoute) {}

  employees: Employee[];

  ngOnInit(): void {
    this.route.data.subscribe((data) => {
      this.employees = data.employees;
    });
  }

  showRegisterEmployeeDialog() {
    const addEmployeeDialogRef = this.dialog.open(
      EmployeeRegistrationComponent,
      {}
    );

    addEmployeeDialogRef.afterClosed().subscribe((result) => {
      if (result) {
        const employee = result as Employee
        this.employees.push(employee);
      }
    });
  }
}

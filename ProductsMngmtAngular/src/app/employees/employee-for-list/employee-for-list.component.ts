import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { EmployeeRegistrationComponent } from '../employee-registration/employee-registration.component';
import { IEmployee } from 'src/app/interfaces/employee.interface';
import { UserService } from 'src/app/services/user.service';
import { Subscription } from 'rxjs';
import { Employee } from 'src/app/models/employee/employee.model';

@Component({
  selector: 'app-employee-for-list',
  templateUrl: './employee-for-list.component.html',
  styleUrls: ['./employee-for-list.component.css'],
})
export class EmployeeForListComponent implements OnInit, OnDestroy {
  @Input() employee: Employee;
  private subscriptions = new Subscription();

  constructor(private dialog: MatDialog, private userService: UserService) {}

  ngOnInit(): void {
    this.employee.roles.sort();
  }

  showIcon(role: string) {
    switch (role) {
      case 'Admin':
        return '&#128081;';
      case 'Manager':
        return '&#x265C;';
      case 'TeamLeader':
        return '&#128221;';
      default:
        return '&#128100;';
    }
  }

  openEditDialog() {
    const editDialogRef = this.dialog.open(EmployeeRegistrationComponent, {
      data: this.employee,
    });

    editDialogRef.afterClosed().subscribe((value: IEmployee) => {
      if (value) {
        this.userService.getUser(value.id).subscribe((resp) => {
          this.employee = resp;
        });
      }
    });
  }

  ngOnDestroy(){
    this.subscriptions.unsubscribe();
  }
}

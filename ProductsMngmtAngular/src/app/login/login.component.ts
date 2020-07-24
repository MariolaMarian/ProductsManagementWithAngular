import { Component, OnInit, ViewChild } from '@angular/core';
import { AlertifyService } from '../services/alertify.service';
import { Router } from '@angular/router';
import { EmployeeLogin } from '../models/employee/employee-login.model';
import { AuthorizationService } from '../services/authorization.service';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Form, NgForm } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  @ViewChild('modalContent', {static: false}) modal: ModalDirective;
  employeeLoginModel: EmployeeLogin;

  constructor(public authService: AuthorizationService, private alertify: AlertifyService,
              private router: Router, private modalService: NgbModal) { }

  ngOnInit() {
    this.employeeLoginModel = new EmployeeLogin();
  }

  open(modalContent){
    this.modalService.open(modalContent, {ariaLabelledBy: 'loginModalTitle'});
  }

  login(){
    this.authService.login(this.employeeLoginModel).subscribe(() => {
      this.employeeLoginModel = new EmployeeLogin();
      this.modalService.dismissAll();
      this.alertify.success('Successfully logged');
      this.router.navigate(['/expirationDates']);
    }, error => {
      this.alertify.error(error);
    });
  }

  loggedIn(): boolean {
    return this.authService.loggedIn();
  }

  logout(){
    localStorage.removeItem('token');
    localStorage.removeItem('roles');
    localStorage.removeItem('username');
    this.alertify.message('logged out');
    this.router.navigate(['/home']);
  }

}

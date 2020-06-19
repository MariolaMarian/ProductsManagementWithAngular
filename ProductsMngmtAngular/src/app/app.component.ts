import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from './services/authorization.service';
import { JwtHelperService } from '@auth0/angular-jwt';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  jwtHelper = new JwtHelperService();

  constructor(private authService: AuthorizationService){}

  ngOnInit(){
    const token = localStorage.getItem('token');
  }
}

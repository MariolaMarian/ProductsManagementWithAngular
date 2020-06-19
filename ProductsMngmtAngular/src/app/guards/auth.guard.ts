import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { AuthorizationService } from '../services/authorization.service';
import { AlertifyService } from '../services/alertify.service';

@Injectable({
  providedIn: 'root'
})
export class AuthGuard implements CanActivate {
  constructor(private authService: AuthorizationService, private router: Router, private alertify: AlertifyService){}

  canActivate(): boolean {
    if (this.authService.loggedIn()){
      return true;
    }

    this.alertify.error('Log in to see this page!');
    this.router.navigate(['']);
    return false;
  }
}

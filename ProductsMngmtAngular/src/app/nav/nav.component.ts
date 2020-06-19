import { Component, OnInit } from '@angular/core';
import { AuthorizationService } from '../services/authorization.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})
export class NavComponent implements OnInit {

  isMenuCollapsed: boolean;

  constructor(public authService: AuthorizationService, private router: Router) {
    router.events.subscribe(event => {
      this.isMenuCollapsed = true;
    });
  }

  ngOnInit() {
    this.isMenuCollapsed = true;
  }

  loggedIn(): boolean {
    return this.authService.loggedIn();
  }

}

import { Component, OnInit } from '@angular/core';
import { ContactMe } from '../models/contact-me.model';

@Component({
  selector: 'app-contact-me',
  templateUrl: './contact-me.component.html',
  styleUrls: ['./contact-me.component.css']
})
export class ContactMeComponent implements OnInit {

  contactMeData: ContactMe;

  constructor() { }

  ngOnInit(): void {
  }

  sendContactMe()
  {

  }

}

import { Component, OnInit } from '@angular/core';
import {AuthorizeService} from "../../api-authorization/authorize.service";

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  isAdmin = false;
  description = '';

  constructor(private authorizeService: AuthorizeService) { }

  ngOnInit(): void {

    this.authorizeService.hasRole('Admin').subscribe(isAdmin => {
      this.isAdmin = isAdmin;
    });


    const storedDescription = localStorage.getItem('aboutDescription');
    if (storedDescription) {
      this.description = storedDescription;
    }
  }

  submit(): void {
    localStorage.setItem('aboutDescription', this.description);
  }
}

import {Component, Inject, OnInit} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";
import {AuthorizeService} from "../../api-authorization/authorize.service";
import {ApiResponse, UrlModel} from "../models/url";

@Component({
  selector: 'app-urls-table-view',
  templateUrl: './urls-table-view.component.html',
  styleUrls: ['./urls-table-view.component.css']
})
export class UrlsTableViewComponent {
  urls: UrlModel[] = [];
  isAuthenticated?: Observable<boolean>;
  http: HttpClient;
  baseUrl: any;
  originalUrl: string = '';

  constructor(http: HttpClient,
              @Inject('BASE_URL') baseUrl: string,
              private authorizeService: AuthorizeService)
  {
    http.get<UrlModel[]>(baseUrl + 'url').subscribe({
      next: result => {
        this.urls = result;
      },
      error: error => console.error(error),
    });

    this.isAuthenticated = this.authorizeService.isAuthenticated();
    this.http = http;
    this.baseUrl = baseUrl;
  }

  deleteAction(urlId: number): void {
    this.http.delete(this.baseUrl + 'url/' + urlId).subscribe({
      next: result => {
        console.log('Delete successful');
        this.urls = this.urls.filter(url => url.id !== urlId);
      },
      error: (error) => console.error(error),
    });
  }

  submitAction(event: Event): void {
    event.preventDefault();
    if (this.originalUrl) {
      this.http.post<ApiResponse<UrlModel>>(this.baseUrl + 'url', { originalUrl: this.originalUrl }).subscribe({
        next: (newUrl) => {
          console.log('Shorten successful');
          this.urls.push(newUrl.value);
          this.originalUrl = '';
        },
        error: (error) => {
          if (error.status === 400) {
            alert("This URL already exists");
          }
        },
      });
    }
  }
}

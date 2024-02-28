import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";

@Component({
  selector: 'app-redirect',
  templateUrl: './redirect.component.html',
  styleUrls: ['./redirect.component.css']
})
export class RedirectComponent {
  http: HttpClient;
  baseUrl: any;
  shortenedUrl: string = '';
  originalUrl: string | null = null;

  constructor(http: HttpClient,
              @Inject('BASE_URL') baseUrl: string) {
    this.http = http;
    this.baseUrl = baseUrl;
  }

  submitAction(event: Event): void {
    event.preventDefault();
    if (this.shortenedUrl) {
      this.http.get(`${this.baseUrl}url/find/${this.shortenedUrl}`, { responseType: 'text' })
        .subscribe({
          next: (result) => {
            this.originalUrl = result;
            this.shortenedUrl = '';
          },
          error: (error) => {
            if (error.status === 404) {
              alert("URL not found");
            }
            this.originalUrl = '';
          },
        });
    }
  }
}

import {Component, Inject} from '@angular/core';
import {HttpClient} from "@angular/common/http";
import {UrlModel} from "../models/url";
import {ActivatedRoute, Router} from "@angular/router";

@Component({
  selector: 'app-urls-info',
  templateUrl: './urls-info.component.html',
  styleUrls: ['./urls-info.component.css']
})
export class UrlsInfoComponent {
  urlId!: number;
  url!: UrlModel;

  constructor(http: HttpClient,
              @Inject('BASE_URL') baseUrl: string,
              private route: ActivatedRoute,
              private router: Router) {

    route.params.subscribe((p) => {
      this.urlId = +p['id'];
      if (isNaN(this.urlId) || this.urlId <= 0) {
        router.navigate(['/urls-table-view']);
        return;
      }
    });

    http.get<UrlModel>(baseUrl + 'url/' + this.urlId).subscribe({
      next: (result:any) => {
        this.url = result[0];
        console.log(this.url);
      },
      error: error => console.error(error),
    });
  }
}

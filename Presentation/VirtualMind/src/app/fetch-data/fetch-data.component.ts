import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public purchases: Purchases[];

  constructor(http: HttpClient, @Inject('BASE_URL') baseUrl: string) {
    http.get<Purchases[]>(baseUrl + 'api/exchange/purchases').subscribe(result => {
      this.purchases = result;
    }, error => console.error(error));
  }
}

interface Purchases {
  createdAt: string;
  id: number;
  userId: string;
  amount: number;
  currency: string;
  active: boolean;
}

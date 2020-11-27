import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

@Component({
  selector: 'app-quote',
  templateUrl: './quote.component.html',
  styleUrls: ['./quote.component.css']
})
export class QuoteComponent implements OnInit {

  public quotes: CurrencyQuote;
  private baseUrl: string;
  private http: HttpClient;
  public loading: boolean = true;

  constructor(_http: HttpClient, @Inject('BASE_URL') _baseUrl: string) {
    this.baseUrl = _baseUrl;
    this.http = _http;

    this.doRequest();

  }

  doRequest() {
    this.loading = true;

    this.http.get<CurrencyQuote>(this.baseUrl + 'api/exchange/quote', {
      responseType: "json"
    }).subscribe(result => {
      this.loading = false;
      this.quotes = result;
    }, error => console.error(error));
  }

  public refreshQuotes() {
    this.doRequest();
  }

  ngOnInit() {
  }
}

interface CurrencyQuote {
  currency: string;
  quote: number;
}


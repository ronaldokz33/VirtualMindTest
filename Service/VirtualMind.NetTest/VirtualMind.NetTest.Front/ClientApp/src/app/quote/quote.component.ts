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

  constructor(_http: HttpClient, @Inject('BASE_URL') _baseUrl: string) {
    this.baseUrl = _baseUrl;
    this.http = _http;

    this.doRequest();

  }

  doRequest() {
    this.http.get<CurrencyQuote>(this.baseUrl + 'api/exchange/quote', {
      responseType: "json"
    }).subscribe(result => {
      console.log('result: ', result)
      this.quotes = result;
      console.log('this.quotes: ', this.quotes);
    }, error => console.error(error));
  }

  ngOnInit() {
  }
}

interface CurrencyQuote {
  currency: string;
  quote: number;
}


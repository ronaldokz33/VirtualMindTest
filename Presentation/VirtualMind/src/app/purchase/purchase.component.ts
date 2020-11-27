import { HttpClient } from '@angular/common/http';
import { Component, Inject } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import Swal from 'sweetalert2'

@Component({
  selector: 'app-purchase-component',
  templateUrl: './purchase.component.html'
})
export class PurchaseComponent {
  public formGroup: FormGroup;
  public loading: boolean = false;

  constructor(
    private formBuilder: FormBuilder,
    private router: Router,
    private route: ActivatedRoute,
    private http: HttpClient,
    @Inject('BASE_URL') private baseUrl: string) {

    this.formGroup = this.formBuilder.group({
      userId: ['', Validators.required],
      currency: ['', Validators.required],
      amount: ['', Validators.required]
    });

  }

  public createPurchase() {
    const body: Purchase = {
      userId: this.formGroup.controls.userId.value,
      currency: this.formGroup.controls.currency.value,
      amount: this.formGroup.controls.amount.value
    }

    console.log('body: ', body)

    this.http.post<Purchase>(this.baseUrl + 'api/exchange/purchase', body, {
      responseType: "json"
    }).subscribe(result => {
      Swal.fire('Concluded', 'Order successfully sent!', 'success').then((result) => {
        this.router.navigate(['purchases']);
      });

      this.loading = false;
    }, ex => {
      Swal.fire('Oops...', ex.error.message, 'error')
    });


  }
}

interface Purchase {
  id?: number;
  userId: string;
  currency: string;
  amount: number;
}

import {Component, DestroyRef} from '@angular/core';
import {BalanceRequestService} from "../../../api";

@Component({
  selector: 'app-add-balance',
  templateUrl: './add-balance.component.html',
})
export class AddBalanceComponent {
  amount = 0;

  constructor(private readonly balanceRequest: BalanceRequestService, private readonly destroyRef: DestroyRef) {
  }

  async save(): Promise<void> {
    this.balanceRequest.createBalanceRequest({amount: 0});
  }
}

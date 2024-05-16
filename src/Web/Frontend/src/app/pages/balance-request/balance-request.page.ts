import { ChangeDetectionStrategy, Component, effect, inject } from '@angular/core';
import { injectMutation, injectQuery, injectQueryClient } from '@ngneat/query';
import { BalanceRequestService } from '../../../api';
import {
  IonContent,
  IonHeader,
  IonItem,
  IonItemOption,
  IonItemOptions,
  IonItemSliding,
  IonLabel,
  IonList,
  IonNote,
  IonRefresher,
  IonRefresherContent,
  IonSkeletonText,
  IonText,
  IonTitle,
  IonToolbar,
  RefresherCustomEvent,
} from '@ionic/angular/standalone';
import { TranslateModule } from '@ngx-translate/core';

@Component({
  selector: 'app-balance-request-page',
  templateUrl: './balance-request.page.html',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    IonContent,
    IonHeader,
    IonToolbar,
    IonTitle,
    TranslateModule,
    IonList,
    IonItem,
    IonLabel,
    IonSkeletonText,
    IonText,
    IonItemSliding,
    IonNote,
    IonItemOptions,
    IonItemOption,
    IonRefresher,
    IonRefresherContent,
  ],
})
export class BalanceRequestPage {
  #query = injectQuery();
  #queryClient = injectQueryClient();
  #mutation = injectMutation();

  protected readonly Array = Array;
  protected readonly balanceRequests = this.#query({
    queryFn: () => this.balanceRequest.getBalanceRequests(),
    queryKey: ['balanceRequests'],
  }).result;

  private readonly handleBalanceRequest = this.#mutation({
    mutationFn: ({ id, isApproved }: { id: number; isApproved: boolean }) =>
      this.balanceRequest.approve(id, {
        balanceRequestId: id,
        isApproved,
      }),
    onSuccess: () => this.#queryClient.invalidateQueries({ queryKey: ['balanceRequests'] }),
  });

  constructor() {
    effect(async () => {
      await this.#queryClient.prefetchQuery({ queryKey: ['balanceRequests'] });
    });
  }

  private readonly balanceRequest = inject(BalanceRequestService);

  handle(id: number, approved: boolean): Promise<void> {
    return this.handleBalanceRequest.mutateAsync({ id, isApproved: approved });
  }

  async update(event: RefresherCustomEvent): Promise<void> {
    await this.#queryClient.invalidateQueries({ queryKey: ['balanceRequests'] });
    setTimeout(() => {
      event.detail.complete();
    }, 200);
  }
}

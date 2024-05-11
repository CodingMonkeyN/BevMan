import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { AccountPage } from '../account.page';
import { FormControl, NonNullableFormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import {
  IonBackButton,
  IonButton,
  IonButtons,
  IonContent,
  IonHeader,
  IonInput,
  IonItem,
  IonLabel,
  IonNav,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { TranslateModule } from '@ngx-translate/core';
import { BalanceRequestService } from '../../../../api';
import { injectMutation, injectQueryClient } from '@ngneat/query';
import { NotificationService } from '../../../services/notification.service';

interface BalanceForm {
  amount: FormControl<number>;
}

@Component({
  selector: 'app-add-balance',
  templateUrl: './add-balance.page.html',
  standalone: true,
  imports: [
    IonNav,
    IonHeader,
    IonToolbar,
    IonButtons,
    IonBackButton,
    IonTitle,
    IonContent,
    ReactiveFormsModule,
    IonItem,
    IonLabel,
    IonInput,
    IonButton,
    TranslateModule,
  ],
  changeDetection: ChangeDetectionStrategy.OnPush,
})
export class AddBalancePage {
  readonly root = AccountPage;
  form = inject(NonNullableFormBuilder).group<BalanceForm>({
    amount: new FormControl(
      { value: 0, disabled: false },
      {
        validators: [Validators.min(1), Validators.required],
        nonNullable: true,
      },
    ),
  });

  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly balanceRequest = inject(BalanceRequestService);
  private readonly mutation = injectMutation();
  private readonly queryClient = injectQueryClient();
  private readonly notification = inject(NotificationService);

  private readonly addBalance = this.mutation({
    mutationFn: () => this.balanceRequest.createBalanceRequest(this.form.getRawValue()),
    onSuccess: async () => {
      await this.queryClient.invalidateQueries({ queryKey: ['balance'] });
      await this.router.navigate(['..'], { relativeTo: this.route });
    },
    onError: () => this.notification.showError('COMPONENTS.ADD_BALANCE.ERROR'),
  });

  async save(): Promise<void> {
    if (this.form.valid) {
      await this.addBalance.mutateAsync({});
    }
  }
}

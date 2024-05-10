import {ChangeDetectionStrategy, Component, DestroyRef, inject} from '@angular/core';
import {BalanceRequestService} from "../../../api";
import {AccountPage} from "../account.page";
import {FormControl, NonNullableFormBuilder, ReactiveFormsModule, Validators} from "@angular/forms";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";
import {catchError, of} from "rxjs";
import {ActivatedRoute, Router} from "@angular/router";
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
  IonToolbar
} from "@ionic/angular/standalone";
import {TranslateModule} from "@ngx-translate/core";

interface BalanceForm {
  amount: FormControl<number>
}

@Component({
  selector: 'app-add-balance',
  templateUrl: './add-balance.component.html',
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
    TranslateModule
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddBalanceComponent {
  readonly root = AccountPage;
  form = inject(NonNullableFormBuilder).group<BalanceForm>({
    amount: new FormControl({value: 0, disabled: false}, {
      validators: [Validators.min(1), Validators.required],
      nonNullable: true
    })
  })

  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly balanceRequest = inject(BalanceRequestService);
  private readonly destroyRef = inject(DestroyRef);

  async save(): Promise<void> {
    if (this.form.valid) {
      this.balanceRequest.createBalanceRequest(this.form.getRawValue()).pipe(
        takeUntilDestroyed(this.destroyRef),
        catchError(error => {
          console.error(error)
          return of(null)
        })).subscribe(value => {
        if (value) {
          this.router.navigate(['..'], {relativeTo: this.route});
        }
      });
    }
  }
}

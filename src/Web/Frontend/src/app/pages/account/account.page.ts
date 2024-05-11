import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
  IonActionSheet,
  IonButton,
  IonContent,
  IonFab,
  IonFabButton,
  IonHeader,
  IonIcon,
  IonInput,
  IonItem,
  IonLabel,
  IonList,
  IonRouterLink,
  IonSkeletonText,
  IonText,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { addIcons } from 'ionicons';
import { add } from 'ionicons/icons';
import { BalanceService, UserService } from '../../../api';
import { SupabaseService } from '../../services/supabase.service';
import { injectMutation, injectQuery, injectQueryClient } from '@ngneat/query';
import { ActionSheetButton } from '@ionic/angular';

@Component({
  selector: 'app-account',
  templateUrl: './account.page.html',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    IonButton,
    IonLabel,
    IonInput,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonContent,
    IonList,
    IonItem,
    FormsModule,
    TranslateModule,
    IonText,
    IonFabButton,
    IonIcon,
    IonFab,
    IonRouterLink,
    RouterLink,
    IonSkeletonText,
    IonActionSheet,
  ],
})
export class AccountPage {
  #query = injectQuery();
  #queryClient = injectQueryClient();
  #mutation = injectMutation();

  protected readonly balance = this.#query({
    queryFn: () => this.balanceService.getBalance(),
    queryKey: ['balance'],
  }).result;

  protected readonly deleteUser = this.#mutation({
    mutationFn: async () => {
      const user = await this.supabase.user;
      if (!user) {
        return;
      }

      return this.user.deleteUser(user.id);
    },
    onSuccess: () => this.signOut(),
  });
  protected readonly translate = inject(TranslateService);
  protected deleteAccountButtons: ActionSheetButton[] = [
    {
      text: this.translate.instant('ACCOUNT.DELETE'),
      role: 'destructive',
      handler: () => this.deleteUser.mutate({}) as unknown as any,
    },
    {
      text: this.translate.instant('GLOBAL.CANCEL'),
      role: 'cancel',
    },
  ];

  private readonly balanceService = inject(BalanceService);
  private readonly supabase = inject(SupabaseService);
  private readonly user = inject(UserService);
  private readonly router = inject(Router);

  constructor() {
    addIcons({ add });
  }

  async signOut(): Promise<void> {
    await this.supabase.signOut();
    this.#queryClient.removeQueries();
    await this.router.navigate(['/login'], { replaceUrl: true });
  }
}

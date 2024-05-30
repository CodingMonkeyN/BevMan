import { ChangeDetectionStrategy, Component, ElementRef, inject, ViewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
  ActionSheetButton,
  IonActionSheet,
  IonAvatar,
  IonButton,
  IonButtons,
  IonContent,
  IonFab,
  IonFabButton,
  IonFooter,
  IonHeader,
  IonIcon,
  IonInput,
  IonItem,
  IonLabel,
  IonList,
  IonRefresher,
  IonRefresherContent,
  IonRouterLink,
  IonSelect,
  IonSelectOption,
  IonSkeletonText,
  IonText,
  IonTitle,
  IonToolbar,
  LoadingController,
  RefresherCustomEvent,
} from '@ionic/angular/standalone';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { addIcons } from 'ionicons';
import { add } from 'ionicons/icons';
import { BalanceService, UserProfileService, UserService } from '../../../api';
import { SupabaseService } from '../../services/supabase.service';
import { injectMutation, injectQuery, injectQueryClient } from '@ngneat/query';

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
    IonAvatar,
    IonFooter,
    IonButtons,
    IonRefresher,
    IonRefresherContent,
    IonSelect,
    IonSelectOption,
  ],
})
export class AccountPage {
  @ViewChild('fileInput') fileInput!: ElementRef<HTMLInputElement>;

  #query = injectQuery();
  #queryClient = injectQueryClient();
  #mutation = injectMutation();

  protected readonly balance = this.#query({
    queryFn: () => this.balanceService.getBalance(),
    queryKey: ['balance'],
  }).result;
  protected readonly user = this.#query({
    queryFn: () => this.userProfile.getUserProfile(),
    queryKey: ['userProfile'],
  }).result;

  protected readonly deleteUser = this.#mutation({
    mutationFn: async () => {
      const user = await this.supabase.user;
      if (!user) {
        return;
      }

      return this.userService.deleteUser(user.id);
    },
    onSuccess: () => this.logout(),
  });
  protected readonly addProfilePicture = this.#mutation({
    mutationFn: (blob: Blob) => {
      return this.userProfile.addUserProfileImage(blob);
    },
    onMutate: () => this.loadingController.create().then(loading => loading.present()),
    onSuccess: async () => {
      await this.loadingController.dismiss();
      return this.#queryClient.invalidateQueries({ queryKey: ['userProfile'] });
    },
    onError: () => this.loadingController.dismiss(),
  });

  protected readonly translate = inject(TranslateService);
  protected accountButtons: ActionSheetButton[] = [
    {
      text: this.translate.instant('ACCOUNT.CHANGE_PROFILE_PICTURE'),
      role: 'selected',
      handler: () => this.fileInput.nativeElement.click(),
    },
    {
      text: this.translate.instant('ACCOUNT.LOGOUT'),
      role: 'destructive',
      handler: () => this.logout(),
    },
    {
      text: this.translate.instant('GLOBAL.CANCEL'),
      role: 'cancel',
    },
  ];
  protected readonly lanuageOptions = [
    { value: 'de', text: this.translate.instant('ACCOUNT.LANGUAGE.GERMAN') },
    { value: 'en', text: this.translate.instant('ACCOUNT.LANGUAGE.ENGLISH') },
  ];
  protected language = localStorage.getItem('language') ?? 'de';

  private readonly balanceService = inject(BalanceService);
  private readonly supabase = inject(SupabaseService);
  private readonly userService = inject(UserService);
  private readonly userProfile = inject(UserProfileService);
  private readonly loadingController = inject(LoadingController);
  private readonly router = inject(Router);

  constructor() {
    addIcons({ add });
  }

  protected async pickFile(inputElement: HTMLInputElement): Promise<void> {
    if (inputElement.files?.length !== 1) {
      return;
    }
    await this.addProfilePicture.mutateAsync(inputElement.files[0]);
  }

  private async logout(): Promise<void> {
    await this.supabase.signOut();
    this.#queryClient.removeQueries();
    await this.router.navigate(['/login'], { replaceUrl: true });
  }

  protected changeLanguage(language: string): void {
    localStorage.setItem('language', language);
    this.translate.use(language);
  }

  async update(event: RefresherCustomEvent): Promise<void> {
    await this.#queryClient.invalidateQueries({ queryKey: ['userProfile'] });
    await this.#queryClient.invalidateQueries({ queryKey: ['balance'] });
    setTimeout(() => {
      event.detail.complete();
    }, 200);
  }
}

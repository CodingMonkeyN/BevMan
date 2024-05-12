import { ChangeDetectionStrategy, Component, inject, ViewChild } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import {
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
  IonRouterLink,
  IonSkeletonText,
  IonText,
  IonTitle,
  IonToolbar,
  Platform,
} from '@ionic/angular/standalone';
import { FormsModule } from '@angular/forms';
import { TranslateModule, TranslateService } from '@ngx-translate/core';
import { addIcons } from 'ionicons';
import { add } from 'ionicons/icons';
import { BalanceService, UserProfileService, UserService } from '../../../api';
import { SupabaseService } from '../../services/supabase.service';
import { injectMutation, injectQuery, injectQueryClient } from '@ngneat/query';
import { ActionSheetButton } from '@ionic/angular';
import { Camera } from '@capacitor/camera';
import { Filesystem } from '@capacitor/filesystem';

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
  ],
})
export class AccountPage {
  @ViewChild('fileInput') fileInput!: HTMLInputElement;

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
    onSuccess: () => this.logout(),
  });

  protected readonly translate = inject(TranslateService);
  protected accountButtons: ActionSheetButton[] = [
    {
      text: this.translate.instant('ACCOUNT.CHANGE_PROFILE_PICTURE'),
      role: 'selected',
      handler: async () => {
        if (this.platform.is('mobile')) {
          await this.pickPhoto();
          return;
        }
        this.fileInput.click();
      },
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

  private readonly balanceService = inject(BalanceService);
  private readonly supabase = inject(SupabaseService);
  private readonly userService = inject(UserService);
  private readonly userProfile = inject(UserProfileService);
  private readonly platform = inject(Platform);
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

  private async pickPhoto(): Promise<void> {
    const image = await Camera.pickImages({ limit: 1 });
    if (image.photos.length === 0) {
      return;
    }

    const { data } = await Filesystem.readFile({ path: image.photos[0].path! });
    const blob = data instanceof Blob ? data : this.base64toBlob(data, image.photos[0].format);
    await this.addProfilePicture.mutateAsync(blob);
  }

  private base64toBlob(base64: string, type: string): Blob {
    const binaryString = atob(base64);
    const bytes = new Uint8Array(binaryString.length);
    for (let i = 0; i < binaryString.length; i++) {
      bytes[i] = binaryString.charCodeAt(i);
    }
    return new Blob([bytes], { type });
  }
}

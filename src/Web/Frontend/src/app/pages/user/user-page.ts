import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {
  IonAvatar,
  IonButton,
  IonContent,
  IonHeader,
  IonItem,
  IonItemOption,
  IonItemOptions,
  IonItemSliding,
  IonLabel,
  IonList,
  IonNote,
  IonRouterLink,
  IonSkeletonText,
  IonThumbnail,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { TranslateModule } from '@ngx-translate/core';
import { RouterLink } from '@angular/router';
import { UserService } from '../../../api';
import { injectQuery } from '@ngneat/query';

@Component({
  selector: 'app-user',
  templateUrl: 'user-page.html',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    IonThumbnail,
    IonAvatar,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonContent,
    IonButton,
    IonLabel,
    IonList,
    IonItem,
    IonItemSliding,
    IonItemOptions,
    IonItemOption,
    TranslateModule,
    IonRouterLink,
    RouterLink,
    IonSkeletonText,
    IonNote,
  ],
})
export class UserPage {
  #query = injectQuery();
  protected readonly users = this.#query({ queryFn: () => this.user.getUsers(), queryKey: ['users'] }).result;

  private readonly user = inject(UserService);
  protected readonly Array = Array;
}

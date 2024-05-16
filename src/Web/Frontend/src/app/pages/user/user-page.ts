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
  IonRefresher,
  IonRefresherContent,
  IonRouterLink,
  IonSkeletonText,
  IonThumbnail,
  IonTitle,
  IonToolbar,
  RefresherCustomEvent,
} from '@ionic/angular/standalone';
import { TranslateModule } from '@ngx-translate/core';
import { RouterLink } from '@angular/router';
import { UserService } from '../../../api';
import { injectQuery, injectQueryClient } from '@ngneat/query';

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
    IonRefresher,
    IonRefresherContent,
  ],
})
export class UserPage {
  #query = injectQuery();
  #queryClient = injectQueryClient();
  protected readonly users = this.#query({ queryFn: () => this.user.getUsers(), queryKey: ['users'] }).result;

  private readonly user = inject(UserService);
  protected readonly Array = Array;

  protected async update(event: RefresherCustomEvent): Promise<void> {
    await this.#queryClient.invalidateQueries({ queryKey: ['users'] });
    setTimeout(() => {
      event.detail.complete();
    }, 200);
  }
}

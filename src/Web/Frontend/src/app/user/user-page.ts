import {Component, inject} from '@angular/core';
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
  IonThumbnail,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import {UserService} from "../../api";
import {toSignal} from "@angular/core/rxjs-interop";
import {TranslateModule} from "@ngx-translate/core";
import {RouterLink} from "@angular/router";

@Component({
  selector: 'app-user',
  templateUrl: 'user-page.html',
  standalone: true,
  imports: [IonThumbnail, IonAvatar, IonHeader, IonToolbar, IonTitle, IonContent, IonButton, IonLabel, IonList, IonItem, IonItemSliding, IonItemOptions, IonItemOption, TranslateModule, RouterLink],
})
export class UserPage {
  protected readonly users = toSignal(inject(UserService).getUsers());
}

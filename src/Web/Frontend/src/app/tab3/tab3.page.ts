import {Component, Signal} from '@angular/core';
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
import {UserDto, UserService} from "../../api";
import {UserRoleService} from "../services/user-role.service";
import {toSignal} from "@angular/core/rxjs-interop";
import {TranslateModule} from "@ngx-translate/core";
import {Router} from "@angular/router";

@Component({
  selector: 'app-tab3',
  templateUrl: 'tab3.page.html',
  styleUrls: ['tab3.page.scss'],
  standalone: true,
  imports: [IonThumbnail, IonAvatar, IonHeader, IonToolbar, IonTitle, IonContent, IonButton, IonLabel, IonList, IonItem, IonItemSliding, IonItemOptions, IonItemOption, TranslateModule],
})
export class Tab3Page {
  protected readonly users: Signal<UserDto[] | undefined>

  constructor(private readonly router: Router, private readonly user: UserService, private readonly userRole: UserRoleService) {
    this.users = toSignal(this.user.getUsers())
  }

  protected ChangeUserRole(user: UserDto): void {
    console.log(`Changing ${user.email} with role ${user.roles}`);
  }

  protected DeleteUser(user: UserDto) {
    console.log(`Deleting ${user.email} with role ${user.roles}`);
  }
}

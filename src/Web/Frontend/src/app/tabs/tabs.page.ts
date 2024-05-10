import {Component, inject} from '@angular/core';
import {IonIcon, IonLabel, IonTabBar, IonTabButton, IonTabs} from '@ionic/angular/standalone';
import {addIcons} from 'ionicons';
import {add, beer, people, person} from 'ionicons/icons';
import {TranslateModule} from "@ngx-translate/core";
import {UserRoleService} from "../services/user-role.service";
import {UserRole} from "../enums/user-role.enum";

@Component({
  selector: 'app-tabs',
  templateUrl: 'tabs.page.html',
  standalone: true,
  imports: [IonTabs, IonTabBar, IonTabButton, IonIcon, IonLabel, TranslateModule],
})
export class TabsPage {
  protected readonly userRole = inject(UserRoleService);
  protected readonly UserRole = UserRole;

  constructor() {
    addIcons({person, people, beer, add});
  }
}

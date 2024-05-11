import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import { IonIcon, IonLabel, IonTabBar, IonTabButton, IonTabs } from '@ionic/angular/standalone';
import { addIcons } from 'ionicons';
import { beer, card, people, person } from 'ionicons/icons';
import { TranslateModule } from '@ngx-translate/core';
import { UserRole } from '../../enums/user-role.enum';
import { UserContext } from '../../services/user-context.service';

@Component({
  selector: 'app-tabs',
  templateUrl: 'tabs.page.html',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [IonTabs, IonTabBar, IonTabButton, IonIcon, IonLabel, TranslateModule],
})
export class TabsPage {
  protected readonly userContext = inject(UserContext);
  protected readonly UserRole = UserRole;

  constructor() {
    addIcons({ person, people, beer, card });
  }
}

import {Component, Signal} from '@angular/core'
import {Router, RouterLink} from '@angular/router'
import {SupabaseService} from '../services/supabase.service'
import {
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
  IonText,
  IonTitle,
  IonToolbar
} from "@ionic/angular/standalone";
import {FormsModule} from "@angular/forms";
import {TranslateModule} from "@ngx-translate/core";
import {BalanceDto, BalanceService} from "../../api";
import {toSignal} from "@angular/core/rxjs-interop";
import {addIcons} from "ionicons";
import {add} from "ionicons/icons";

@Component({
  selector: 'app-account',
  templateUrl: './account.page.html',
  standalone: true,
  imports: [IonButton, IonLabel, IonInput, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, FormsModule, TranslateModule, IonText, IonFabButton, IonIcon, IonFab, IonRouterLink, RouterLink],
})
export class AccountPage {
  balance: Signal<BalanceDto | undefined>;

  constructor(
    private readonly balanceService: BalanceService,
    private readonly supabase: SupabaseService,
    private router: Router
  ) {
    this.balance = toSignal(this.balanceService.getBalance());
    addIcons({add})
  }

  async signOut() {
    await this.supabase.signOut()
    return this.router.navigate(['/login'], {replaceUrl: true})
  }
}

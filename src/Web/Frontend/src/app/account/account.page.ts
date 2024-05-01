import {Component, OnInit} from '@angular/core'
import {Router} from '@angular/router'
import {SupabaseService} from '../supabase.service'
import {
  IonButton,
  IonContent,
  IonHeader,
  IonInput,
  IonItem,
  IonLabel,
  IonList,
  IonTitle,
  IonToolbar
} from "@ionic/angular/standalone";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-account',
  templateUrl: './account.page.html',
  styleUrls: ['./account.page.scss'],
  standalone: true,
  imports: [IonButton, IonLabel, IonInput, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, FormsModule],
})
export class AccountPage implements OnInit {
  email = ''

  constructor(
    private readonly supabase: SupabaseService,
    private router: Router
  ) {
  }

  ngOnInit() {
    this.getEmail()
  }

  async getEmail() {
    this.email = await this.supabase.user.then((user) => user?.email || '')
  }


  async signOut() {
    await this.supabase.signOut()
    return this.router.navigate(['/login'], {replaceUrl: true})
  }
}

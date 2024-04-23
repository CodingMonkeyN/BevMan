import { Component, OnInit } from '@angular/core'
import { Router } from '@angular/router'
import { Profile, SupabaseService } from '../supabase.service'
import {
  IonButton,
  IonContent,
  IonHeader,
  IonInput, IonItem,
  IonLabel,
  IonList,
  IonTitle,
  IonToolbar
} from "@ionic/angular/standalone";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-account',
  template: `
    <ion-header>
      <ion-toolbar>
        <ion-title>Account</ion-title>
      </ion-toolbar>
    </ion-header>

    <ion-content>
      <form>
        <ion-item>
          <ion-label position="stacked">Email</ion-label>
          <ion-input type="email" name="email" [(ngModel)]="email" readonly></ion-input>
        </ion-item>
      </form>

      <div class="ion-text-center">
        <ion-button fill="clear" (click)="signOut()">Log Out</ion-button>
      </div>
    </ion-content>
  `,
  styleUrls: ['./account.page.scss'],
  standalone: true,
  imports: [IonButton, IonLabel, IonInput, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, FormsModule],
})
export class AccountPage implements OnInit {
  email = ''

  constructor(
    private readonly supabase: SupabaseService,
    private router: Router
  ) {}
  ngOnInit() {
    this.getEmail()
  }

  async getEmail() {
    this.email = await this.supabase.user.then((user) => user?.email || '')
  }


  async signOut()  {
    await this.supabase.signOut()
    return this.router.navigate(['/login'], { replaceUrl: true })
  }
}

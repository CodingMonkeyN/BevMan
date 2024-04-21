import { Component, OnInit } from '@angular/core'
import { SupabaseService } from '../supabase.service'
import {
  IonButton,
  IonLabel,
  IonInput,
  IonToolbar,
  IonHeader,
  IonTitle,
  IonContent,
  IonList, IonItem
} from "@ionic/angular/standalone";
import {FormsModule} from "@angular/forms";

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
  standalone: true,
  imports: [IonButton, IonLabel, IonInput, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, FormsModule],
})
export class LoginPage {
  email = ''
  password = ''

  constructor(private readonly supabase: SupabaseService) {}

  async handleLogin(event: any) {
    event.preventDefault()
    const loader = await this.supabase.createLoader()
    await loader.present()
    try {
      const { error } = await this.supabase.signInEmailPassword(this.email, this.password)
      if (error) {
        throw error
      }
      await loader.dismiss()
      await this.supabase.createNotice('Check your email for the login link!')
    } catch (error: any) {
      await loader.dismiss()
      await this.supabase.createNotice(error.error_description || error.message)
    }
  }
}

import {Component} from '@angular/core'
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
import {Router} from "@angular/router";

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

  constructor(private readonly router: Router,
              private readonly supabase: SupabaseService) {
  }

  async handleLogin(event: any) {
    event.preventDefault()
    const loader = await this.supabase.createLoader()
    await loader.present()
    try {
      const {error} = await this.supabase.signInEmailPassword(this.email, this.password)
      if (error) {
        throw error
      }
      await loader.dismiss()
      await this.router.navigate(['tabs', 'account'])
    } catch (error: any) {
      await loader.dismiss()
      await this.supabase.createNotice(error.error_description || error.message)
    }
  }
}

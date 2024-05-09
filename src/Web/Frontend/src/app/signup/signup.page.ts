import {Component} from '@angular/core'
import {SupabaseService} from '../services/supabase.service'
import {
  IonBackButton,
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
import {NgIf} from "@angular/common";

@Component({
  selector: 'app-signup',
  templateUrl: './signup.page.html',
  standalone: true,
  imports: [IonButton, IonLabel, IonInput, IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, FormsModule, IonBackButton, NgIf],
})

export class SignupPage {
  email: string = ''
  password: string = ''
  confirmPassword: string = ''

  constructor(private readonly router: Router,
              private readonly supabase: SupabaseService) {
  }

  async handleSignup(event: any) {
    event.preventDefault()
    const loader = await this.supabase.createLoader()
    await loader.present()
    try {
      const {error} = await this.supabase.signUp(this.email, this.password)
      this.email = ''
      this.password = ''
      this.confirmPassword = ''
      if (error) {
        throw error
      }
      await loader.dismiss()
      await this.router.navigate(['login'])
    } catch (error: any) {
      await loader.dismiss()
      await this.supabase.createNotice(error.error_description || error.message)
    }
  }
}

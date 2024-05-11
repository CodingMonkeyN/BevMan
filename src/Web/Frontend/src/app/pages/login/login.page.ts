import { ChangeDetectionStrategy, Component } from '@angular/core';
import {
  IonButton,
  IonContent,
  IonHeader,
  IonInput,
  IonItem,
  IonLabel,
  IonList,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { TranslateModule } from '@ngx-translate/core';
import { SupabaseService } from '../../services/supabase.service';
import { NotificationService } from '../../services/notification.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    IonButton,
    IonLabel,
    IonInput,
    IonHeader,
    IonToolbar,
    IonTitle,
    IonContent,
    IonList,
    IonItem,
    FormsModule,
    TranslateModule,
  ],
})
export class LoginPage {
  email = '';
  password = '';

  constructor(
    private readonly router: Router,
    private readonly supabase: SupabaseService,
    private readonly notification: NotificationService,
  ) {}

  async redirectToSignUp(): Promise<void> {
    await this.router.navigate(['signup']);
  }

  async handleLogin(): Promise<void> {
    try {
      const { error } = await this.supabase.signInEmailPassword(this.email, this.password);
      if (error) {
        throw error;
      }
      await this.router.navigate(['tabs']);
    } catch (error: any) {
      await this.notification.showError('LOGIN.ERRORS.INVALID_CREDENTIALS');
    }
  }
}

import { inject, Injectable } from '@angular/core';
import { ToastController } from '@ionic/angular';
import { TranslateService } from '@ngx-translate/core';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly toastController = inject(ToastController);
  private readonly translate = inject(TranslateService);

  async showQueryError(): Promise<void> {
    const toast = await this.toastController.create({
      message: this.translate.instant('GLOBAL.ERROR.QUERY'),
      duration: 3000,
      color: 'danger',
    });
    await toast.present();
  }

  async showError(translateKey: string): Promise<void> {
    const toast = await this.toastController.create({
      message: this.translate.instant(translateKey),
      duration: 3000,
      color: 'danger',
    });
    await toast.present();
  }

  async showSuccess(translateKey: string): Promise<void> {
    const toast = await this.toastController.create({
      message: this.translate.instant(translateKey),
      duration: 1500,
      color: 'success',
    });
    await toast.present();
  }
}

import { Injectable } from '@angular/core';
import { LoadingController, ToastController } from '@ionic/angular/standalone';
import { AuthChangeEvent, createClient, Session, SupabaseClient } from '@supabase/supabase-js';
import { environment } from '../../environments/environment';

export interface Profile {
  username: string;
  website: string;
  avatar_url: string;
}

@Injectable({
  providedIn: 'root',
})
export class SupabaseService {
  private supabase: SupabaseClient;

  constructor(
    private loadingCtrl: LoadingController,
    private toastCtrl: ToastController,
  ) {
    this.supabase = createClient(environment.supabaseUrl, environment.supabaseKey);
  }

  get user() {
    return this.supabase.auth.getUser().then(({ data }) => data?.user);
  }

  get session() {
    return this.supabase.auth.getSession().then(({ data }) => data?.session);
  }

  authChanges(callback: (event: AuthChangeEvent, session: Session | null) => void) {
    return this.supabase.auth.onAuthStateChange(callback);
  }

  signInEMail(email: string) {
    return this.supabase.auth.signInWithOtp({ email });
  }

  signInEmailPassword(email: string, password: string) {
    return this.supabase.auth.signInWithPassword({ email, password });
  }

  signOut() {
    return this.supabase.auth.signOut();
  }

  async createNotice(message: string) {
    const toast = await this.toastCtrl.create({ message, duration: 5000 });
    await toast.present();
  }

  createLoader() {
    return this.loadingCtrl.create();
  }

  signUp(email: string, password: string) {
    return this.supabase.auth.signUp({ email, password });
  }
}

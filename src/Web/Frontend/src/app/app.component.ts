import {Component} from '@angular/core';
import {IonApp, IonRouterOutlet} from '@ionic/angular/standalone';
import { Router } from '@angular/router'
import { SupabaseService } from './supabase.service'

@Component({
  selector: 'app-root',
  templateUrl: 'app.component.html',
  standalone: true,
  imports: [IonApp, IonRouterOutlet],
})
export class AppComponent {
  constructor(
    private supabase: SupabaseService,
    private router: Router
  ) {
    this.supabase.authChanges((_, session) => {
      console.log(session)

    })
  }
}

import {Component} from '@angular/core';
import {
  IonButton,
  IonContent,
  IonHeader,
  IonInput,
  IonItem,
  IonLabel,
  IonTitle,
  IonToolbar
} from '@ionic/angular/standalone';
import {FormsModule, ReactiveFormsModule} from "@angular/forms";

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss'],
  standalone: true,
  imports: [IonHeader, IonToolbar, IonTitle, IonContent, FormsModule, IonButton, IonInput, IonItem, IonLabel, ReactiveFormsModule],
})
export class Tab1Page {
  constructor() {
  }
}

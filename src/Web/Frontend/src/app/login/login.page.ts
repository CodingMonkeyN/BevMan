import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import {
  IonCard,
  IonContent,
  IonHeader,
  IonRow,
  IonTitle,
  IonToolbar,
  IonCol,
  IonButton, IonCardHeader, IonCardContent, IonCardTitle
} from '@ionic/angular/standalone';

@Component({
  selector: 'app-login',
  templateUrl: './login.page.html',
  styleUrls: ['./login.page.scss'],
  standalone: true,
  imports: [IonContent, IonHeader, IonTitle, IonToolbar, CommonModule, FormsModule, IonCard, IonRow, IonCol, IonButton, IonCardHeader, IonCardContent, IonCardTitle]
})
export class LoginPage implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}

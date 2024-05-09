import {Component} from '@angular/core';
import {
  IonBackButton,
  IonButton,
  IonButtons,
  IonContent,
  IonHeader,
  IonItem,
  IonItemOption,
  IonItemOptions,
  IonItemSliding,
  IonLabel,
  IonList,
  IonNav,
  IonThumbnail,
  IonTitle,
  IonToolbar
} from "@ionic/angular/standalone";
import {TranslateModule} from "@ngx-translate/core";
import {ProductPage} from "../product/product-page";

@Component({
  selector: 'app-product-editor',
  templateUrl: './product-editor.component.html',
  standalone: true,
  imports: [
    IonContent,
    IonHeader,
    IonItem,
    IonItemOption,
    IonItemOptions,
    IonItemSliding,
    IonLabel,
    IonList,
    IonThumbnail,
    IonTitle,
    IonToolbar,
    TranslateModule,
    IonNav,
    IonButton,
    IonButtons,
    IonBackButton
  ]
})
export class ProductEditorComponent {
  protected readonly root = ProductPage

  constructor() {
  }
}

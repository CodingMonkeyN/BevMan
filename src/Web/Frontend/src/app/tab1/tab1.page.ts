import {Component, Signal} from '@angular/core';
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
import {TranslateModule} from "@ngx-translate/core";
import {ProductDto, ProductService} from "../../api";
import {UserRoleService} from "../services/user-role.service";
import {toSignal} from "@angular/core/rxjs-interop";

@Component({
  selector: 'app-tab1',
  templateUrl: 'tab1.page.html',
  styleUrls: ['tab1.page.scss'],
  standalone: true,
  imports: [IonHeader, IonToolbar, IonTitle, IonContent, FormsModule, IonButton, IonInput, IonItem, IonLabel, ReactiveFormsModule, TranslateModule],
})
export class Tab1Page {
  protected readonly products: Signal<ProductDto[] | undefined>

  constructor(private readonly product: ProductService, private readonly userRole: UserRoleService) {
    this.products = toSignal(this.product.getProducts())
  }
}

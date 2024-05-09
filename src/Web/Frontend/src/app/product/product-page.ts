import {Component, Signal} from '@angular/core';
import {IonContent, IonHeader, IonItem, IonLabel, IonList, IonTitle, IonToolbar} from '@ionic/angular/standalone';
import {ProductDto, ProductService} from "../../api";
import {toSignal} from "@angular/core/rxjs-interop";
import {TranslateModule} from "@ngx-translate/core";
import {UserRoleService} from "../services/user-role.service";

@Component({
  selector: 'app-product-page',
  templateUrl: 'product-page.html',
  standalone: true,
  imports: [IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, IonLabel, TranslateModule]
})
export class ProductPage {
  protected readonly products: Signal<ProductDto[] | undefined>

  constructor(private readonly product: ProductService, private readonly userRole: UserRoleService) {
    this.products = toSignal(this.product.getProducts())
  }
}

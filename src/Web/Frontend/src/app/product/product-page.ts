import {Component, Signal} from '@angular/core';
import {
  IonAvatar,
  IonCard,
  IonCardContent,
  IonCardHeader,
  IonCardSubtitle,
  IonCardTitle,
  IonContent,
  IonHeader,
  IonItem,
  IonItemOption,
  IonItemOptions,
  IonItemSliding,
  IonLabel,
  IonList,
  IonPopover,
  IonRouterLink,
  IonThumbnail,
  IonTitle,
  IonToolbar
} from '@ionic/angular/standalone';
import {ProductDto, ProductService} from "../../api";
import {toSignal} from "@angular/core/rxjs-interop";
import {TranslateModule} from "@ngx-translate/core";
import {UserRoleService} from "../services/user-role.service";
import {Router, RouterLink} from "@angular/router";

@Component({
  selector: 'app-product-page',
  templateUrl: 'product-page.html',
  standalone: true,
  imports: [IonHeader, IonToolbar, IonTitle, IonContent, IonList, IonItem, IonLabel, TranslateModule, IonCard, IonCardHeader, IonCardTitle, IonCardSubtitle, IonCardContent, IonThumbnail, IonPopover, IonItemSliding, IonItemOption, IonItemOptions, IonAvatar, IonRouterLink, RouterLink]
})
export class ProductPage {
  protected readonly products: Signal<ProductDto[] | undefined>

  constructor(private readonly product: ProductService, private readonly userRole: UserRoleService, private readonly router: Router) {
    this.products = toSignal(this.product.getProducts())
  }

  protected slideActionBuy(product: ProductDto): void {
    console.log(`Buying ${product.name}`);
  }

  protected openEditor(product: ProductDto) {
    this.router.navigate([['tabs', 'products', 'product-editor']])
  }
}

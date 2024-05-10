import {Component, inject, Signal} from '@angular/core';
import {
  IonAvatar,
  IonCard,
  IonCardContent,
  IonCardHeader,
  IonCardSubtitle,
  IonCardTitle,
  IonContent,
  IonFab,
  IonFabButton,
  IonHeader,
  IonIcon,
  IonItem,
  IonItemOption,
  IonItemOptions,
  IonItemSliding,
  IonLabel,
  IonList,
  IonNote,
  IonPopover,
  IonRouterLink,
  IonText,
  IonThumbnail,
  IonTitle,
  IonToolbar
} from '@ionic/angular/standalone';
import {ProductDto, ProductService} from "../../api";
import {toSignal} from "@angular/core/rxjs-interop";
import {TranslateModule} from "@ngx-translate/core";
import {UserRoleService} from "../services/user-role.service";
import {Router, RouterLink} from "@angular/router";
import {UserRole} from "../enums/user-role.enum";

@Component({
  selector: 'app-product-page',
  templateUrl: 'product-page.html',
  standalone: true,
  imports: [IonHeader, IonToolbar, IonTitle,
    IonContent, IonList, IonItem, IonLabel,
    TranslateModule, IonCard, IonCardHeader,
    IonCardTitle, IonCardSubtitle, IonCardContent,
    IonThumbnail, IonPopover, IonItemSliding,
    IonItemOption, IonItemOptions, IonAvatar,
    IonRouterLink, RouterLink, IonText, IonNote, IonIcon, IonFab, IonFabButton]
})
export class ProductPage {
  protected readonly products: Signal<ProductDto[] | undefined>
  protected readonly userRole = inject(UserRoleService);
  protected readonly UserRole = UserRole;
  protected routerLinkParameter: string | undefined = undefined;


  constructor(private readonly product: ProductService, private readonly router: Router) {
    this.products = toSignal(this.product.getProducts())
  }

  protected getRouterLink(id: number): any {
    if (this.userRole.hasRoles([UserRole.Admin])) {
      return id.toString()
    }
    return undefined
  }

  protected slideActionBuy(product: ProductDto): void {
    console.log(`Buying ${product.name}`);
  }

  protected addProduct() {
    this.router.navigate(['tabs', 'products', 'create']);
  }
}

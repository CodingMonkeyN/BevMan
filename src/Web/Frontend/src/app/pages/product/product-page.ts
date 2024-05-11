import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
import {
  IonAvatar,
  IonCard,
  IonCardContent,
  IonCardHeader,
  IonCardSubtitle,
  IonCardTitle,
  IonContent,
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
  IonSkeletonText,
  IonText,
  IonThumbnail,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { TranslateModule } from '@ngx-translate/core';
import { RouterLink } from '@angular/router';
import { ProductDto, ProductService } from '../../../api';
import { UserContext } from '../../services/user-context.service';
import { UserRole } from '../../enums/user-role.enum';
import { injectQuery } from '@ngneat/query';

@Component({
  selector: 'app-product-page',
  templateUrl: 'product-page.html',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    IonHeader,
    IonToolbar,
    IonTitle,
    IonContent,
    IonList,
    IonItem,
    IonLabel,
    TranslateModule,
    IonCard,
    IonCardHeader,
    IonCardTitle,
    IonCardSubtitle,
    IonCardContent,
    IonThumbnail,
    IonPopover,
    IonItemSliding,
    IonItemOption,
    IonItemOptions,
    IonAvatar,
    IonRouterLink,
    RouterLink,
    IonText,
    IonNote,
    IonIcon,
    IonSkeletonText,
  ],
})
export class ProductPage {
  #query = injectQuery();

  protected readonly products = this.#query({
    queryFn: () => this.product.getProducts(),
    queryKey: ['products'],
  }).result;

  protected readonly userContext = inject(UserContext);
  protected readonly product = inject(ProductService);

  protected getRouterLink(id: number): string | undefined {
    if (this.userContext.hasRoles([UserRole.Admin])) {
      return id.toString();
    }
    return undefined;
  }

  protected slideActionBuy(product: ProductDto): void {
    console.log(`Buying ${product.name}`);
  }

  protected readonly Array = Array;
}

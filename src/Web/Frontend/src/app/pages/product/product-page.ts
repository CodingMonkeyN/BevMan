import { ChangeDetectionStrategy, Component, inject } from '@angular/core';
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
  IonRefresher,
  IonRefresherContent,
  IonRouterLink,
  IonSkeletonText,
  IonSpinner,
  IonText,
  IonThumbnail,
  IonTitle,
  IonToolbar,
  LoadingController,
  RefresherCustomEvent,
} from '@ionic/angular/standalone';
import { TranslateModule } from '@ngx-translate/core';
import { RouterLink } from '@angular/router';
import { BalanceService, ProductDto, ProductService } from '../../../api';
import { UserContext } from '../../services/user-context.service';
import { UserRole } from '../../enums/user-role.enum';
import { injectMutation, injectQuery, injectQueryClient } from '@ngneat/query';
import { NotificationService } from '../../services/notification.service';
import { addIcons } from 'ionicons';
import { add } from 'ionicons/icons';

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
    IonSpinner,
    IonFabButton,
    IonFab,
    IonRefresher,
    IonRefresherContent,
  ],
})
export class ProductPage {
  #query = injectQuery();
  #queryClient = injectQueryClient();
  #mutation = injectMutation();

  protected readonly products = this.#query({
    queryFn: () => this.product.getProducts(),
    queryKey: ['products'],
  }).result;

  protected readonly balance = this.#query({
    queryFn: () => this.balanceService.getBalance(),
    queryKey: ['balance'],
  }).result;
  protected readonly buyProduct = this.#mutation({
    mutationFn: (product: ProductDto) => this.product.buyProduct(product.id, product.id),
    onMutate: () => this.loadingController.create().then(loading => loading.present()),
    onSuccess: async () => {
      await this.loadingController.dismiss();
      await this.notification.showSuccess('PRODUCTS.BUY_SUCCESS');
      await this.#queryClient.invalidateQueries({ queryKey: ['products'] });
      await this.#queryClient.invalidateQueries({ queryKey: ['balance'] });
    },
    onError: async () => {
      await this.loadingController.dismiss();
      return this.notification.showError('PRODUCTS.BUY_ERROR');
    },
  });

  protected readonly Array = Array;
  protected readonly userContext = inject(UserContext);
  private readonly balanceService = inject(BalanceService);
  private readonly product = inject(ProductService);
  private readonly loadingController = inject(LoadingController);
  private readonly notification = inject(NotificationService);

  constructor() {
    addIcons({ add });
  }

  protected getRouterLink(id: number): string | undefined {
    if (this.userContext.hasRoles([UserRole.Admin])) {
      return id.toString();
    }
    return undefined;
  }

  protected async onBuyProduct(product: ProductDto): Promise<void> {
    await this.buyProduct.mutateAsync(product);
  }

  protected async update(event: RefresherCustomEvent): Promise<void> {
    await this.#queryClient.invalidateQueries({ queryKey: ['products'] });
    await this.#queryClient.invalidateQueries({ queryKey: ['balance'] });
    setTimeout(() => {
      event.detail.complete();
    }, 200);
  }

  protected readonly UserRole = UserRole;
}

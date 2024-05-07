import {Component, Signal} from '@angular/core';
import {IonContent, IonHeader, IonTitle, IonToolbar} from '@ionic/angular/standalone';
import {ProductDto, ProductService} from "../../api";
import {toSignal} from "@angular/core/rxjs-interop";
import {ExploreContainerComponent} from "../explore-container/explore-container.component";

@Component({
  selector: 'app-tab2',
  templateUrl: 'tab2.page.html',
  styleUrls: ['tab2.page.scss'],
  standalone: true,
  imports: [IonHeader, IonToolbar, IonTitle, IonContent, ExploreContainerComponent]
})
export class Tab2Page {
  protected readonly products: Signal<ProductDto[] | undefined>

  constructor(private readonly product: ProductService) {
    this.products = toSignal(this.product.getProducts())
  }

}

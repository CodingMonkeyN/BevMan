import {Component, Signal} from '@angular/core';
import {IonContent, IonHeader, IonItem, IonLabel, IonList, IonTitle, IonToolbar} from '@ionic/angular/standalone';
import {ProductDto, ProductService} from "../../api";
import {toSignal} from "@angular/core/rxjs-interop";
import {ExploreContainerComponent} from "../explore-container/explore-container.component";
import {of} from "rxjs";

@Component({
  selector: 'app-tab2',
  templateUrl: 'tab2.page.html',
  styleUrls: ['tab2.page.scss'],
  standalone: true,
  imports: [IonHeader, IonToolbar, IonTitle, IonContent, ExploreContainerComponent, IonList, IonItem, IonLabel]
})
export class Tab2Page {
  protected readonly products: Signal<ProductDto[] | undefined>

  constructor(private readonly product: ProductService) {
    this.products = toSignal(of([{name: "Bier", price: 1.40, id: 0} as ProductDto, {
      name: "Cola",
      price: 2.10,
      id: 1
    } as ProductDto]));
    //this.products = toSignal(this.product.getProducts())
  }

}

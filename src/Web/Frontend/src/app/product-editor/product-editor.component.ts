import {Component, input} from '@angular/core';
import {
  IonBackButton,
  IonButton,
  IonButtons,
  IonContent,
  IonHeader,
  IonInput,
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
import {FormControl, FormGroup, FormsModule, NonNullableFormBuilder, Validators} from "@angular/forms";
import {NgIf} from "@angular/common";
import {ProductService} from "../../api";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";

interface ProductForm {
  name: FormControl<string>;
  price: FormControl<number>;
  quantity: FormControl<number>;
  description: FormControl<string | undefined>;
}

interface ProductFormValue {
  name: string;
  price: number;
  quantity: number;
  description?: string;
}

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
    IonBackButton,
    FormsModule,
    IonInput,
    NgIf
  ]
})
export class ProductEditorComponent {
  protected readonly root = ProductPage
  protected readonly id = input.required<string>
  protected readonly form: FormGroup<ProductForm>

  constructor(private readonly product: ProductService, formBuilder: NonNullableFormBuilder) {
    this.form = formBuilder.group<ProductForm>({
      name: new FormControl({value: "", disabled: false}, {nonNullable: true, validators: [Validators.required]}),
      price: new FormControl({value: 0, disabled: false}, {nonNullable: true, validators: [Validators.required]}),
      quantity: new FormControl({value: 0, disabled: false}, {nonNullable: true, validators: [Validators.required]}),
      description: new FormControl({value: "", disabled: false}, {
        nonNullable: true,
        validators: [Validators.required]
      }),
    })

    this.product.getProduct(Number(this.id())).pipe(
      takeUntilDestroyed(),
    ).subscribe(product => this.form.patchValue(product as ProductFormValue))
  }

  handleProductSaved($event: any) {

  }
}

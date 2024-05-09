import {Component, signal} from '@angular/core';
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
import {
  FormControl,
  FormGroup,
  FormsModule,
  NonNullableFormBuilder,
  ReactiveFormsModule,
  Validators
} from "@angular/forms";
import {NgIf} from "@angular/common";
import {ProductService} from "../../api";
import {takeUntilDestroyed} from "@angular/core/rxjs-interop";
import {ActivatedRoute, Router} from "@angular/router";
import {firstValueFrom, switchMap, tap} from "rxjs";

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
    NgIf,
    ReactiveFormsModule
  ]
})
export class ProductEditorComponent {
  protected readonly root = ProductPage
  protected readonly form: FormGroup<ProductForm>
  private id = signal<number | undefined>(undefined)

  constructor(private readonly router: Router,
              private readonly product: ProductService,
              formBuilder: NonNullableFormBuilder,
              route: ActivatedRoute) {
    this.form = formBuilder.group<ProductForm>({
      name: new FormControl({value: "", disabled: false}, {nonNullable: true, validators: [Validators.required]}),
      price: new FormControl({value: 0, disabled: false}, {nonNullable: true, validators: [Validators.required]}),
      quantity: new FormControl({value: 0, disabled: false}, {nonNullable: true, validators: [Validators.required]}),
      description: new FormControl({value: "", disabled: false}, {
        nonNullable: true,
        validators: [Validators.required]
      }),
    })

    route.params.pipe(
      takeUntilDestroyed(),
      tap(({id}) => this.id.set(Number(id))),
      switchMap(({id}) => this.product.getProduct(Number(id))),
    ).subscribe(product => this.form.patchValue(product as ProductFormValue))
  }

  async save(): Promise<void> {
    if (!this.id()) {
      return
    }
    try {
      await firstValueFrom(this.product.updateProduct(this.id()!, {...this.form.getRawValue(), id: this.id()!}))
      await this.router.navigate(['..']);
    } catch (error) {
      console.error(error);
    }
  }
}

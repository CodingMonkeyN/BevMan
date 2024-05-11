import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
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
  IonToolbar,
} from '@ionic/angular/standalone';
import { TranslateModule } from '@ngx-translate/core';
import { ProductPage } from '../product-page';
import { FormBuilder, FormControl, FormsModule, ReactiveFormsModule, Validators } from '@angular/forms';
import { NgIf } from '@angular/common';
import { takeUntilDestroyed } from '@angular/core/rxjs-interop';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap, tap } from 'rxjs';
import { ProductService } from '../../../../api';
import { injectMutation, injectQueryClient } from '@ngneat/query';
import { NotificationService } from '../../../services/notification.service';

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
  templateUrl: './product-details.page.html',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
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
    ReactiveFormsModule,
  ],
})
export class ProductDetailsPage {
  protected readonly root = ProductPage;
  protected readonly form = inject(FormBuilder).group<ProductForm>({
    name: new FormControl({ value: '', disabled: false }, { nonNullable: true, validators: [Validators.required] }),
    price: new FormControl({ value: 0, disabled: false }, { nonNullable: true, validators: [Validators.required] }),
    quantity: new FormControl({ value: 0, disabled: false }, { nonNullable: true, validators: [Validators.required] }),
    description: new FormControl(
      { value: '', disabled: false },
      {
        nonNullable: true,
        validators: [Validators.required],
      },
    ),
  });

  private id = signal<number | undefined>(undefined);

  private readonly mutation = injectMutation();
  private readonly queryClient = injectQueryClient();
  private readonly notification = inject(NotificationService);
  private readonly editProduct = this.mutation({
    mutationFn: (value: ProductFormValue) => this.product.updateProduct(this.id()!, { ...value, id: this.id()! }),
    onSuccess: async () => {
      await this.queryClient.invalidateQueries({ queryKey: ['products'] });
      await this.router.navigate(['..', { relativeTo: this.route }]);
    },
    onError: () => this.notification.showError('PRODUCT_DETAILS.ERROR'),
  });

  constructor(
    private readonly router: Router,
    private readonly product: ProductService,
    private readonly route: ActivatedRoute,
  ) {
    this.route.params
      .pipe(
        takeUntilDestroyed(),
        tap(({ id }) => this.id.set(Number(id))),
        switchMap(({ id }) => this.product.getProduct(Number(id))),
      )
      .subscribe(product => this.form.patchValue(product as ProductFormValue));
  }

  async save(): Promise<void> {
    if (!this.id()) {
      return;
    }
    this.editProduct.mutate(this.form.getRawValue());
  }

  delete(): void {
    if (!this.id()) {
      return;
    }
    try {
    } catch (error) {
      console.error(error);
    }
  }
}

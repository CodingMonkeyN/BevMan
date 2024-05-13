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
import { filter, switchMap, tap } from 'rxjs';
import { ProductDto, ProductService } from '../../../../api';
import { injectMutation, injectQuery, injectQueryClient } from '@ngneat/query';
import { NotificationService } from '../../../services/notification.service';
import { LoadingController } from '@ionic/angular';

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
  readonly #query = injectQuery();
  readonly #mutation = injectMutation();
  readonly #queryClient = injectQueryClient();

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

  protected product = signal<ProductDto | undefined>(undefined);

  private readonly notification = inject(NotificationService);
  protected readonly addProduct = this.#mutation({
    mutationFn: (value: ProductFormValue) => this.productService.createProduct(value),
    onMutate: () => this.loadingController.create().then(loading => loading.present()),
    onSuccess: async () => {
      await this.loadingController.dismiss();
      await this.#queryClient.invalidateQueries({ queryKey: ['products'] });
      await this.router.navigate(['..', { relativeTo: this.route }]);
    },
    onError: async () => {
      await this.loadingController.dismiss();
      return this.notification.showError('PRODUCT_DETAILS.ERROR');
    },
  });
  protected readonly addProductImage = this.#mutation({
    mutationFn: ({ productId, blob }: { productId: number; blob: Blob }) =>
      this.productService.addProductImage(productId, productId, blob),
    onMutate: () => this.loadingController.create().then(loading => loading.present()),
    onSuccess: async () => {
      await this.loadingController.dismiss();
      await this.#queryClient.invalidateQueries({ queryKey: ['products'] });
      await this.#queryClient.invalidateQueries({ queryKey: ['product'] });
    },
    onError: async () => {
      await this.loadingController.dismiss();
      return this.notification.showError('PRODUCT.EDITOR.ADD_IMAGE_ERROR');
    },
  });
  protected readonly deleteProductImage = this.#mutation({
    mutationFn: ({ id }: ProductDto) => this.productService.deleteProductImage(id, id),
    onMutate: () => this.loadingController.create().then(loading => loading.present()),
    onSuccess: async () => {
      await this.loadingController.dismiss();
      await this.#queryClient.invalidateQueries({ queryKey: ['products'] });
      await this.#queryClient.invalidateQueries({ queryKey: ['product'] });
    },
    onError: async () => {
      await this.loadingController.dismiss();
      return this.notification.showError('PRODUCT.EDITOR.DELETE_IMAGE_ERROR');
    },
  });
  protected readonly editProduct = this.#mutation({
    mutationFn: (value: ProductFormValue) =>
      this.productService.updateProduct(this.product()?.id!, {
        ...value,
        id: this.product()!.id,
      }),
    onMutate: () => this.loadingController.create().then(loading => loading.present()),
    onSuccess: async () => {
      await this.loadingController.dismiss();
      await this.#queryClient.invalidateQueries({ queryKey: ['products'] });
      await this.router.navigate(['..', { relativeTo: this.route }]);
    },
    onError: async () => {
      await this.loadingController.dismiss();
      return this.notification.showError('PRODUCT.EDITOR.DELETE_ERROR');
    },
  });
  protected readonly deleteProduct = this.#mutation({
    mutationFn: () => this.productService.deleteProduct(this.product()?.id!),
    onMutate: () => this.loadingController.create().then(loading => loading.present()),
    onSuccess: async () => {
      await this.loadingController.dismiss();
      await this.#queryClient.invalidateQueries({ queryKey: ['products'] });
      await this.router.navigate(['..', { relativeTo: this.route }]);
    },
    onError: async () => {
      await this.loadingController.dismiss();
      return this.notification.showError('PRODUCT.EDITOR.DELETE_ERROR');
    },
  });

  constructor(
    private readonly router: Router,
    private readonly productService: ProductService,
    private readonly loadingController: LoadingController,
    private readonly route: ActivatedRoute,
  ) {
    this.route.params
      .pipe(
        takeUntilDestroyed(),
        filter(({ id }) => !!id),
        switchMap(
          ({ id }) =>
            this.#query({
              queryKey: ['product', id],
              queryFn: () => this.productService.getProduct(id),
            }).result$,
        ),
        tap(product => console.log(product.data)),
        tap(product => this.product.set(product.data)),
      )
      .subscribe(product => this.form.patchValue(product.data as ProductFormValue));
  }

  async save(): Promise<void> {
    if (!this.product()) {
      await this.addProduct.mutateAsync(this.form.getRawValue());
      return;
    }
    await this.editProduct.mutateAsync(this.form.getRawValue());
  }

  async delete(): Promise<void> {
    if (!this.product()) {
      return;
    }
    await this.deleteProduct.mutateAsync(this.product());
  }

  async addFile(input: HTMLInputElement): Promise<void> {
    if (!input.files?.length || !this.product()) {
      return;
    }
    await this.addProductImage.mutateAsync({ productId: this.product()!.id, blob: input.files![0] });
  }
}

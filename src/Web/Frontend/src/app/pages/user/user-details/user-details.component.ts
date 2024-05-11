import { ChangeDetectionStrategy, Component, inject, signal } from '@angular/core';
import { UserPage } from '../user-page';
import {
  IonBackButton,
  IonButton,
  IonButtons,
  IonContent,
  IonHeader,
  IonItem,
  IonNav,
  IonSelect,
  IonSelectOption,
  IonTitle,
  IonToolbar,
} from '@ionic/angular/standalone';
import { takeUntilDestroyed, toSignal } from '@angular/core/rxjs-interop';
import { TranslateModule } from '@ngx-translate/core';
import { FormControl, NonNullableFormBuilder, ReactiveFormsModule } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { switchMap, tap } from 'rxjs';
import { JsonPipe } from '@angular/common';
import { SnakeCasePipe } from '../../../pipes/snakecase.pipe';
import { RoleService, UserDto, UserRoleService, UserService } from '../../../../api';
import { injectMutation } from '@ngneat/query';
import { NotificationService } from '../../../services/notification.service';

interface UserRoleForm {
  roles: FormControl<string[]>;
}

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  standalone: true,
  changeDetection: ChangeDetectionStrategy.OnPush,
  imports: [
    IonBackButton,
    IonTitle,
    IonButtons,
    IonToolbar,
    IonHeader,
    IonNav,
    IonContent,
    IonItem,
    IonSelect,
    IonSelectOption,
    TranslateModule,
    SnakeCasePipe,
    ReactiveFormsModule,
    JsonPipe,
    IonButton,
  ],
})
export class UserDetailsComponent {
  protected readonly root = UserPage;
  protected roles = toSignal(inject(RoleService).getRoles());
  private id = signal<string | undefined>(undefined);
  protected readonly user = signal<UserDto | undefined>(undefined);
  protected readonly form = inject(NonNullableFormBuilder).group<UserRoleForm>({
    roles: new FormControl<string[]>(
      {
        value: [],
        disabled: false,
      },
      { nonNullable: true },
    ),
  });

  private readonly userService = inject(UserService);
  private readonly userRole = inject(UserRoleService);
  private readonly router = inject(Router);
  private readonly route = inject(ActivatedRoute);
  private readonly notification = inject(NotificationService);

  #mutation = injectMutation();
  updateRoles = this.#mutation({
    mutationFn: (value: { roles: string[] }) =>
      this.userRole.updateUserRoles(this.id()!, {
        ...value,
        userId: this.id()!,
      }),
    onSuccess: async () => this.router.navigate(['..'], { relativeTo: this.route }),
    onError: () => this.notification.showError('COMPONENTS.USER_DETAILS.ERROR'),
  });

  constructor() {
    this.route.params
      .pipe(
        takeUntilDestroyed(),
        tap(({ id }) => this.id.set(id)),
        switchMap(({ id }) => this.userService.getUser(id)),
        tap(user => this.user.set(user)),
      )
      .subscribe(user => this.form.patchValue({ roles: user.roles }));
  }

  protected async save(): Promise<void> {
    if (!this.id()) return;
    await this.updateRoles.mutateAsync(this.form.getRawValue());
  }
}

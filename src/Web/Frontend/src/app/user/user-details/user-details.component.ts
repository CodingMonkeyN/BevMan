import {Component, inject, signal} from '@angular/core';
import {UserPage} from "../user-page";
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
  IonToolbar
} from "@ionic/angular/standalone";
import {RoleService, UserDto, UserRoleService, UserService} from "../../../api";
import {takeUntilDestroyed, toSignal} from "@angular/core/rxjs-interop";
import {TranslateModule} from "@ngx-translate/core";
import {SnakeCasePipe} from "../../pipes/snakecase.pipe";
import {FormControl, FormGroup, NonNullableFormBuilder, ReactiveFormsModule} from "@angular/forms";
import {ActivatedRoute, Router} from "@angular/router";
import {catchError, of, switchMap, tap} from "rxjs";
import {JsonPipe} from "@angular/common";

interface UserRoleForm {
  roles: FormControl<string[]>
}

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  standalone: true,
  imports: [
    IonBackButton,
    IonTitle,
    IonButtons,
    IonToolbar, IonHeader,
    IonNav,
    IonContent, IonItem, IonSelect, IonSelectOption, TranslateModule, SnakeCasePipe, ReactiveFormsModule, JsonPipe, IonButton
  ],
})
export class UserDetailsComponent {
  protected readonly root = UserPage;
  protected roles = toSignal(inject(RoleService).getRoles())
  private id = signal<string | undefined>(undefined);
  protected readonly user = signal<UserDto | undefined>(undefined);
  protected readonly form: FormGroup<UserRoleForm>

  constructor(private readonly userService: UserService, private readonly userRole: UserRoleService, private readonly router: Router, private readonly formBuilder: NonNullableFormBuilder, private readonly route: ActivatedRoute) {
    this.form = this.formBuilder.group<UserRoleForm>({roles: this.formBuilder.control([])});

    this.route.params.pipe(
      takeUntilDestroyed(),
      tap(({id}) => this.id.set(id)),
      switchMap(({id}) => this.userService.getUser(id)),
      tap(user => this.user.set(user))
    ).subscribe(user => this.form.patchValue({roles: user.roles}))
  }

  protected async save(): Promise<void> {
    if (!this.id()) return;
    this.userRole.updateUserRoles(this.id()!, {
      ...this.form.getRawValue(),
      userId: this.id()!
    }).pipe(
      catchError(err => {
        console.error(err);
        return of(false);
      }),
    ).subscribe(falseOrNull => {
      if (falseOrNull === null) {
        this.router.navigate(['..'], {relativeTo: this.route});
      }
    });
  }
}

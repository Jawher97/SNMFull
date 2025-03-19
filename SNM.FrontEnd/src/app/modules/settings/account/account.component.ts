import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';
import { Subject, takeUntil } from 'rxjs';

@Component({
    selector       : 'settings-account',
    templateUrl    : './account.component.html',
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class SettingsAccountComponent implements OnInit
{
    accountForm: FormGroup;
    currentUser: User;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    avatarImageUrl: string='assets/images/avatars/brian-hughes.jpg';

    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private formBuilder: FormBuilder,
        private userService: UserService
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        // Create the form
        this.accountForm = this.formBuilder.group({
            avatar: [''],  
            fullName: [''],
            title: [''],
            company: [''],
            about: [''],
            email: [''],
            phone: [''],
            country: ['']
          });
          
        // Subscribe to user changes
        this.userService.user$
        .pipe(takeUntil(this._unsubscribeAll))
        .subscribe((currentUser: User) => {
            this.currentUser = currentUser;

            // Mark for check
            this._changeDetectorRef.markForCheck();
        });

        this.accountForm.patchValue(this.currentUser);

        this.userService.get().subscribe(
          (user) => {
            // Traitez l'utilisateur retourné ici
            this.accountForm.patchValue(user);
          },
          (error) => {
            // Gérez les erreurs ici
            console.error(error);
          }
        );
          

    }

    onSave(): void {
        if (this.accountForm.valid) {
          const updatedUser: User = { ...this.currentUser, ...this.accountForm.value };
          this.userService.update(updatedUser).subscribe(() => {
            // Succès de la mise à jour
          });
        }
      }
    
      onCancel(): void {
        this.accountForm.patchValue(this.currentUser);
      }


      onAvatarSelected(event: any): void {
        const file = event.target.files[0];
        const reader = new FileReader();
        reader.onload = (e: any) => {
          const imageSrc = e.target.result;
          // Mettre à jour l'URL de l'image de l'avatar
          this.currentUser.avatar = imageSrc;

          this.userService.update(this.currentUser).subscribe();

          localStorage.setItem('avatar', imageSrc);
          //this.userService.user = this.currentUser;

        };
        reader.readAsDataURL(file);
      }

      
    }


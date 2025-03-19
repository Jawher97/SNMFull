import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit, ViewEncapsulation } from '@angular/core';
import { FormBuilder, FormGroup, UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { FuseAlertType } from '@fuse/components/alert';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';
import { Subject, takeUntil } from 'rxjs';

@Component({
    selector       : 'settings-security',
    templateUrl    : './security.component.html',
    encapsulation  : ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush
})
export class SettingsSecurityComponent implements OnInit
{
    securityForm: FormGroup;
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    currentUser: User

    currentPassword: string;
    newPassword: string;
    
    alert: { type: FuseAlertType; message: string } = {
      type   : 'success',
      message: ''
  };
    showAlert: boolean = false;


    private baseUrl = "https://localhost:44345/auth/User/";
    /**
     * Constructor
     */
    constructor(
        private http: HttpClient,
        private _changeDetectorRef: ChangeDetectorRef,
        private _formBuilder: FormBuilder,
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
    ngOnInit(): void {
        // Create the form
        this.securityForm = this._formBuilder.group({
          userId: [''],
          currentPassword: ['', Validators.required],
          newPassword: ['', [Validators.required, Validators.minLength(8)]]
        });

          // Subscribe to user changes
          this.userService.user$
          .pipe(takeUntil(this._unsubscribeAll))
          .subscribe((currentUser: User) => {
              this.currentUser = currentUser;
              this.securityForm.patchValue({ userId: this.currentUser.id });
              // Mark for check
              this._changeDetectorRef.markForCheck();
        });
        
      }

    
      changePassword(): void {

          this.http.post(this.baseUrl + 'reset-pwd-without-link', this.securityForm.value).subscribe(
            () => {
              console.log('Password reset successful');

              this.securityForm.reset();
              // Set the alert
              this.alert = {
                type   : 'success',
                message: 'Password reset Successfully!'
            };

            // Show the alert
            this.showAlert = true;
            },
            (error) => {
              console.error('Password reset failed', error);
              this.securityForm.reset();
                // Set the alert
                  this.alert = {
                  type   : 'error',
                  message: 'Current Password incorrect. Please retry!'
                };
              
              // Show the alert
                this.showAlert = true;
            }
          );
      }
}

import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, NgForm, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { AuthService } from 'app/core/auth/auth.service';
import { UserService } from 'app/core/user/user.service';
import { FuseAlertType } from '@fuse/components/alert';
import { finalize } from 'rxjs';

@Component({
    selector     : 'auth-unlock-session',
    templateUrl  : './unlock-session.component.html',
    encapsulation: ViewEncapsulation.None,
    animations   : fuseAnimations
})
export class AuthUnlockSessionComponent implements OnInit
{
    @ViewChild('unlockSessionNgForm') unlockSessionNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type   : 'success',
        message: ''
    };
    name: string;
    showAlert: boolean = false;
    unlockSessionForm: FormGroup;

    private _email: string;
    private _emailToken: string

    /**
     * Constructor
     */
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _authService: AuthService,
        private _formBuilder: FormBuilder,
        private _router: Router,
        private _userService: UserService
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
        this.unlockSessionForm = this._formBuilder.group({
            email: [''],
            emailToken: [''],
        });


        this._activatedRoute.queryParams.subscribe(val=> {
            this._email = val['email'];
            let uriToken = val['code'];
            this._emailToken = uriToken.replace(/ /g,'+');

            this.unlockSessionForm.patchValue({
                email: this._email,
                emailToken: this._emailToken
            });
        });

    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Unlock
     */
    unlock(): void
    {
        // Return if the form is invalid
        if ( this.unlockSessionForm.invalid )
        {
            return;
        }

        // Disable the form
        this.unlockSessionForm.disable();

        // Hide the alert
        this.showAlert = false;

        this._authService.activateAccount(this.unlockSessionForm.value).
        pipe(
            finalize(() => {

                // Re-enable the form
                this.unlockSessionForm.enable();

                // Reset the form
                this.unlockSessionNgForm.resetForm();

                // Show the alert
                this.showAlert = true;
            })
        )
        .subscribe(
            (response) => {

                // Set the alert
                this.alert = {
                    type   : 'success',
                    message: 'Account activated Successfully..'
                };
            },
            (response) => {

                // Set the alert
                this.alert = {
                    type   : 'error',
                    message: 'Something went wrong, please try again.'
                };
            }
        );
    }
}

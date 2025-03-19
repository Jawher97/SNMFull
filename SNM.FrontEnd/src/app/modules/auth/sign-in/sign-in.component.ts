import { Component, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { NgForm, Validators, FormBuilder, FormGroup } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { fuseAnimations } from '@fuse/animations';
import { FuseAlertType } from '@fuse/components/alert';
import { FacebookLoginProvider, SocialAuthService, SocialUser } from 'angularx-social-login';
import { AuthService } from 'app/core/auth/auth.service';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';

@Component({
    selector     : 'auth-sign-in',
    templateUrl  : './sign-in.component.html',
    encapsulation: ViewEncapsulation.None,
    animations   : fuseAnimations
})
export class AuthSignInComponent implements OnInit
{
    @ViewChild('signInNgForm') signInNgForm: NgForm;

    alert: { type: FuseAlertType; message: string } = {
        type   : 'success',
        message: ''
    };
    signInForm: FormGroup;
    showAlert: boolean = false;
    user: User;
    isLoggedin: boolean;
    socialUser: SocialUser;

    
    /**
     * Constructor
     */
    constructor(
        private _activatedRoute: ActivatedRoute,
        private _authService: AuthService,
        private _formBuilder: FormBuilder,
        private _router: Router,
        private socialAuthService: SocialAuthService,
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
        // this.signInForm = this._formBuilder.group({
        //     email     : ['hughes.brian@company.com', [Validators.required, Validators.email]],
        //     password  : ['admin', Validators.required],
        //     rememberMe: ['']
        // });

        const email = localStorage.getItem('email');
        const password = localStorage.getItem('password');


            this.signInForm = this._formBuilder.group({
                email     : ['', [Validators.required, Validators.email]],
                password  : ['', Validators.required],
                rememberMe: [false]
            });
        
            
            if(email && password) {
                this.signInForm.patchValue({
                  email: email,
                  password: password,
                  rememberMe: true // cocher automatiquement la case "Remember Me"
                });
            }

            this.socialAuthService.authState.subscribe((user) => {
                this.socialUser = user;
                this.isLoggedin = user != null;
              });

    }


    signInWithFacebook(): void {
        this.socialAuthService.signIn(FacebookLoginProvider.PROVIDER_ID).then((response) => {
            // Effectuez les actions nécessaires après la connexion réussie avec Facebook
            
            // Redirigez l'utilisateur vers la page principale
            const redirectURL = this._activatedRoute.snapshot.queryParamMap.get('redirectURL') || '/brand';

            // Navigate to the redirect url
            this._router.navigateByUrl(redirectURL);
        });;

    }

    signOutWithFacebook(): void {
        this.socialAuthService.signOut();
      }

    

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Sign in
     */
    signIn(): void
    {
        // Return if the form is invalid
        if ( this.signInForm.invalid )
        {
            return;
        }

        // Disable the form
        this.signInForm.disable();

        // Hide the alert
        this.showAlert = false;

        // Sign in
        this._authService.signIn(this.signInForm.value)
            .subscribe(
                () => {

                    if(this.signInForm.get('rememberMe').value)
                    {
                        localStorage.setItem('email', this.signInForm.get('email').value);
                        localStorage.setItem('password', this.signInForm.get('password').value);
                    }
                    // Set the redirect url.
                    // The '/signed-in-redirect' is a dummy url to catch the request and redirect the user
                    // to the correct page after a successful sign in. This way, that url can be set via
                    // routing file and we don't have to touch here.
                    const redirectURL = this._activatedRoute.snapshot.queryParamMap.get('redirectURL') || '/brand';

                    // Navigate to the redirect url
                    this._router.navigateByUrl(redirectURL);

                },
                (response) => {
                    console.log(response)

                    // Re-enable the form
                    this.signInForm.enable();

                    // Reset the form
                    this.signInNgForm.resetForm();

                    // Set the alert
                    this.alert = {
                        type   : 'error',
                        message: 'Wrong email or password'
                    };

                    // Show the alert
                    this.showAlert = true;
                }
            );
    }
}

import { Component } from '@angular/core';
import { FacebookService } from 'app/core/services-api';

@Component({
  selector: 'facebook-group',
  templateUrl: './facebook-group.component.html',
  styleUrls: ['./facebook-group.component.scss']
})
export class FacebookGroupComponent {
  /**
  * 
    * Constructor
    */
  constructor(
    // private _changeDetectorRef: ChangeDetectorRef,
    // private _brandsService: BrandsService,
    // private _fuseMediaWatcherService: FuseMediaWatcherService,
    // private _matDialog: MatDialog,
    private facebookService: FacebookService
  ) {
  }
  // -----------------------------------------------------------------------------------------------------
  // @ Public methods
  // -----------------------------------------------------------------------------------------------------

  login() {
  //   this.facebookService.FacebookGroupLogin()
  //     .subscribe(() => {
  //       // get return url from query parameters or default to home page
  //       // const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
  //       // this.router.navigateByUrl(returnUrl);
  //     });
   }
}

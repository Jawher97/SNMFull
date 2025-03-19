import { ChangeDetectorRef, Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { ChannelProfile } from 'app/core/models/ChannelProfile.model';
import { LinkedinProfileData } from 'app/core/models/linkedin-profile-data';
import { LinkedInService } from 'app/core/services-api/linkedin.service';
import { BehaviorSubject, Observable, Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-linkedin-profile-channel',
  templateUrl: './linkedin-profile-channel.component.html',
  styleUrls: ['./linkedin-profile-channel.component.scss']
})
export class LinkedinProfileChannelComponent implements OnInit, OnDestroy {
  private _unsubscribeAll: Subject<any> = new Subject<any>();
  linkedinProfileData$: Observable<ChannelProfile>
  linkedinProfileData: ChannelProfile
  private _linkedinProfile: BehaviorSubject<ChannelProfile | null> = new BehaviorSubject(null)
  /**
       * Constructor
       */
  constructor(
    private _changeDetectorRef: ChangeDetectorRef,
    private _linkedinservice: LinkedInService,
    private _matDialogRef: MatDialogRef<LinkedinProfileChannelComponent>

  ) {
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Lifecycle hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    // Get the contacts

    this.linkedinProfileData$ = this._linkedinservice.linkedinProfile$;
    this._linkedinservice.linkedinProfile$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((linkedinProfileData: ChannelProfile) => {
        this._linkedinProfile.next(linkedinProfileData)
        // Update the counts
        //  this.contactsCount = contacts.length;

        // Mark for check
        this._changeDetectorRef.markForCheck();
      });

    this._linkedinservice.login()
    //  this._linkedinservice.linkedinProfile$.pipe(takeUntil(this._unsubscribeAll)).subscribe(x => {
    //               this._linkedinProfileData = x
    //               // Mark for check
    //               this._changeDetectorRef.markForCheck();   })
    // this.openlinkedinProfileDataDialog(x) // Message received from child
    // window.open(`https://www.linkedin.com/oauth/v2/authorization?response_type=code&state=true&client_id=77vkkcv6n3ct4x&redirect_uri=https://localhost:4200/callback&scope=r_liteprofile%20r_emailaddress%20w_member_social%20rw_organization_admin%20r_organization_social%20w_organization_social%20r_organization_admin`, "toolbar=no,location=no,directories=no,status=no,menubar=no,titlebar=no,fullscreen=no,scrollbars=1,resizable=no,width=430,height=400,left=500,top=100");
    // window.addEventListener('message', (event) => {


    //   if (event.origin === 'https://localhost:4200' && event.type === 'message' && event.data.type === undefined) {
    //     // Access the data sent in the message                
    //     const expr = event?.data?.split(":")[0];
    //     const receivedData = event.data.split(":")[1]
    //     console.log("event.data.type "+receivedData);
    //     switch (expr) {
    //       case 'linkedInCode':
    //         {
    //           this.linkedinservice.generateProfileAccessTooken(receivedData) .pipe(takeUntil(this._unsubscribeAll)).subscribe(x => {
    //             this.linkedinProfileData = x
    //             // Mark for check
    //             this._changeDetectorRef.markForCheck();
    //             // this.openlinkedinProfileDataDialog(x) // Message received from child
    //           })
    //           break;
    //         }
    //       default:
    //         console.log(`Sorry, we are out of ${expr}.`);
    //     }
    //   } else return

    // });
  }
  get linkedinProfile$(): Observable<ChannelProfile> {
    return this._linkedinProfile.asObservable();
  }
  save() {
    // this._linkedinservice.
    // this._matDialogRef.close();
  }
  // Close the dialog
  close() { 
    this._matDialogRef.close();
  }

  ngOnDestroy(): void {
    console.log("distroyed");
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();

  }
  /**
* Track by function for ngFor loops
*
* @param index
* @param item
*/
  trackByFn(index: number, item: any): any {
    return item.id || index;
  }
}

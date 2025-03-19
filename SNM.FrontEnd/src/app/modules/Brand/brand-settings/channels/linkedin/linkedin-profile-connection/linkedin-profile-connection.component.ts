import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { LinkedInService } from 'app/core/services-api/linkedin.service';
import { map } from 'rxjs';
import { LinkedinProfileChannelComponent } from '../linkedin-profile-channel/linkedin-profile-channel.component';
import { LinkedinProfileData } from 'app/core/models/linkedin-profile-data';
import { MatDialog } from '@angular/material/dialog';
import { cloneDeep } from 'lodash';
import { ChannelProfile } from 'app/core/models/ChannelProfile.model';

@Component({
  selector: 'linkedin-profile-connection',
  templateUrl: './linkedin-profile-connection.component.html',
  styleUrls: ['./linkedin-profile-connection.component.scss']
})
export class LinkedinProfileConnectionComponent implements OnInit {
  windowRef = null;
  linkedInProfile: ChannelProfile;
  accessToken;
  /**
      * Constructor
      */
  constructor(
    private linkedinservice: LinkedInService,
    private _changeDetectorRef: ChangeDetectorRef,
    private _matDialog: MatDialog,) {

  }
  ngOnInit(): void {
    window.addEventListener('message', (event) => {
      if (event.origin === 'https://localhost:4200' && event.type === 'message' && event.data.type === undefined) {
        // Access the data sent in the message                
        const expr = event?.data?.split(":")[0];
        const receivedData = event.data.split(":")[1]
        switch (expr) {
          case 'linkedInCode':
            {
              this.connectToLinkedinProfileDataDialog();
              this.linkedinservice.generateAccessToken(receivedData,"currentBrandID").subscribe(x => {
                this.linkedInProfile = x
                // Mark for check
                this._changeDetectorRef.markForCheck();
                // this.openlinkedinProfileDataDialog(x) // Message received from child
               
              })
              break;
            }
          default:
            console.log(`Sorry, we are out of ${expr}.`);
        }
      } else return
    });
    // this.linkedinservice.linkedinProfile$.subscribe(x=>
    //   this.linkedInProfile=x)
  }

  login() {
    // this.connectToLinkedinProfileDataDialog();
    this.linkedinservice.login()
    // this.windowRef = window.open(`https://www.linkedin.com/oauth/v2/authorization?response_type=code&state=true&client_id=77vkkcv6n3ct4x&redirect_uri=https://localhost:4200/callback&scope=r_liteprofile%20r_emailaddress%20w_member_social%20rw_organization_admin%20r_organization_social%20w_organization_social%20r_organization_admin`, "toolbar=no,location=no,directories=no,status=no,menubar=no,titlebar=no,fullscreen=no,scrollbars=1,resizable=no,width=430,height=400,left=500,top=100");
  }


  receivemessage(evt: any) {
    console.log("receivemessage " + evt.data)
  }
  /**
      * Open the note dialog
      */
  openlinkedinProfileDataDialog(linkedinProfileData: LinkedinProfileData): void {
    this._matDialog.open(LinkedinProfileChannelComponent, {
      autoFocus: true,
      disableClose: false,
      data: {
        linkedinProfileData: cloneDeep(linkedinProfileData)
      }
    });
  }
  connectToLinkedinProfileDataDialog(): void {
    this._matDialog.open(LinkedinProfileChannelComponent, {
      autoFocus: true,
      disableClose: true,
      data: {
        // linkedinProfileData: cloneDeep(linkedinProfileData)
      }
    });
  }

  logOut(): void {
    localStorage.removeItem('LinkedIn access_token');
    localStorage.removeItem('LinkedIn refresh_token');

  }
  getAccessToken() {
    this.linkedinservice.code$.subscribe(x => this.accessToken = x);
  }
}

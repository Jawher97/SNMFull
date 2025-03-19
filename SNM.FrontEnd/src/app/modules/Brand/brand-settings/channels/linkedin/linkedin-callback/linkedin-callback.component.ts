import { ChangeDetectorRef, Component,  OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { LinkedInChannelDto } from 'app/core/models/brand.model';
import { LinkedinProfileData } from 'app/core/models/linkedin-profile-data';
import { LinkedInService } from 'app/core/services-api/linkedin.service';

@Component({
  selector: 'app-linkedin-callback',
  templateUrl: './linkedin-callback.component.html',
  styleUrls: ['./linkedin-callback.component.scss']
})
export class LinkedinCallbackComponent implements OnInit{

  code = "";
  socialChannels: LinkedInChannelDto[];
  linkedInProfile:LinkedinProfileData;
  linkedInToken: string;
  constructor(private route: ActivatedRoute,
    
     private _changeDetectorRef: ChangeDetectorRef,
     private router: Router) {}
 
  
  ngOnInit() :void{
console.log(this.route.snapshot.routeConfig["path"]);

    this.linkedInToken = this.route.snapshot.queryParams["code"];
    // this.linkedInToken = this.route.snapshot.routeConfig["path"];
    window.opener.postMessage("linkedInCode:"+this.linkedInToken, "*");
    // window.close();
  }


}

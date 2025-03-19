import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ChannelTypeService } from 'app/core/services-api/channelType.service';
import { LinkedInService } from 'app/core/services-api/linkedin.service';

@Component({
  selector: 'social-channel-callback',
  templateUrl: './social-channel-callback.component.html',
  styleUrls: ['./social-channel-callback.component.scss']
})
export class SocialChannelCallbackComponent implements OnInit {
  rootCode: string
  

  /**
   * Constructor
   */
  constructor(private _linkedinservice: LinkedInService,private _channelTypeService: ChannelTypeService,private route: ActivatedRoute,) {

  }
  ngOnInit(): void {
    const responseData = {
      source: this.route.snapshot.routeConfig["path"],
      
      code: this.route.snapshot.queryParams["code"]
    }
      console.log(JSON.stringify(responseData)+"response")
   
        //window.opener.postMessage(responseData, "*");
      this.Window(responseData)
} 
 Window(responseData:any){
  this._linkedinservice.notificationMessage.subscribe((responseData) => {
    console.log('receive message', responseData);
    
  });
  window.opener.postMessage(responseData, "*");
 }
    // window.close();
  }



import { ChangeDetectorRef, Component, Input, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ChannelType } from 'app/core/models/channelType.model';
import { SocialChannel } from 'app/core/models/socialChannel.model';
import { ChannelTypeService } from 'app/core/services-api/channelType.service';
import { SocialChannelService } from 'app/core/services-api/socialChannel.service';
import { ISocialChannel } from 'app/core/types/socialChannel.types';
import {  facebookPaths, facebookGroupsPaths,instagramPaths, linkedInPaths, twitterPaths } from 'environments/environment.development';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-social-channel',
  templateUrl: './social-channel.component.html',
  styleUrls: ['./social-channel.component.scss']
})
export class SocialChannelComponent implements OnInit {
  @Input() channelType!: ChannelType;
  @Input() success!: boolean;
  connected:boolean=false;

  /**
     * Constructor
     */
  constructor(
    private _channelService: SocialChannelService,
    private _channelTypeService: ChannelTypeService,
    private _changeDetectorRef: ChangeDetectorRef,
    private _matDialog: MatDialog,
    private toastr:ToastrService,    private _channelTypesService:ChannelTypeService,) {

  }
  ngOnInit(): void {
      

  }
  logOut(channels:SocialChannel[]){
    this._channelService.DeleteChannels(channels).subscribe(x => {
     this.toastr.success("Disconnect Successfully","SNM")
    
     window.location.reload();
    })
  }
  login(channelType:string) {
    switch(channelType){
      case'LinkedIn Profile':
      {
       
       this._channelTypeService.connectChannel(linkedInPaths.oauthUrl)
       this.connected=this.success
       console.log(this.connected+"connrctrd")
        break;
      } 
      case'Facebook Page':
      {
        this._channelTypeService.connectChannel(facebookPaths.oauthUrl)
        break;
      } 
      case'Facebook Group':
      {
        this._channelTypeService.connectChannel(facebookGroupsPaths.oauthUrl)
   
        break;
      }
      case'Twitter Profile':
      {
        this._channelTypeService.connectChannel(twitterPaths.oauthUrl)
       
        break;
      }
      case'Instagram Profile':
      {
        //this._channelTypeService.connectChannel(facebookPaths.oauthUrl)
         this._channelTypeService.connectChannel(instagramPaths.oauthUrl)
        break;
      }
      default:
        console.log('sorry, we are out of${channelType}');
        
    }
    
    
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
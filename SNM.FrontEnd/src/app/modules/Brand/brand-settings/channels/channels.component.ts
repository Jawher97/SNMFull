import { ChangeDetectionStrategy, ChangeDetectorRef, Component, EventEmitter, OnInit, ViewEncapsulation } from '@angular/core';
import { BrandComponent } from '../../brand.component';
import { ActivatedRoute, Router } from '@angular/router';
import { FacebookService } from 'app/core/services-api';
import { LinkedInService } from 'app/core/services-api/linkedin.service';
import { SocialAuthService } from 'angularx-social-login';
import { SocialChannelService } from 'app/core/services-api/socialChannel.service';
import { BrandsService } from 'app/core/services-api/brands.service';
import { Observable, Subject, map, takeUntil } from 'rxjs';
import { Brand } from '../../../../core/models/brand.model';
import { SocialChannel } from 'app/core/models/socialChannel.model';
import { IChannelType } from 'app/core/types/channelType.types';
import { ChannelTypeService } from 'app/core/services-api/channelType.service';
import { ChannelType } from 'app/core/models/channelType.model';
import { MatDialog } from '@angular/material/dialog';
import { ManagedSocialChannelComponent } from './managed-social-channel/managed-social-channel.component';
import { InstagramService } from 'app/core/services-api/instagram.service';
import { TwitterService } from 'app/core/services-api/twitter.service';
import { ToastrService } from 'ngx-toastr';
import { CreatePostsComponent } from 'app/modules/posts/create-posts/create-posts.component';

@Component({
  selector: 'channels',
  templateUrl: './channels.component.html',
styleUrls: ['./channels.component.scss'],
 
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class ChannelsComponent implements OnInit {
  authWindow: Window;
  currentBrand:Brand;
  channelList: SocialChannel[];
  channelList_Filtred: SocialChannel[];
  channelList_Selected: SocialChannel[] = [];
  channelTypeList: IChannelType[];
  success:boolean=false;
  private _unsubscribeAll: Subject<any> = new Subject<any>();
  notificationMessage = new EventEmitter();
  /**
       * Constructor
       */
  message: string = '';


  
  constructor(private _brandComponent: BrandComponent, private router: Router,
    private route: ActivatedRoute,
    private accountService: FacebookService,
    private _brandsService: BrandsService,
    private _socialChannelService:SocialChannelService,
    private _channelTypesService:ChannelTypeService,
    private _facebookService:FacebookService,
    private _linkedinservice: LinkedInService,
    private _twitterService:TwitterService,
    private _instagramService:InstagramService,
    private _changeDetectorRef: ChangeDetectorRef,
    private _matDialog: MatDialog,
    private toastr:ToastrService) {
    
    
  }
 
  ngOnInit(): void {
    
    window.addEventListener('message', (event) => {
      
    this._linkedinservice.notificationMessage.emit(event);

      if (event.origin === 'https://localhost:4200' && event.type === 'message' && event.data.type === undefined) {
       
        const expr = event.data.source;
        const accessCode = event.data.code;   
           
        switch (expr) {
          case 'callback':
         
       {
             
             this._linkedinservice.generateAccessToken(accessCode,this.currentBrand?.id).subscribe(x => {
              if(x.succeeded){  
                this.success=true 
                console.log(this.success+"sssss")
                this.connectDataDialog(x.data,"LinkedIn");             
                  this._changeDetectorRef.markForCheck();
                
              }
             else{

              this.toastr.error(x.message,"SNM")
             }
             
              })
             
              break;
            } 
            case 'facebookcallback':
            {
             
              this._facebookService.generateAccessToken(accessCode,this.currentBrand?.id,"Facebook Page").subscribe(x => {
                if(x.succeeded){  
                  this.success=true  
                  console.log(this.success + "ssuuuuuccceeedChannnels")
                  //console.log(JSON.stringify(x.data)+"dataaaaaa")
                  this.connectDataDialog(x.data,"Facebook");
                  
                }
               else{

                this.toastr.error(x.message,"SNM")
               }
                // this.linkedInProfile = x
                // Mark for check
                this._changeDetectorRef.markForCheck();
               })
             
              break;
            }
            case 'facebookForGroupscallback':
              {

               // this.connectDataDialog();
                this._facebookService.generateAccessTokenForGroups(accessCode,this.currentBrand?.id,"Facebook Group").subscribe(x => {
                  if(x.succeeded){ 
                    this.success=true                 
                    this.connectDataDialog(x.data,"Facebook");
                    
                  }
                 else{
  
                  this.toastr.error(x.message,"SNM")
                 }
                  
                  this._changeDetectorRef.markForCheck();
                 })
               
                break;
              }
            case 'instagramcallback':
            {
             // this.connectDataDialog();
              this._instagramService.generateAccessToken(accessCode,this.currentBrand?.id).subscribe(x => {
                if(x.succeeded){   
                  this.success=true 
                  window.close();
                  //console.log(JSON.stringify(x.data)+"dataaaaaa")
                  this.connectDataDialog(x.data,"Facebook ");
                  
                }
               else{

                this.toastr.error(x.message,"SNM")
               }
                
                this._changeDetectorRef.markForCheck();
               })
             
              break;
            }
            case 'twittercallback':
            {
             // console.log("aaaaaa")
              // this.connectDataDialog();
               this._twitterService.generateAccessToken(accessCode,this.currentBrand?.id).subscribe(x => {
                if(x.succeeded){  
                  this.success=true  
                 // console.log(JSON.stringify(x.data)+"dataaaaaa")
                  this.connectDataDialog(x,"Twitter ");
                }
               else{

                this.toastr.error(x.message,"SNM")
               }
                
                this._changeDetectorRef.markForCheck();
               })
             
              break;
            }
          default:
            console.log(`Sorry, we are out of ${expr}.`);
        }
      }
       else return
    });
    // window.addEventListener('message', (event) => {

    //   if (event.origin === 'https://localhost:4200') {
    
    //     // Access the data sent in the message
    
    //     const receivedData = event.data;
    
    //     console.log('Received data:', receivedData);
    //     this.linkedinService.code.next(this.linkedInToken);
    //   }
    
    // });
    // Get Current Brand
    this._brandsService.brand$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((brand: Brand) => {
        // Get the brandName
        this.currentBrand = brand;
       
        this._channelTypesService.getChannelTypes(this.currentBrand?.id).subscribe();
        // Mark for check
        this._changeDetectorRef.markForCheck();
      });
     

     this._channelTypesService.channelTypes$
     .pipe(takeUntil(this._unsubscribeAll))
     .subscribe((channels: ChannelType[]) => {
       
        this.channelTypeList = channels
      
        //console.log(JSON.stringify(channels)+"channnnels")
        // Mark for check
        this._changeDetectorRef.markForCheck();
     });
   
  }
 

  // -----------------------------------------------------------------------------------------------------
  // @ Public methods
  // -----------------------------------------------------------------------------------------------------

  connectDataDialog(channelProfile:any,name:string): void {
   // console.log(channelProfile+"channelProfile")
    const dialogRef = this._matDialog.open(ManagedSocialChannelComponent, {
      autoFocus: true,
      disableClose: true,
      data: {"channelProfile":channelProfile,"name":name}
      
    });
    dialogRef.afterClosed().subscribe(result => {
    
      this._brandsService.brand$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((brand: Brand) => {
        // Get the brandName
        this.currentBrand = brand;
        // this._socialChannelService.getSocialChannels(this.currentBrand?.id).subscribe();
        this._channelTypesService.getChannelTypes(this.currentBrand?.id).subscribe();
        // Mark for check
        this._changeDetectorRef.markForCheck();
      });
      this.success=true 
     // Get the channel to filter
     console.log(this.currentBrand?.id);
    
    
    }) 
  }
  /**
   * Toggle the drawer
   */
  toggleDrawer(): void {
    // Toggle the drawer
    this._brandComponent.matDrawer.toggle();
  }
  
  launchFbLogin() {
    this.authWindow = window.open('https://www.facebook.com/v2.11/dialog/oauth?&response_type=token&display=popup&client_id=1290057835057119&display=popup&redirect_uri=http://localhost:5000/facebook-auth.html&scope=pages_show_list', null, 'width=600,height=400');
  } 
 
  addNewPost(): void
  {
     let dialogRef= this._matDialog.open(CreatePostsComponent, {
          autoFocus: false,
          data     : {
              post: {}
          } 
      });
  
  dialogRef.afterClosed().subscribe(result => {
      
          
      
    });
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

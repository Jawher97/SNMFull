// import { ChangeDetectorRef, Component, OnDestroy, OnInit } from '@angular/core';
// import { FacebookChannelDto } from 'app/core/models/brand.model';
// import { SocialChannel } from 'app/core/models/socialChannel.model';
// import { FacebookService } from 'app/core/services-api';
// import { BrandsService } from 'app/core/services-api/brands.service';
// import { SocialChannelService } from 'app/core/services-api/socialChannel.service';
// import { IChannelType } from 'app/core/types/channelType.types';
// import { Observable, Subject, takeUntil } from 'rxjs';

// @Component({
//   selector: 'facebook-pages',
//   templateUrl: './facebook-pages.component.html',
//   styleUrls: ['./facebook-pages.component.scss']
// })
// export class FacebookPagesComponent implements OnInit, OnDestroy {

//   socialChannels: FacebookChannelDto[];
//   facebookChannels$: Observable<FacebookChannelDto[]>;
//   connected: boolean = false;
//   facebookChannelList: SocialChannel[];
//   channelTypeList: IChannelType[];
//     // Private
//     private _unsubscribeAll: Subject<any> = new Subject<any>();
    
//   /**
//    * 
//      * Constructor
//      */
//   constructor(
//      private _changeDetectorRef: ChangeDetectorRef,
//      private _brandsService: BrandsService,
//     // private _fuseMediaWatcherService: FuseMediaWatcherService,
//     // private _matDialog: MatDialog,
//     private facebookService: FacebookService,
//     private _socialChannelService:SocialChannelService,
//   ) {
//   }
//   // -----------------------------------------------------------------------------------------------------
//   // @ Lifecycle hooks
//   // -----------------------------------------------------------------------------------------------------

//   /**
//    * On init
//    */
//   ngOnInit(): void {
//  // Get the contacts
//  this.facebookChannels$ = this._brandsService.facebookChannels$;
//  this._brandsService.facebookChannels$
//      .pipe(takeUntil(this._unsubscribeAll))
//      .subscribe((facebookChannel: FacebookChannelDto[]) => {

//         //  // Update the counts
//         //  this.contactsCount = facebookChannel.length;
//         this.socialChannels = facebookChannel;
//          // Mark for check
//          this._changeDetectorRef.markForCheck();
//      });


//      this._socialChannelService.socialChannels$
//      .pipe(takeUntil(this._unsubscribeAll))
//      .subscribe((channels: SocialChannel[]) => {
//        this.facebookChannelList = channels.filter(x=>x.channelType.name==="Facebook Page")
//        this.channelTypeList = channels.map(channel => channel.channelType);
//      //  this.filtersChannel.channelTypeSlug$.next(this.channelTypeList[0]);
//        // Mark for check
//        this._changeDetectorRef.markForCheck();
//      });

//     // // Request the data from the server
//     // this._notesService.getSocialChannels("08db4587-5d6c-47a1-8dd1-8a963a5bd8d0").subscribe();
//     // // Get the boards
//     // this._notesService.socialChannels$
//     // .pipe(takeUntil(this._unsubscribeAll))
//     // .subscribe((boards: FacebookChannelDto[]) => {
//     //     this.socialChannels = boards;

//     //     // Mark for check
//     //     this._changeDetectorRef.markForCheck();
//     // });
//     // this._notesService.getNotes().subscribe();

//     // Get labels
//     // this.socialChannels = this._notesService.socialChannels$;
//     console.log(this.socialChannels);
    
    
//   }

//   /**
//    * On destroy
//    */
//   ngOnDestroy(): void {
//     // Unsubscribe from all subscriptions
//     // this._unsubscribeAll.next(null);
//     // this._unsubscribeAll.complete();
//   }

//   // -----------------------------------------------------------------------------------------------------
//   // @ Public methods
//   // -----------------------------------------------------------------------------------------------------

//   login() {
//     this.facebookService.login()
//       .subscribe(() => {
//         // get return url from query parameters or default to home page
//         // const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
//         // this.router.navigateByUrl(returnUrl);
//       });
//       this.connected = true;
//   } 
//   FacebookPagesLogin() {
//     this.facebookService.FacebookPagesLogin();
//       // .subscribe(() => {
//       //   // get return url from query parameters or default to home page
//       //   // const returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
//       //   // this.router.navigateByUrl(returnUrl);
//       // });
//       this.connected = true;
//   }

//   logOut(): void {

//     //localStorage.removeItem('instagram_access_token');

//     this.connected = false

//   }
// /**
//      * Track by function for ngFor loops
//      *
//      * @param index
//      * @param item
//      */
// trackByFn(index: number, item: any): any
// {
//     return item.id || index;
// }
// }

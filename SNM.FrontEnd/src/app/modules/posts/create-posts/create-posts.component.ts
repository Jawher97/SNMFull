import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, Inject, OnDestroy, OnInit, Renderer2, TemplateRef, ViewChild, ViewContainerRef, ViewEncapsulation } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { Post } from 'app/core/models/post.model';
import { PostService } from 'app/core/services-api/post.service';
import { UplodeFileService } from 'app/core/services-api/uplode-file.service';
import { BehaviorSubject, combineLatest, debounceTime, Observable, of, Subject, switchMap, takeUntil } from 'rxjs';
import * as moment from 'moment';
import { ThemePalette } from '@angular/material/core';
import { MatRadioChange } from '@angular/material/radio';
import { IlinkPreview } from 'app/core/types/IlinkPreview';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { FacebookService } from 'app/core/services-api/facebook.service';
import { LinkedInService } from 'app/core/services-api/linkedin.service';
import { TwitterService } from 'app/core/services-api/twitter.service';


import { BrandsService } from 'app/core/services-api/brands.service';
import { OverlayRef, Overlay } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { SocialChannel } from 'app/core/models/socialChannel.model';
import { SocialChannelService } from 'app/core/services-api/socialChannel.service';
import { IChannelType } from 'app/core/types/channelType.types';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { GenericPublishingPosts } from 'app/core/models/genericPublishingPosts.model';
import { PublicationStatusEnum } from 'app/core/enumerations/PublicationStatusEnum';
import { IGenericPublishingPosts } from 'app/core/types/genericPublishingPosts.types';
import { ToastrService } from 'ngx-toastr';
import { MediaTypeEnum } from 'app/core/enumerations/MediaTypeEnum';
import { Brand } from 'app/core/models/brand.model';
import { ChannelTypeService } from 'app/core/services-api/channelType.service';
import { ChannelType } from 'app/core/models/channelType.model';
import { Response } from 'app/core/types/Response.type';
import { ISocialChannel } from 'app/core/types/socialChannel.types';
import { DatePipe } from '@angular/common';
@Component({
  selector: 'app-create-posts',
  templateUrl: './create-posts.component.html',
  styleUrls: ['./create-posts.component.scss'],
  encapsulation: ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})

export class CreatePostsComponent implements OnInit, OnDestroy {
  formData = new FormData();
  postForm: FormGroup;
  channelExist:any=["LinkedIn Profile","Facebook Page","Facebook Group","LinkedIn Page"]
  channelExistInstagram:any=["Instagram Profile"]
  socialChannels: SocialChannel[] = [];
  preview: IlinkPreview;
  Images: any[] = []
  // Regular Expression for validating the link the user enters
  private regExHyperlink = /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/;
  link = new FormControl('', [Validators.required, Validators.pattern(this.regExHyperlink)]);
  private _unsubscribeAll: Subject<any> = new Subject<any>();
  //tochange

  schedule: boolean = false;


  private _channelsPanelOverlayRef: OverlayRef;
  @ViewChild('channelsPanel') private _channelsPanel: TemplateRef<any>;
  @ViewChild('channelsPanelOrigin') private _channelsPanelOrigin: ElementRef;
  public isEmojiPickerVisible: boolean;
  filtersChannel: {
    channelTypeSlug$: BehaviorSubject<IChannelType>;

  } = {
      channelTypeSlug$: new BehaviorSubject(null),
    };

  channelList: ISocialChannel[];
  channelList_Filtred: ISocialChannel[]=[];
  channelList_Selected: SocialChannel[] = [];
  channelTypeList: IChannelType[];
  currentBrand: Brand;
  message:any=''
 
  post$: Observable<GenericPublishingPosts>;
  postChanged: Subject<GenericPublishingPosts> = new Subject<GenericPublishingPosts>();
  errorMessages: string[] = [];
  post: IGenericPublishingPosts = {
    socialChannels: [],
    message: '',
    publicationDate: new Date(),
    publicationStatus: PublicationStatusEnum.Draft,
    scheduleTime: new Date(),
    mediaData: []
  }


   videoCount = 0;
   imageCount = 0;
   succeess=false;
   showEmojiPicker: boolean = false;

  sets = [
    'native',
    'google',
    'twitter',
    'facebook',
    'emojione',
    'apple',
    'messenger',
  ];
  searchTerm: string = '';
  set: string = 'facebook';
  /**
   * Constructor
   */
  constructor(private http: HttpClient,
  
    private _changeDetectorRef: ChangeDetectorRef,
    @Inject(MAT_DIALOG_DATA) private _data: { post: Post },
    private _postsService: PostService,
    private formBuilder: FormBuilder,
    private _facebookService: FacebookService,
    private _linkedinService: LinkedInService,
    private _twitterService: TwitterService,
    private _uplodeService: UplodeFileService,
    // private _matDialogRef: MatDialogRef<PostsDetailsComponent>,
    private _brandsService: BrandsService,
    private _socialChannelService: SocialChannelService,
    private _renderer2: Renderer2,
    private _overlay: Overlay,
    private _viewContainerRef: ViewContainerRef,
    private toastr: ToastrService,
    private _channelTypesService:ChannelTypeService,
    public dialogRef: MatDialogRef<CreatePostsComponent>,
   private _channelTypeService:ChannelTypeService

  ) {
  }


  // -----------------------------------------------------------------------------------------------------
  // @ Lifecycle hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {

    const geniricPost = new GenericPublishingPosts(this.post);
    this.post$ = of(geniricPost);
   
        // this._changeDetectorRef.detectChanges();
  
    this.postChanged
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe(() => {
        
        // Mark for check
        this._changeDetectorRef.markForCheck();
      });

     
      // this.postChanged.subscribe((newPost) => {
      //   this.errorMessages=[];
      //   // This code block will be executed whenever a new value is emitted through postChanged
      //   console.log('Post has changed:', newPost);
      //   this._changeDetectorRef.markForCheck();
      //   // You can perform actions here based on the changes in newPost
      // });
    // Get Current Brand
    this._brandsService.brand$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((brand: Brand) => {
        // Get the brandName
        this.currentBrand = brand;

        // Mark for check
        this._changeDetectorRef.markForCheck();
      });

   
 
    this._socialChannelService.getSocialChannels(this.currentBrand?.id).subscribe();
   
    this._channelTypeService.channelTypes$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((channelsType: ChannelType[]) => {
      
        this.channelList = channelsType.flatMap(channelsType => channelsType.channels)
        this.channelTypeList = channelsType
       
        this.filtersChannel.channelTypeSlug$.next(this.channelTypeList[0]);

        // Mark for check
        this._changeDetectorRef.markForCheck();
      });

      this._channelTypeService.getChannelTypes(this.currentBrand?.id).subscribe();
   
    //Filter the channel
    combineLatest([this.filtersChannel.channelTypeSlug$])
    .pipe(takeUntil(this._unsubscribeAll))
    .subscribe(([channelTypeSlug]) => {
      if (channelTypeSlug !== null) {
        // Find the corresponding channelType in channelTypeList using the channelTypeSlug
        const selectedChannelType = this.channelTypeList.find(
          (type) => type.id === channelTypeSlug.id
        );
  
        if (selectedChannelType) {
          // Filter the channelList based on channelTypeSlug
          this.channelList_Filtred = selectedChannelType.channels;
          this._changeDetectorRef.detectChanges();
        } else {
          // Handle the case when the channelTypeSlug is not found
          this.channelList_Filtred = [];
        }
      } else {
        // Reset the filtered channel when no filter is applied
        this.channelList_Filtred = this.channelList;
      }
    });
  }

  searchEmojis(searchTerm: string) {
    this.searchTerm = searchTerm;
  }
  toggleEmojiPicker(): void {
    this.showEmojiPicker = !this.showEmojiPicker;
  }

  addEmoji(event): void {

    const text = `${this.message} ${event.emoji.native}`;
    this.message = text;
    this.post.message=this.message
   
    this.showEmojiPicker = false;
  }

  


  /**
   * On destroy
   */
  ngOnDestroy(): void {
    // Unsubscribe from all subscriptions
    this._unsubscribeAll.next(null);
    this._unsubscribeAll.complete();

    // Dispose the overlays if they are still on the DOM
    if (this._channelsPanelOverlayRef) {
      this._channelsPanelOverlayRef.dispose();
    }
  }

  // -----------------------------------------------------------------------------------------------------
  // @ Public methods
  // -----------------------------------------------------------------------------------------------------
  /**
  * Open add channel panel
  */
  openAddChanelPanel(): void {
    // Create the overlay
    this._channelsPanelOverlayRef = this._overlay.create({
      backdropClass: '',
      hasBackdrop: true,
      scrollStrategy: this._overlay.scrollStrategies.block(),
      positionStrategy: this._overlay.position()
        .flexibleConnectedTo(this._channelsPanelOrigin.nativeElement)
        .withFlexibleDimensions(true)
        .withViewportMargin(64)
        .withLockedPosition(true)
        .withPositions([
          {
            originX: 'start',
            originY: 'bottom',
            overlayX: 'start',
            overlayY: 'top'
          }
        ])
    });

    // Subscribe to the attachments observable
    this._channelsPanelOverlayRef.attachments().subscribe(() => {

      // Add a class to the origin
      this._renderer2.addClass(this._channelsPanelOrigin.nativeElement, 'panel-opened');

      // Focus to the search input once the overlay has been attached
      //  this._channelsPanelOverlayRef.overlayElement.querySelector('input').focus();
    });

    // Create a portal from the template
    const templatePortal = new TemplatePortal(this._channelsPanel, this._viewContainerRef);

    // Attach the portal to the overlay
    this._channelsPanelOverlayRef.attach(templatePortal);

    // Subscribe to the backdrop click
    this._channelsPanelOverlayRef.backdropClick().subscribe(() => {

      // Remove the class from the origin
      this._renderer2.removeClass(this._channelsPanelOrigin.nativeElement, 'panel-opened');

      // If overlay exists and attached...
      if (this._channelsPanelOverlayRef && this._channelsPanelOverlayRef.hasAttached()) {
        // Detach it
        this._channelsPanelOverlayRef.detach();

        // Reset the tag filter
        // this.filtered = this.tags;
      }

      // If template portal exists and attached...
      if (templatePortal && templatePortal.isAttached) {
        // Detach it
        templatePortal.detach();
      }
    });
  }

  /**
      * Filter channelks
      *
      * @param change
  */
  selectedType = (event: MatTabChangeEvent): void => {
    var type = this.channelTypeList.find(x => x.name.trim() == event.tab.textLabel.trim());
    this.filtersChannel.channelTypeSlug$.next(type);
  }

  /**
 * Toggle selected channel to be published
 *
 * @param channel
 */
  toggleSelectedChannel(channel: SocialChannel): void {
    if (this.channelList_Selected.map(channel =>
      channel.id

    ).includes(channel.id)) {

      this.removeChannelFromPublishing(channel);
    }
    else {
      this.addChannelToPublishingList(channel);
     
    }
  }

  /**
 * Add channel to publishing list
 *
 * @param channel
 */
  addChannelToPublishingList(channel: SocialChannel): void {
    // Add the channel
    this.channelList_Selected.unshift(channel);

    // Mark for check
    this._changeDetectorRef.markForCheck();
  }

  /**
   * Remove channel from publishing list
   *
   * @param channel
   */
  removeChannelFromPublishing(channel: SocialChannel): void {
    // Remove the channel
    this.channelList_Selected.splice(this.channelList_Selected.findIndex(item => item.id === channel.id), 1);

    // Mark for check
    this._changeDetectorRef.markForCheck();
  }

  /**
   * Is the given note has the given label
   *
   * @param note
   * @param label
   */
  isChannelSelected(channel: SocialChannel): boolean {
    console.log(channel.channelType.name + "socialChannels")
    return !!this.channelList_Selected.find(item => item.id === channel.id);
  }

  /**
  * Create a new post
  *
  * @param post
  */
  publishPost(post: GenericPublishingPosts): void {
    post.socialChannels = this.channelList_Selected;
    post.message=this.message
    if(this.schedule){
      post.scheduleTime=post.publicationDate=this.formatDateWithTimezone(this.dateControl.value)
    }
  //  console.log( post)
  //   this._changeDetectorRef.detectChanges();

  post.socialChannels.forEach((element) => {
    // Check for LinkedIn Profile, Facebook Page, Facebook Group, LinkedIn Page
    
    if (
      element.channelType.name=="LinkedIn Profile" ||
      element.channelType.name=="Facebook Page" ||
      element.channelType.name=="Facebook Group" ||
      element.channelType.name=="LinkedIn Page"
    ) {
      console.log( element.channelType.name+"post.socialChannels")
      if (this.videoCount > 1) {
        // Display an error message for multiple videos
        this.errorMessages.push('Only one video is allowed for ' +  element.channelType.name);
        this.toastr.error('Only one video is allowed for ' +  element.channelType.name, 'SNM');
      }
  
      if (this.imageCount > 0 && this.videoCount > 0) {
        // Display an error message for having both images and videos
        this.toastr.error('You can only upload images or videos, not both, for ' +  element.channelType.name, 'SNM');
        this.errorMessages.push('You can only upload images or videos, not both, for ' +  element.channelType.name);
      }
    }
  
    // Check for Instagram Profile
    if (element.displayName=="Instagram Profile") {
      if (this.post.mediaData.length === 0) {
        // Display an error message for no videos or images
        this.toastr.error("We don't publish posts on Instagram without Video or Image.", 'SNM');
        this.errorMessages.push("We don't publish posts on Instagram without Video or Image.");
      }
    }
  });

  if(this.errorMessages.length==0 && post.socialChannels.length>0){

    this._postsService.publishPost(post).subscribe({
     next: (res:any) => {
       if(res.succeeded){
        res.data.forEach(element => {
          this.toastr.success(element,'SNM')
          this.dialogRef.close();
        });
       console.log(JSON.stringify( res.data))
      
      
       }

     }
   });
 }  
 else{
   this.errorMessages=[]

 }

    

  }

  

  /**
    * Upload image/videao to given post
    *
    * @param post
    * @param fileList
    */
  uploadMedia(post: GenericPublishingPosts, fileList: FileList): void {
   
    // Return if canceled
    if (!fileList.length) {
      return;
    }

    const allowedTypes = ['image/jpeg', 'image/png', 'video/mp4', 'video/x-m4v', 'video/*'];
    for (let i = 0; i < fileList.length; i++) {
      const file = fileList[i];
      // Return if the file is not allowed
      if (!allowedTypes.includes(file.type)) {
        return;
      }
      console.log(file.type+"file.type")
      if (file.type.includes("video") || file.type.includes("video/mp4")) {
        this.videoCount++;
        console.log(this.videoCount)
      } else {
        this.imageCount++;
        console.log( this.imageCount)
      }
      this._readAsDataURL(file).then((data) => {
        const mediaType = file.type.includes("video") || file.type.includes("video/mp4") ? MediaTypeEnum.VIDEO : MediaTypeEnum.IMAGE;
       
        // Add the image or video object to the post
        post?.mediaData.push({
          media_url: data,
          media_type: mediaType,
        });
       
       // this.errorMessages=[]
        // Update the post
        this.postChanged.next(post);
         this._changeDetectorRef.detectChanges();
      });
    }
     this.post.mediaData=post?.mediaData

  }
  deleteMedia(index: number): void {
    this.post.mediaData.splice(index, 1);
    if ( this.post.mediaData[index].media_type==1) {
      this.videoCount--;
      console.log(this.videoCount)
    } else {
      this.imageCount--;
      console.log( this.imageCount)
    }
  }
  
uploadImageWithchannel(){ }
  // -----------------------------------------------------------------------------------------------------
  // @ Private methods
  // -----------------------------------------------------------------------------------------------------
  isVideo(val): boolean {
    return val.toString() === 'video/mp4' || val.toString() === 'video/*' || val.toString() === "1";
  }


  private _readAsDataURL(file: File): Promise<string> {
    // Return a new promise
    return new Promise((resolve, reject) => {
      // Create a new reader
      const reader = new FileReader();

      // Resolve the promise on success
      reader.onload = (event) => {
        const dataURL = event.target.result as string;
        const bytes = this._getBytesInDataURL(dataURL);

        if (bytes < 8388608) {
          resolve(dataURL);
        } else {
          // this.errorMessages.push("File size exceeds the limit of 8,388,608 bytes.")
          // console.log("File size exceeds the limit of 8,388,608 bytes.");
          this.toastr.error("File size exceeds the limit of 8,388,608 bytes.", "SNM")
        }
      };

      // Reject the promise on error
      reader.onerror = (e) => {
        reject(e);
      };

      // Read the file as data URL
      reader.readAsDataURL(file);
    });
  }

  private _getBytesInDataURL(dataURL: string): number {
    const base64 = dataURL.split(',')[1];
    const binary = atob(base64);
    return binary.length;
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


  /** date time*/
  @ViewChild('picker') picker: any;
  public date: moment.Moment;
  public disabled = false;
  public showSpinners = true;
  public showSeconds = false;
  public touchUi = false;
  public enableMeridian = false;
  public minDate: moment.Moment;
  public maxDate: moment.Moment;
  public stepHour = 1;
  public stepMinute = 1;
  public stepSecond = 1;
  public color: ThemePalette = 'primary';
  toggleMinDate(evt: any) {
    if (evt.checked) {
      this._setMinDate();
    } else {
      this.minDate = null;
    }
  }

  toggleMaxDate(evt: any) {
    if (evt.checked) {
      this._setMaxDate();
    } else {
      this.maxDate = null;
    }
  }

  closePicker() {
    this.picker.cancel();
  }

  private _setMinDate() {
    const now = new Date();
    this.minDate = moment(new Date());
    // this.minDate.setDate(now.getDate() - 1);
  }


  private _setMaxDate() {
    const now = new Date();
    this.maxDate = moment(new Date());
    // this.maxDate.setDate(now.getDate() + 1);
  }

  dateControl: FormControl = new FormControl();
  publishDate: Date;

  radioButtonChange(data: MatRadioChange) {

    data.value == "Schedule" ? this.schedule = true : this.schedule = false;

  }


  formatDateWithTimezone(selectedDate: Date): Date | null {
    if (selectedDate) {
      // Create a new Date object with the same timestamp as the selectedDate
      const localDate = new Date(selectedDate);
  
      // Get the local time zone offset in minutes
      const timeZoneOffset = localDate.getTimezoneOffset();
  
      // Adjust the time to the local time zone
      localDate.setMinutes(localDate.getMinutes() - timeZoneOffset);
  
      return localDate;
    }
    return null;
  }
  schedulePublication(post: any) {

    // if (this.schedule) {

    //   const scheduledDate: Date = this.dateControl.value;

    //   // Opérations pour planifier la publication
    //   console.log('Publication scheduled for:', scheduledDate);

    //   // Enregistrer la date de publication
    //   this.publishDate = scheduledDate;

    //   // Comparer la date actuelle avec la date de publication
    //   const currentDate: Date = new Date();

    //   if (currentDate >= scheduledDate) {
    //     // La date actuelle est supérieure ou égale à la date de publication
    //     this.publish(post);
    //   } else {
    //     // La date actuelle est antérieure à la date de publication
    //     console.log('Publication will be scheduled for a future date.');
    //   }
    // } else {
    //   // Si la planification n'est pas activée, exécuter simplement la publication immédiate
    //   this.publish(post);
    // }
  }
  //endregion
}

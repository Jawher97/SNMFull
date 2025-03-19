// import { AfterViewChecked, AfterViewInit, ChangeDetectionStrategy, ChangeDetectorRef, Component, Inject, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
// import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
// import { Label, Post, Task } from 'app/core/models/post.model';
// import { PostsService } from 'app/core/services-api/posts.service';
// import { UplodeFileService } from 'app/core/services-api/uplode-file.service';
// import { debounceTime, map, Observable, of, Subject, switchMap, takeUntil } from 'rxjs';
// import * as moment from 'moment';
// import { ThemePalette } from '@angular/material/core';
// import { MatRadioChange } from '@angular/material/radio';
// import { IlinkPreview } from 'app/core/types/IlinkPreview';
// import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
// import { HttpClient } from '@angular/common/http';
// import { FacebookService } from 'app/core/services-api/facebook.service';
// import { LinkedInService } from 'app/core/services-api/linkedin.service';
// import { LinkedInChannelDto } from 'app/core/models/brand.model';
// import { TwitterService } from 'app/core/services-api/twitter.service';
// import { SocialChannel } from 'app/core/models/socialChannel.model';
// @Component({
//   selector: 'app-details',
//   templateUrl: './details.component.html',
//   styleUrls: ['./details.component.scss'],
//   encapsulation: ViewEncapsulation.None,
//   changeDetection: ChangeDetectionStrategy.OnPush
// })
// export class PostsDetailsComponent implements OnInit, OnDestroy {
  
//   postForm: FormGroup;

//   post: Post = {
//     message: '',
//     createdBy: '',
//     lastModifiedBy: '',
//     lastModifiedOn: '',
//     deletedOn: '',
//     deletedBy: '',
//     id: '',
//     photo: undefined,
//     icon: '',
//     link: '',
//     useRouter: false,
//     lastActivity: '',
//     socialChannels: []
//   };
  
//   showEmojiPicker = false;

//   socialChannels: SocialChannel[] = [];
  

//   post$: Observable<Post>;
//   // labels$: Observable<Label[]>;
//   preview: IlinkPreview;
//   // Regular Expression for validating the link the user enters
//   private regExHyperlink = /^(?:http(s)?:\/\/)?[\w.-]+(?:\.[\w\.-]+)+[\w\-\._~:/?#[\]@!\$&'\(\)\*\+,;=.]+$/;
//   link = new FormControl('', [Validators.required, Validators.pattern(this.regExHyperlink)]);
//   postChanged: Subject<Post> = new Subject<Post>();
//   private _unsubscribeAll: Subject<any> = new Subject<any>();

//   schedule: boolean = false;

//   selectedVal: string[] = [];

//   allowedImgTypes = ['image/jpeg', 'image/png'];
//   allowedVideoTypes = ['video/mp4', 'video/*'];

  

//   /**
//    * Constructor
//    */
//   constructor(private http: HttpClient,
//     private _changeDetectorRef: ChangeDetectorRef,
//     @Inject(MAT_DIALOG_DATA) private _data: { post: Post },
//     private _postsService: PostsService,
//     private formBuilder: FormBuilder,
//     private _facebookService: FacebookService,
//     private _linkedinService: LinkedInService,
//     private _twitterService: TwitterService,
//     private _uplodeService: UplodeFileService,
//     private _matDialogRef: MatDialogRef<PostsDetailsComponent>
//   ) {
//   }

//   // ngAfterViewInit(): void {
//   //     this._changeDetectorRef.detectChanges(); 
//   //   }
//   // -----------------------------------------------------------------------------------------------------
//   // @ Lifecycle hooks
//   // -----------------------------------------------------------------------------------------------------

//   /**
//    * On init
//    */
//   ngOnInit(): void {
//     this.getCompanyPages();
//     // Edit
//     if (this._data.post.id) {
//       // Request the data from the server
//       this._postsService.getPostById(this._data.post.id).subscribe();

//       // Get the post
//       this.post$ = this._postsService.post$;
//     }
//     // Add
//     else {
//       // Create an empty post
//       const post = {
//         id: null,
//         title: '',
//         postContent: '',
//         message: '',
//         tasks: null,
//         images: [],
//         reminder: null,
//         labels: [],
//         socialChannels: [],
//         archived: false,
//         createdAt: null,
//         updatedAt: null,
//         description: null,
//         photo: null,
//         coverPhoto: null,
//         // icon: null,
//         // link: null,
//         useRouter: null,
//         lastActivity: null,
//         createdOn:"2023-04-17T08:34:47.224Z",
//         createdBy: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//       lastModifiedBy: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//       lastModifiedOn: "2023-04-17T08:34:47.224Z",
//       deletedOn: "2023-04-17T08:34:47.224Z",
//       deletedBy: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//       icon: "string",
//       link: "www.consultim-it.com",

//       };

//       this.post$ = of(post);

//     }

//     // Get the labels
//     // this.labels$ = this._postsService.labels$;

//     // Subscribe to post updates
//     this.postChanged
//       .pipe(
//         takeUntil(this._unsubscribeAll),
//         debounceTime(500),
//         switchMap(post => this._postsService.updatePost(post)))
//       .subscribe(() => {

//         // Mark for check
//         this._changeDetectorRef.markForCheck();
//       });
//   }

//   /**
//    * On destroy
//    */
//   ngOnDestroy(): void {
//     // Unsubscribe from all subscriptions
//     this._unsubscribeAll.next(null);
//     this._unsubscribeAll.complete();
//   }

//   // -----------------------------------------------------------------------------------------------------
//   // @ Public methods
//   // -----------------------------------------------------------------------------------------------------
//   getLinkPreview(link: string): Observable<any> {
//     const api = 'https://api.linkpreview.net/?key=a3ddaeb87cb004e786427f429f6bfe5c&q=' + link;
//     return this.http.get(api);
//   }

//   onPreview() {
//     this.getLinkPreview(this.link.value)
//       .subscribe(preview => {
//         this.preview = preview;

//         if (!this.preview.title) {
//           this.preview.title = this.preview.url;
//         }

//       }, error => {
//         this.preview.url = this.link.value;
//         this.preview.title = this.preview.url;
//       });
//   }

//   public onValChange(val: string) {
//     this.selectedVal.push(val);
//     console.log(val);

//   }

//   selectedPage: LinkedInChannelDto
   
//   onSelectPage(page: LinkedInChannelDto): void {
//     this.selectedPage = page;
//   }


  

//   public addEmoji(event) {
//     console.log(this.post.message);
//     const text = `${this.post.message}${event.emoji.native}`;

//    console.log(text)
//     this.post.message = text;
//   }



//   publish(post: any): void {
    
//     // Effectuer la publication en fonction des canaux sociaux sélectionnés
//     for (const channel of this.selectedVal) {
//       // Utiliser la méthode appropriée pour effectuer la publication pour chaque canal
//       if (channel.indexOf('facebook') != -1) {
//         // Effectuer la publication sur Facebook

//       } 
//       else if (channel.indexOf('instagram') != -1) {
//         // Effectuer la publication sur Instagram

//       } 
//       else if (channel.indexOf("twitter") != -1) {
        
//         // Effectuer la publication sur Twitter
//         // Appeler la méthode appropriée pour publier sur Twitter
//         this.publishOnTwitter(post);
//       }
//       else if (channel.indexOf("linkedin-profile") != -1) {
          
//           // Effectuer la publication sur Twitter
//           // Appeler la méthode appropriée pour publier sur Twitter
//           const author_urn = localStorage.getItem('person')
//           this.publishToLinkedIn(post);

//       } else if (channel.indexOf('linkedin')!= -1){
//         //   // Effectuer la publication sur LinkedIn
//         console.log(this.selectedPage.companyUrn)
//           this.publishOnLinkedIn(post)  

//      }
//    }

//     // Réinitialiser les canaux sociaux sélectionnés après la publication
//     this.socialChannels = [];
//     this.selectedVal = [];
//   }


//   publishOnTwitter(post: any): void {

//     this._twitterService.publishToTwitter(post).subscribe(
//       response => {
//         // Gérer la réponse de la publication sur Twitter
//         console.log(response);
//       },
//       error => {
//         // Gérer les erreurs lors de la publication sur Twitter
//         console.error(error);
//       }
//     );
//   ;
//   }


//   publishOnLinkedIn(post: any): void {
  
//     this._linkedinService.publishLinkedInPost(post).subscribe(
//       response => {
//         // Gérer la réponse de la publication sur Twitter
//         console.log(response);
//       },
//       error => {
//         // Gérer les erreurs lors de la publication sur Twitter
//         console.error(error);
//       }
//     );
//   ;
//   }

//   publishToLinkedIn(post: any): void {

//     this._linkedinService.publishLinkedInProfilePost(post).subscribe(
//       response => {
//         // Gérer la réponse de la publication sur Twitter
//         console.log(response);
//       },
//       error => {
//         // Gérer les erreurs lors de la publication sur Twitter
//         console.error(error);
//       }
//     );
//   ;
//   }


//   pages: LinkedInChannelDto[];

//   /**
//    * LinkedIn Company Pages
//    */
//   // getCompanyPages()
//   // {
//   //   this._linkedinService.getCompanyPages().subscribe(
//   //     (pages: LinkedInChannelDto[]) => {
//   //       this.pages = pages;
//   //       // Mark for check
//   //      this._changeDetectorRef.markForCheck()});
//   // }

//   /**
//    * Create a new post
//    *
//    * @param post
//    */
//   createPost(post: Post): void {
//     const p = {
//       createdBy: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//       lastModifiedBy: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//       lastModifiedOn: "2023-04-17T08:34:47.224Z",
//       deletedOn: "2023-04-17T08:34:47.224Z",
//       deletedBy: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//       icon: "string",
//       link: "www.consultim-it.com",
//       message: "test"
//     }

//     this._postsService.createPost(p).pipe(
//       map(() => {
//         // Get the post
//         this.post$ = this._postsService.post$;
//       })).subscribe();
//     //   this._postsService.createPost(post).pipe(
//     // map(() => {
//     //     // Get the post
//     //     this.post$ = this._facebookService.post$;
//     // })).subscribe();

//   }
//   /**
//   * Create a new post
//   *
//   * @param post
//   */

//   publishPost(post: Post): void {
//     // let res = post.message.match(/https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|www\.[a-zA-Z0-9][a-zA-Z0-9-]+[a-zA-Z0-9]\.[^\s]{2,}|https?:\/\/(?:www\.|(?!www))[a-zA-Z0-9]+\.[^\s]{2,}|www\.[a-zA-Z0-9]+\.[^\s]{2,}/igm);

//     // console.log(res);
//     // const posttopublish = {
//     //   id: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//     //   facebookPostId: "string",
//     //   createdTime: "2023-03-31T10:13:14.545Z",
//     //   formatting: 0,
//     //   icon: "string",
//     //   link: res != null ? res[0].toString() : "",
//     //   message: post.message,
//     //   name: "string",
//     //   objectId: "string",
//     //   permalinkUrl: "string",
//     //   picture: "https://consultim-it.com/wp-content/uploads/2022/10/Logo-New-.png",
//     //   photos: [
//     //     "https://consultim-it.com/wp-content/uploads/2022/02/11.png",
//     //     "https://consultim-it.com/wp-content/uploads/2022/02/12.png",
//     //     "https://consultim-it.com/wp-content/uploads/2022/02/10.png",
//     //   ],
//     //   properties: [
//     //     "string"
//     //   ],
//     //   statusType: 0,
//     //   story: "string",
//     //   type: 0,
//     //   updatedTime: "2023-03-31T10:13:14.545Z",
//     //   facebookChannelId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//     //   facebookChannelDto: {
//     //     displayName: "Consultim-IT",
//     //     channelId: "101277042876317",
//     //     channelType: "Page",
//     //     channelAPI: "https://graph.facebook.com/",
//     //     userAccessToken: "EAAQZAIG8xV0EBAJYvVT90nWDPJlatsrGxEEBnK0kOjYvlyy5zNXqw5JEQVmhj5tTyd9M7AawqmiLZB2azgQmaZBpmnZCmuU4vMNsLdyTlBntNUbOrGKBfubJ4RqxEN7inPZBr8cAgw7hfXKi1uTkg3oDoZBDJRFHZCbITkuzPw0rAZDZD",
//     //     channelAccessToken: "EAAQZAIG8xV0EBAISMlqzFVQwm54aUVjf1cGZCsBT0bqzdqgY2X5wiZBNjWwsVCQSF3KAMBN5B89lgRBT0qWODUTJYOf3PNjTwJ1pZCBZBN8opWy17RKrZCwF3JhrotxdXZCtqZALrZA9oDBwY6QBx1TiJh2vivZAKDDMfcmslQp6StcmPIubYCHl9U",
//     //     pageEdgeFeed: "Feed",
//     //     pageEdgePhotos: "photos",
//     //     postToPageURL: "string",
//     //     postToPagePhotosURL: "string",
//     //     photo: "string",
//     //     brandId: "08daf882-b1e6-4bf2-86ec-e1faaf4829c7",



//     //   },

//     //   postId: "3fa85f64-5717-4562-b3fc-2c963f66afa6",
//     //   post: {}
//     // }


//     // this._facebookService.publish(posttopublish).subscribe();
//   }






//   /**
//    * Upload image to given post
//    *
//    * @param post
//    * @param fileList
//    */
//   uploadImages = (files) => {
//     if (files.length === 0) {
//       return;
//     }

//     let filesToUpload: File[] = files;
//     const formData = new FormData();

//     Array.from(filesToUpload).map((file, index) => {
//       return formData.append('photo', file, file.name);
//     });
//   }
//   /**
//    * Upload image to given post
//    *
//    * @param post
//    * @param fileList
//    */
//   uploadImage(post: Post, fileList: FileList): void {
//     // Return if canceled
//     if (!fileList.length) {
//       return;
//     }

//     const allowedTypes = ['image/jpeg', 'image/png', 'video/mp4'];
//     for (let i = 0; i < fileList.length; i++) {

//       const file = fileList[i];
//       // Return if the file is not allowed
//       if (!allowedTypes.includes(file.type)) {
//         return;
//       }
//       this._readAsDataURL(file).then((data) => {
//         const image = {
//           id: '630d2e9a-d110-47a0-ac03-256073a0f56d',
//           folderId: null,
//           name: 'Scanned image 20201012-2',
//           createdBy: 'Brian Hughes',
//           createdAt: 'September 14, 2020',
//           modifiedAt: 'September 14, 2020',
//           size: '7.4 MB',
//           type: file.type,
//           src: data,
//           contents: null,
//           description: null
//         }
      
//         // Update the image
//         post?.images.push(image);

//         // Update the post
//         this.postChanged.next(post);
//       });
//     }


//   }

//   /**
//    * Remove the image on the given post
//    *
//    * @param post
//    */
//   removeImage(post: Post): void {
//     // post.image = null;

//     // Update the post
//     this.postChanged.next(post);
//   }

//   /**
//    * Add an empty tasks array to post
//    *
//    * @param post
//    */
//   addTasksToPost(post): void {
//     if (!post.tasks) {
//       post.tasks = [];
//     }
//   }

//   /**
//    * Add task to the given post
//    *
//    * @param post
//    * @param task
//    */
//   addTaskToPost(post: Post, task: string): void {
//     if (task.trim() === '') {
//       return;
//     }

//     // Add the task
//     this._postsService.addTask(post, task).subscribe();
//   }

//   /**
//    * Remove the given task from given post
//    *
//    * @param post
//    * @param task
//    */
//   removeTaskFromPost(post: Post, task: Task): void {
//     // Remove the task
//     // post.tasks = post.tasks.filter(item => item.id !== task.id);

//     // Update the post
//     this.postChanged.next(post);
//   }

//   /**
//    * Update the given task on the given post
//    *
//    * @param post
//    * @param task
//    */
//   updateTaskOnPost(post: Post, task: Task): void {
//     // If the task is already available on the item
//     if (task.id) {
//       // Update the post
//       this.postChanged.next(post);
//     }
//   }

//   /**
//    * Is the given post has the given label
//    *
//    * @param post
//    * @param label
//    */
//   // isPostHasLabel(post: Post, label: Label): boolean {
//   //   return !!post.labels.find(item => item.id === label.id);
//   // }

//   /**
//    * Toggle the given label on the given post
//    *
//    * @param post
//    * @param label
//    */
//   // toggleLabelOnPost(post: Post, label: Label): void {
//   //   // If the post already has the label
//   //   if (this.isPostHasLabel(post, label)) {
//   //     // post.labels = post.labels.filter(item => item.id !== label.id);
//   //   }
//   //   // Otherwise
//   //   else {
//   //     // post.labels.push(label);
//   //   }

//   //   // Update the post
//   //   this.postChanged.next(post);
//   // }

//   /**
//    * Toggle archived status on the given post
//    *
//    * @param post
//    */
//   toggleArchiveOnPost(post: Post): void {
//     // post.archived = !post.archived;

//     // Update the post
//     this.postChanged.next(post);

//     // Close the dialog
//     this._matDialogRef.close();
//   }

//   /**
//    * Update the post details
//    *
//    * @param post
//    */
//   updatePostDetails(post: Post): void {
//     this.postChanged.next(post);
//   }

//   /**
//    * Delete the given post
//    *
//    * @param post
//    */
//   deletePost(post: Post): void {
//     this._postsService.deletePost(post)
//       .subscribe((isDeleted) => {

//         // Return if the post wasn't deleted...
//         if (!isDeleted) {
//           return;
//         }

//         // Close the dialog
//         this._matDialogRef.close();
//       });
//   }

//   /**
//    * Track by function for ngFor loops
//    *
//    * @param index
//    * @param item
//    */
//   trackByFn(index: number, item: any): any {
//     return item.id || index;
//   }

//   // -----------------------------------------------------------------------------------------------------
//   // @ Private methods
//   // -----------------------------------------------------------------------------------------------------
//   isVideo(val): boolean { return val.toString() === 'video/mp4' || val.toString() === 'video/*'; }
//   /**
//    * Read the given file for demonstration purposes
//    *
//    * @param file
//    */
//   private _readAsDataURL(file: File): Promise<any> {
//     // Return a new promise
//     return new Promise((resolve, reject) => {

//       // Create a new reader
//       const reader = new FileReader();

//       // Resolve the promise on success
//       reader.onload = (): void => {
//         resolve(reader.result);
//       };

//       // Reject the promise on error
//       reader.onerror = (e): void => {
//         reject(e);
//       };

//       // Read the file as the
//       reader.readAsDataURL(file);
//     });
//   }


//   selectedImageFile: File;

//   onPhotoSelected(photoSelector: HTMLInputElement) {
//     this.selectedImageFile = photoSelector.files[0];
//     if (!this.selectedImageFile) return;
//     let fileReader = new FileReader();
//     fileReader.readAsDataURL(this.selectedImageFile);
//     fileReader.addEventListener(
//       "loadend",
//       ev => {
//         let readableString = fileReader.result.toString();
//         let postPreviewImage = <HTMLImageElement>document.getElementById("post-preview-image");
//         postPreviewImage.src = readableString;
//       }
//     );
//   }

//   /** date time*/
//   @ViewChild('picker') picker: any;
//   public date: moment.Moment;
//   public disabled = false;
//   public showSpinners = true;
//   public showSeconds = false;
//   public touchUi = false;
//   public enableMeridian = false;
//   public minDate: moment.Moment;
//   public maxDate: moment.Moment;
//   public stepHour = 1;
//   public stepMinute = 1;
//   public stepSecond = 1;
//   public color: ThemePalette = 'primary';
//   toggleMinDate(evt: any) {
//     if (evt.checked) {
//       this._setMinDate();
//     } else {
//       this.minDate = null;
//     }
//   }

//   toggleMaxDate(evt: any) {
//     if (evt.checked) {
//       this._setMaxDate();
//     } else {
//       this.maxDate = null;
//     }
//   }

//   closePicker() {
//     this.picker.cancel();
//   }

//   private _setMinDate() {
//     const now = new Date();
//     this.minDate = moment(new Date());
//     // this.minDate.setDate(now.getDate() - 1);
//   }


//   private _setMaxDate() {
//     const now = new Date();
//     this.maxDate = moment(new Date());
//     // this.maxDate.setDate(now.getDate() + 1);
//   }

//   dateControl: FormControl = new FormControl();
//   publishDate: Date;

//   radioButtonChange(data: MatRadioChange) {
//     console.log(data.value);
//     data.value == "Schedule" ? this.schedule = true : this.schedule = false;

//   }

//   schedulePublication(post: any) {

//     if (this.schedule) {

//       const scheduledDate: Date = this.dateControl.value;

//       // Opérations pour planifier la publication
//       console.log('Publication scheduled for:', scheduledDate);

//       // Enregistrer la date de publication
//       this.publishDate = scheduledDate;

//       // Comparer la date actuelle avec la date de publication
//       const currentDate: Date = new Date();

//       if (currentDate >= scheduledDate) {
//         // La date actuelle est supérieure ou égale à la date de publication
//         this.publish(post);
//       } else {
//         // La date actuelle est antérieure à la date de publication
//         console.log('Publication will be scheduled for a future date.');
//       }
//     } else {
//       // Si la planification n'est pas activée, exécuter simplement la publication immédiate
//       this.publish(post);
//     }
//   }
// }

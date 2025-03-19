import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChildren, ViewEncapsulation,ViewChild } from '@angular/core';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { MatDialog } from '@angular/material/dialog';
import { FuseCardComponent } from '@fuse/components/card';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { Post } from 'app/core/models/post.model';
import { PostsService } from 'app/core/services-api/posts.service';
import { cloneDeep } from 'lodash';
import { BehaviorSubject, combineLatest, distinctUntilChanged, map, Observable, Subject, takeUntil } from 'rxjs';
// import { PostsDetailsComponent } from '../details/details.component';
import { CommentsComponent } from '../comments/comments.component';
import { LinkedinPost } from 'app/core/models/linkedin-post';
import { CreatePostsComponent } from '../create-posts/create-posts.component';
import { ToastrService } from 'ngx-toastr';
import { environment } from 'environments/environment.development';
import { ChannelTypeService } from 'app/core/services-api/channelType.service';
import { ChannelType } from 'app/core/models/channelType.model';
import { BrandsService } from 'app/core/services-api/brands.service';
import { Brand } from 'app/core/models/brand.model';
import { ActivatedRoute, Router } from '@angular/router';
import { MatDrawer } from '@angular/material/sidenav';

import { SocialChannelService } from 'app/core/services-api/socialChannel.service';
import { PostDetailsComponent } from '../post-details/post-details.component';
import { FacebookService } from 'app/core/services-api';
import { InstagramService } from 'app/core/services-api/instagram.service';
import { LinkedInService } from 'app/core/services-api/linkedin.service';
import { PostService } from 'app/core/services-api/post.service';
import { FuseConfirmationService } from '@fuse/services/confirmation';
@Component({
  selector: 'posts-list',
  templateUrl: './list.component.html',
  styleUrls: ['./list.component.scss'],
  encapsulation  : ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class PostsListComponent implements OnInit, OnDestroy
{
    @ViewChildren(FuseCardComponent, {read: ElementRef}) private _fuseCards: QueryList<ElementRef>;
    @ViewChild('matDrawer', {static: true}) matDrawer: MatDrawer;
   
    // labels$: Observable<Label[]>;
    posts$: Observable<Post[]>;
    linkedinposts$: Observable<LinkedinPost[]>
    filters: string[] = ['all','linkedin', 'facebook', 'instagram', 'twitter'];
    numberOfCards: any = {};
    selectedFilter: string = 'all';
    channelTypeList:ChannelType[];
    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    filter$: BehaviorSubject<string> = new BehaviorSubject('posts');
    searchQuery$: BehaviorSubject<string> = new BehaviorSubject(null);
    masonryColumns: number = 4;
    currentBrand:Brand;
    post:any
    private postsData: BehaviorSubject<any[]> = new BehaviorSubject<any[]>([]);
    private _unsubscribeAll: Subject<any> = new Subject<any>();
    publishingAgrgregator_Path = environment.publishingAgrgregatorURL;
    Posts$: Observable<any[]> = this.postsData.asObservable();
    updatedPosts:any[]=[];
    selectedPost?:any;
    isDrawerOpen = false;
    /**
     * Constructor
     */
    constructor(  
        private _changeDetectorRef: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _matDialog: MatDialog,
        private _postsService: PostsService,private toastr: ToastrService,   
        private _router: Router,
        private _fuseConfirmationService: FuseConfirmationService, 
        private _activatedRoute: ActivatedRoute,
        private _socialChannelService:SocialChannelService,
        private _facebookService:FacebookService,
        private _instgramService:InstagramService,
        private _linkedinService:LinkedInService, 
        private postService:PostService,
        private _brandsService: BrandsService,
        )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

 
    ngOnInit(): void
    {
        // this._fuseMediaWatcherService.onMediaQueryChange$('(min-width: 1440px)')
        // .pipe(takeUntil(this._unsubscribeAll))
        // .subscribe((state) => {

        //     // Calculate the drawer mode
        //     this.drawerMode = state.matches ? 'side' : 'over';

        //     // Mark for check
        //     this._changeDetectorRef.markForCheck();
        // });
   // this.Posts$.subscribe((value) => {
        //     console.log('Emitted value:', value);
        //     this.updatedPosts.push(value) ;
        //     this._changeDetectorRef.detectChanges();
        //   });
        
        // this._fuseMediaWatcherService.onMediaQueryChange$('(min-width: 1440px)')
        // .pipe(takeUntil(this._unsubscribeAll))
        // .subscribe((state) => {

        //     // Calculate the drawer mode
        //     this.drawerMode = state.matches ? 'side' : 'over';

        //     // Mark for check
        //     this._changeDetectorRef.markForCheck();
        // });

        this._brandsService.brand$
        .pipe(takeUntil(this._unsubscribeAll))
        .subscribe((brand: Brand) => {
          // Get the brandName
          this.currentBrand = brand; 
          // Mark for check
          this._changeDetectorRef.markForCheck();
        });
        console.log(this.currentBrand?.id+"this.currentBrand?.id")
   
        this._socialChannelService.getSocialChannels(this.currentBrand?.id).subscribe(channels=>{
  

        channels.forEach((socialChannel) => {
        switch (socialChannel?.channelType?.name) {
            case 'Facebook Group':
            case 'Facebook Page':
            this._facebookService.getLatestFacebookPost(socialChannel.id).subscribe((post)=>{
                if(post.succeeded){
                    post.data.channelTypeName = "Facebook";
                    post.data.photo = socialChannel.photo;
                    post.data.channelTypephoto=socialChannel?.channelType.icon
                    post.data.name = socialChannel.displayName;
                    post.data.channelId = socialChannel.id;               
                    this.updatedPosts.push(post.data)           
                    this.postsData.next([...this.updatedPosts]);
                    console.log(socialChannel?.channelType.icon+"socialChannel?.channelType.icon")
                    this._changeDetectorRef.detectChanges();
                }
            })
            break;

            case 'Instagram Profile':
                this._instgramService.getLatestInstagramPost(socialChannel.id).subscribe((post)=>{
                    if(post.succeeded){
                        post.data.channelTypeName = "Instagram Profile";
                        post.data.photo = socialChannel.photo;
                        post.data.channelTypephoto=socialChannel?.channelType.icon
                        post.data.name = socialChannel.displayName;
                        post.data.channelId = socialChannel.id;
                        this.updatedPosts.push(post.data)
                        this.postsData.next([...this.updatedPosts]);
                        this._changeDetectorRef.detectChanges();
                    }
                })
            break;

            case 'LinkedIn Page':
                this._linkedinService.getLatestLinkedInPost(socialChannel.id).subscribe((post)=>{
                    if(post.succeeded){
                        post.data.channelTypeName = "LinkedIn";
                        post.data.channelTypephoto=socialChannel?.channelType.icon
                        post.data.photo = socialChannel.photo;
                        post.data.name = socialChannel.displayName;
                        post.data.channelId = socialChannel.id;
                        this.updatedPosts.push(post.data) 
                        this.postsData.next([...this.updatedPosts]);
                        this._changeDetectorRef.detectChanges();
                    }
                })
            break;
        }
        });


    })

        // this._socialChannelService.socialChannels$
        // .pipe(takeUntil(this._unsubscribeAll))
        // .subscribe((channels: any) => {
          // Get the brandName
          //this.currentBrand = brand;
         
        //   this._socialChannelService.getSocialChannels("08db4587-5d6c-47a1-8dd1-8a963a5bd8d0").subscribe(channels=>{
           
        //         console.log(JSON.stringify(channels)+"channelTypeList")
                
                   
        //             this._postsService.getAllPosts(channels).subscribe(post=>{
        //                 console.log(JSON.stringify(post)+"poooost")
        //                 if(post.succeeded){
        //                     this.updatedPosts=post.data
                           
        //                     this._changeDetectorRef.detectChanges();
        //                      this.postsData.next(this.updatedPosts);
                           
        //                 }

        //                 console.log(JSON.stringify(post.data)+"this.Posts")
                        
        //               });
        //             })
             
            
         
          // Mark for check
          
       
        
          
            
            
           
         
          
        
    }
    DeletePost(selectedpost:any ){
        const confirmation = this._fuseConfirmationService.open({
            title  : 'Delete Post',
            message: 'Are you sure you want to delete this Post? This action cannot be undone!',
            actions: {
                confirm: {
                    label: 'Delete'
                }
            }
        });
        confirmation.afterClosed().subscribe((result) => {

            // If the confirm button pressed...
            if ( result === 'confirmed' )
            {
        this.postService.DeletePost(selectedpost).subscribe({
              next:(resp)=>{
                if(resp.succeeded){
                    const index = this.updatedPosts.findIndex(post => post.postIdAPI === selectedpost.postIdAPI);

                    // If the post is found, delete it from the array
                    if (index !== -1) {
                        this.updatedPosts.splice(index, 1);
                        this.postsData.next([...this.updatedPosts]);
                        this.toastr.success(resp.message, 'SNM');
                    } else {
                        this.toastr.error('Post not found in the array.', 'SNM');
                    }
                }else{
                  this.toastr.success(resp.message, 'SNM');
                }
                
    
              }})
            }
        })
      }
    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }
    openDrawer(post: any): void {
  
        this.selectedPost = post;
        this.isDrawerOpen = true;
        console.log(this.isDrawerOpen)
        // Open the drawer
        this.matDrawer.open();
        this._changeDetectorRef.detectChanges();
      }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
/**
     * On filter change
     *
     * @param change
     */
  
    /**
     * Add a new post
     */
    addNewPost(): void
    {
       let dialogRef= this._matDialog.open(CreatePostsComponent, {
            autoFocus: false,
            data     : {
                post: {}
            }
        });
    
    dialogRef.afterClosed().subscribe(result => {
        if(result)
        { 
            this.toastr.success('Post successfully published','SNM')
        }
      });
    }
  

    /**
     * Open the post dialog
     */
    openPostDialog(post: Post): void
    {
        this._matDialog.open(PostDetailsComponent, {
            autoFocus: false,
            data     : {
                post: cloneDeep(post)
            }
        });
    }
   
    /**
     * Filter by archived
     */
    filterByArchived(): void
    {
        this.filter$.next('archived');
    }

    /**
     * Filter by label
     *
     * @param labelId
     */
    filterByLabel(labelId: string): void
    {
        const filterValue = `label:${labelId}`;
        this.filter$.next(filterValue);
    }

    /**
     * Filter by query
     *
     * @param query
     */
    filterByQuery(query: string): void
    {
        this.searchQuery$.next(query);
    }

    /**
     * Reset filter
     */
    resetFilter(): void
    {
        this.filter$.next('posts');
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any
    {
        return item.id || index;
    }
    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    // private _calcNumberOfCards(): void
    // {
    //     // Prepare the numberOfCards object
    //     this.numberOfCards = {};

    //     // Prepare the count
    //     let count = 0;

    //     // Go through the filters
    //     this.filters.forEach((filter) => {

    //         // For each filter, calculate the card count
    //         if ( filter === 'all' )
    //         {
    //             count = this._fuseCards.length;
    //         }
    //         else
    //         {
    //             count = this.numberOfCards[filter] = this._fuseCards.filter(fuseCard => fuseCard.nativeElement.classList.contains('filter-' + filter)).length;
    //         }

    //         // Fill the numberOfCards object with the counts
    //         this.numberOfCards[filter] = count;
    //     });
    // }

    /**
     * Filter the cards based on the selected filter
     *
     * @private
     */
    private _filterCards(): void
    {
        // Go through all fuse-cards
        this._fuseCards.forEach((fuseCard) => {

            // If the 'all' filter is selected...
            if ( this.selectedFilter === 'all' )
            {
                // Remove hidden class from all cards
                fuseCard.nativeElement.classList.remove('hidden');
            }
            // Otherwise...
            else
            {
                // If the card has the class name that matches the selected filter...
                if ( fuseCard.nativeElement.classList.contains('filter-' + this.selectedFilter) )
                {
                    // Remove the hidden class
                    fuseCard.nativeElement.classList.remove('hidden');
                }
                // Otherwise
                else
                {
                    // Add the hidden class
                    fuseCard.nativeElement.classList.add('hidden');
                }
            }
        });
    }
}

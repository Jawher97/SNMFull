import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, QueryList, ViewChild, ViewChildren, ViewEncapsulation } from '@angular/core';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { MatDrawer } from '@angular/material/sidenav';
import { FuseCardComponent } from '@fuse/components/card';
import { LinkedinPost } from 'app/core/models/linkedin-post';
import { FacebookService } from 'app/core/services-api';
import { InstagramService } from 'app/core/services-api/instagram.service';
import { LinkedInPostService } from 'app/core/services-api/linkedIn-post.service';
import { PostsService } from 'app/core/services-api/posts.service';
import { Observable, Subject, combineLatest, distinctUntilChanged, map, takeUntil } from 'rxjs';

@Component({
  selector: 'posts-home',
  templateUrl: './posts-home.component.html',
  styleUrls: ['./posts-home.component.scss'],
encapsulation  : ViewEncapsulation.None,
changeDetection: ChangeDetectionStrategy.OnPush
})
export class PostsHomeComponent {
  @ViewChildren(FuseCardComponent, {read: ElementRef}) private _fuseCards: QueryList<ElementRef>;
  @ViewChild('matDrawer', {static: true}) matDrawer: MatDrawer;
  @ViewChild('linkedInMatDrawer', {static: true}) linkedInMatDrawer: MatDrawer;
  @ViewChild('instaMatDrawer', {static: true}) instaMatDrawer: MatDrawer;
  @ViewChild('facebookMatDrawer', {static: true}) facebookMatDrawer: MatDrawer;
  filters: string[] = ['all', 'Facebook', 'Instagram', 'LinkedIn', 'Twitter'];
  drawerMode: 'side' | 'over';
  numberOfCards: any = {};
  selectedFilter: string = 'all';
  linkedinposts$: Observable<LinkedinPost[]>
  linkedinpost: LinkedinPost[]
  latestLinkedinpost:LinkedinPost
  latestFacebookPost:any

  instagramPosts:any

  private _unsubscribeAll: Subject<any> = new Subject<any>();
    /**
     * Constructor
     */
    constructor(  
        private _postsService: LinkedInPostService,
        private _facebookService: FacebookService,
        private _instagramService: InstagramService,
      private _changeDetectorRef: ChangeDetectorRef,)
    {
    }
// -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
      
    //     this._postsService.getPosts().subscribe();
    //    this._postsService.getLinkedInPosts();
      this.linkedinposts$=this._postsService.linkedinposts$

    // Get the Post
    this._postsService.linkedinposts$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((post: LinkedinPost[]) => {
        // Get the brand
        post.forEach(p=>this.latestLinkedinpost = p);
        
         // Patch values to the form
        //  this.brandForm.patchValue(brand);
        // Toggle the edit mode off
        // this.toggleEditMode(false);
        // Mark for check
        this._changeDetectorRef.markForCheck();
      });
    
      console.log("linkedinposts$: ", JSON.stringify(this.latestLinkedinpost));
      
      
        // Get posts
    //     this.linkedinposts$ = combineLatest([this._postsService.linkedinposts$]).pipe(
    //       distinctUntilChanged(),
    //       map(([posts]) => {

    //           if ( !posts || !posts.length )
    //           {
    //               return;
    //           }

    //           // Store the filtered posts
    //           let filteredPosts = posts;

    //           // // Filter by query
    //           // if ( searchQuery )
    //           // {
    //           //     searchQuery = searchQuery.trim().toLowerCase();
    //           //     filteredPosts = filteredPosts.filter(post => post.message.toLowerCase().includes(searchQuery));
    //           // }

    //           // // Show all
    //           // if ( filter === 'posts' )
    //           // {
    //           //     // Do nothing
    //           // }

    //           // Show archive
    //           // const isArchive = filter === 'archived';
    //           // filteredPosts = filteredPosts.filter(post => post.archived === isArchive);

    //           // // Filter by label
    //           // if ( filter.startsWith('label:') )
    //           // {
    //           //     const labelId = filter.split(':')[1];
    //           //     // filteredPosts = filteredPosts.filter(post => !!post.labels.find(item => item.id === labelId));
    //           // }

    //           return filteredPosts;
    //       })
          
    //   ); console.log("linkedinposts$: ", JSON.stringify( this.linkedinposts$));

      /** Instagram */
      this._instagramService.posts$.subscribe((responseData: any) => {
        const latestPost = responseData.data[0]; // Obtenir le premier élément du tableau des posts
        this.instagramPosts = latestPost; // Assigner directement le dernier post
        console.log (JSON.stringify(this.instagramPosts));
        this._changeDetectorRef.markForCheck();

        
        
    }); 
    /** Facebook */
    // this._facebookService.getLatestFacebookPost().subscribe();
    // this._facebookService.latestFacebookPost$.subscribe((responseData: any) => {
    //     this._facebookService.getLatestFacebookPost().subscribe((responseData: any) => {
   
    //     const latestPost = responseData.data[0]; // Obtenir le premier élément du tableau des posts
    //     this.latestFacebookPost = latestPost; // Assigner directement le dernier post
    //     console.log ("latestFacebookPost : ",JSON.stringify(this.latestFacebookPost)); 
    // });
      // Get the Post
    //   this._facebookService.latestFacebookPost$
    //   .subscribe((post: any[]) => {
    //     // Get the brand
    //     // post.forEach(p=>this.latestLinkedinpost = p);
    //     this.latestFacebookPost = post; 
    //      console.log ("latestFacebookPost : ",JSON.stringify(this.latestFacebookPost)); 
    //      // Patch values to the form
    //     //  this.brandForm.patchValue(brand);
    //     // Toggle the edit mode off
    //     // this.toggleEditMode(false);
    //     // Mark for check
    //     this._changeDetectorRef.markForCheck();
    //   });
      this._facebookService.latestFacebookPost$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((post: any) => {
        // Get the brand
        post.data.forEach(p=>this.latestFacebookPost = p);
     
         // Patch values to the form
        //  this.brandForm.patchValue(brand);
        // Toggle the edit mode off
        // this.toggleEditMode(false);
        // Mark for check
        this._changeDetectorRef.markForCheck(); console.log ("latestFacebookPost : ",JSON.stringify(this.latestFacebookPost)); 
      });
    }

  // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * On filter change
     *
     * @param change
     */
    onFilterChange(change: MatButtonToggleChange): void
    {
        // Set the filter
        this.selectedFilter = change.value;

        // Filter the cards
        this._filterCards();
    }
     // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    private _calcNumberOfCards(): void
    {
        // Prepare the numberOfCards object
        this.numberOfCards = {};

        // Prepare the count
        let count = 0;

        // Go through the filters
        this.filters.forEach((filter) => {

            // For each filter, calculate the card count
            if ( filter === 'all' )
            {
                count = this._fuseCards.length;
            }
            else
            {
                count = this.numberOfCards[filter] = this._fuseCards.filter(fuseCard => fuseCard.nativeElement.classList.contains('filter-' + filter)).length;
            }

            // Fill the numberOfCards object with the counts
            this.numberOfCards[filter] = count;
        });
    }

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
    isVideo(val): boolean { return val.toString() === 'video/mp4' || val.toString() === 'video/*'; }
    isIMAGE(val): boolean { return val.toString() === 'image/jpg' || val.toString() === 'image/*'; }
   
     /**
     * Open the detail sidebar
     */
     openDetail(): void
     {
        //  this.drawerComponent = 'profile';
        //  this.drawerOpened = true;
        console.log("hello ");
         // Mark for check
         this._changeDetectorRef.markForCheck();
     }
     
     /**
     * linkedIn toggle
     */
    public linkedInToggle(post:any): void {
      console.log(JSON.stringify(post) );
     
      
     this.linkedInMatDrawer?.toggle();

     this.instaMatDrawer?.close();  
      this.facebookMatDrawer?.close();
  } 
   /**
     * toggle
     */
    public instaToggle(post:any): void {
      console.log(post?.username );
      // this.matDrawer.
      
     this.instaMatDrawer.toggle();
    
    this.linkedInMatDrawer?.close();
      this.facebookMatDrawer.close();
  } 
  public FacebookToggle(post:any): void {
      console.log(post?.username );
      // this.matDrawer.
      // if(post?.username=="art_benr")
     this.facebookMatDrawer.toggle();
     this.linkedInMatDrawer?.close();
     this.instaMatDrawer.close();
    // else
      // this.matDrawer.toggle();
  }

}

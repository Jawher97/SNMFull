import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, QueryList, ViewChildren, ViewEncapsulation } from '@angular/core';
import { MatButtonToggleChange } from '@angular/material/button-toggle';
import { MatDialog } from '@angular/material/dialog';
import { FuseCardComponent } from '@fuse/components/card';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { Post } from 'app/core/models/post.model';
import { PostsService } from 'app/core/services-api/posts.service';
import { cloneDeep } from 'lodash';
import { BehaviorSubject, combineLatest, distinctUntilChanged, map, Observable, Subject, takeUntil } from 'rxjs';
import { PostsDetailsComponent } from '../details/details.component';
import { InstagramService } from 'app/core/services-api/instagram.service';
import { InstacommentsComponent } from '../instacomments/instacomments.component';

type NewType = any;

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
    // labels$: Observable<Label[]>;
    posts$: Observable<Post[]>;
    igPosts$ : Observable<any>;
    filters: string[] = ['all', 'article', 'listing', 'list', 'info', 'shopping', 'pricing', 'testimonial', 'post', 'interactive'];
    numberOfCards: any = {};
    selectedFilter: string = 'all';

    drawerMode: 'over' | 'side' = 'side';
    drawerOpened: boolean = true;
    filter$: BehaviorSubject<string> = new BehaviorSubject('posts');
    searchQuery$: BehaviorSubject<string> = new BehaviorSubject(null);
    masonryColumns: number = 4;

    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService,
        private _matDialog: MatDialog,
        private _postsService: PostsService,
        private _instagramService: InstagramService
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get the filter status
     */
    get filterStatus(): string
    {
        return this.filter$.value;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    instagramPosts:any

    ngOnInit(): void {
        this._instagramService.posts$.subscribe((responseData: any) => {
            const latestPost = responseData.data[0]; // Obtenir le premier élément du tableau des posts
            this.instagramPosts = latestPost; // Assigner directement le dernier post
        });
        
        
        
      
        
          
  
        
      
          
        // Request the data from the server
        // this._postsService.getLabels().subscribe();
        this._postsService.getPosts().subscribe();

        // Get labels
        // this.labels$ = this._postsService.labels$;

        // Get posts
        this.posts$ = combineLatest([this._postsService.posts$, this.filter$, this.searchQuery$]).pipe(
            distinctUntilChanged(),
            map(([posts, filter, searchQuery]) => {

                if ( !posts || !posts.length )
                {
                    return;
                }

                // Store the filtered posts
                let filteredPosts = posts;

                // Filter by query
                if ( searchQuery )
                {
                    searchQuery = searchQuery.trim().toLowerCase();
                    filteredPosts = filteredPosts.filter(post => post.title.toLowerCase().includes(searchQuery) || post.postContent.toLowerCase().includes(searchQuery));
                }

                // Show all
                if ( filter === 'posts' )
                {
                    // Do nothing
                }

                // Show archive
                const isArchive = filter === 'archived';
                // filteredPosts = filteredPosts.filter(post => post.archived === isArchive);

                // Filter by label
                if ( filter.startsWith('label:') )
                {
                    const labelId = filter.split(':')[1];
                    // filteredPosts = filteredPosts.filter(post => !!post.labels.find(item => item.id === labelId));
                }

                return filteredPosts;
            })
        );


        // Subscribe to media changes
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({matchingAliases}) => {

                // Set the drawerMode and drawerOpened if the given breakpoint is active
                if ( matchingAliases.includes('lg') )
                {
                    this.drawerMode = 'side';
                    this.drawerOpened = true;
                }
                else
                {
                    this.drawerMode = 'over';
                    this.drawerOpened = false;
                }

                // Set the masonry columns
                //
                // This if block structured in a way so that only the
                // biggest matching alias will be used to set the column
                // count.
                if ( matchingAliases.includes('xl') )
                {
                    this.masonryColumns = 3;
                }
                else if ( matchingAliases.includes('lg') )
                {
                    this.masonryColumns = 4;
                }
                else if ( matchingAliases.includes('md') )
                {
                    this.masonryColumns = 3;
                }
                else if ( matchingAliases.includes('sm') )
                {
                    this.masonryColumns = 2;
                }
                else
                {
                    this.masonryColumns = 1;
                }

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
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
    /**
     * Add a new post
     */
    
    addNewPost(): void
    {
        this._matDialog.open(PostsDetailsComponent, {
            autoFocus: false,
            data     : {
                post: {}
            }
        });
    }
    


    /**
     * Open the edit labels dialog
     */
    openEditLabelsDialog(): void
    {
        // this._matDialog.open(PostsLabelsComponent, {autoFocus: false});
    }

    /**
     * Open the post dialog
     */
    openPostDialog(post: Post): void
    {
        this._matDialog.open(PostsDetailsComponent, {
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

    //afficher les commentaires
    showComments(postId: string): void {
        this._matDialog.open(InstacommentsComponent, {
          autoFocus: false,
          data: {
            postId: postId
          }
        });
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

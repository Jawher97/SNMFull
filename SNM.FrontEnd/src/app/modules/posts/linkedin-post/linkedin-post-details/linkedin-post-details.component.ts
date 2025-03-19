// import { ChangeDetectorRef, Component, ElementRef, Input, OnInit, ViewChild, ViewChildren } from '@angular/core';
// import { MatDrawer } from '@angular/material/sidenav';
// import { FuseCardComponent } from '@fuse/components/card';
// import { LinkedinReaction } from 'app/core/models/LinkedIn/linkedin-reaction.model';
// import { LinkedinComment } from 'app/core/models/linkedin-comments';
// import { LinkedinPost } from 'app/core/models/linkedin-post';
// import { LinkedInPostService } from 'app/core/services-api/linkedIn-post.service';
// import { LinkedInService } from 'app/core/services-api/linkedin.service';
// import { ActiveCommentInterface } from 'app/core/types/activeComment.type';
// import { Subject, takeUntil } from 'rxjs';

// @Component({
//   selector: 'linkedin-post-details',
//   templateUrl: './linkedin-post-details.component.html',
//   styleUrls: ['./linkedin-post-details.component.scss']
// })
// export class LinkedinPostDetailsComponent implements OnInit {
//   @Input() instagramPostId: any;
//   @Input() drawer: MatDrawer;
//   // @ViewChildren(FuseCardComponent, {read: ElementRef}) private _fuseCards: QueryList<ElementRef>;
//   @ViewChild('expandableCard02', { static: true }) expandableCard02: FuseCardComponent;
//   linkedInPost: LinkedinPost;
//   commentsList: LinkedinComment[];
//   comment: LinkedinComment;
//   activeComment: ActiveCommentInterface | null = null;
//   isLikedByAuthor: boolean;
//   // activeComment: ActiveCommentInterface | null = null;
//   private _unsubscribeAll: Subject<any> = new Subject<any>();

//   /**
//       * Constructor
//       */
//   constructor(
//     private _linkedInPostService: LinkedInPostService,
//     private _linkedInService: LinkedInService,
//     private _changeDetectorRef: ChangeDetectorRef,) {
//   }
//   /**
//       * On init
//       */
//   ngOnInit(): void {
//     // Get the linkedinposts
//     // Get the Post
//     this._linkedInPostService.linkedinposts$
//       .pipe(takeUntil(this._unsubscribeAll))
//       .subscribe((post: LinkedinPost[]) => {
//         // Get the brand
//         post.forEach(p => this.linkedInPost = p);
   
//         // Patch values to the form
//         //  this.brandForm.patchValue(brand);
//         // Toggle the edit mode off
//         // this.toggleEditMode(false);
//         // Mark for check
//         this._changeDetectorRef.markForCheck();
//       });
//     this.expandableCard02.expanded = false
//     // this.loadComments() 
//     this.isLikedByAuthor = this.linkedInPost?.insight?.isLikedByAuthor
//   }
//   loadComments(): void {

//     // this.expandableCard02.expanded = !this.expandableCard02.expanded
//     if (!this.expandableCard02.expanded) {

//       // this.linkedinservice.getComments("urn:li:ugcPost:7082603111759933440").subscribe(
//       this._linkedInService.getComments("urn:li:share:7101852118294085632").subscribe(
//         (comments: LinkedinComment[]) => {

//           this.commentsList = comments;
       
//           this._changeDetectorRef.markForCheck();
//         },
//         (error) => {
//           console.error('Erreur lors de la récupération des commentaires:', error);
//         }
//       );
//       console.log(this.commentsList);
//       this.expandableCard02.expanded = true
//     }
//     else {
//       this.expandableCard02.expanded = false
//     }


//   }
//   /**
//    * Like Post
//    */
 
//   // likePost(entityUrn: string): void {
//   //   console.log(entityUrn)
//   //   this._linkedInService.like(entityUrn, this.linkedInPost.author_urn).subscribe(() => {  //currentUserId
//   //     console.log(" Liked!")
//   //     this.isLikedByAuthor = true
//   //   })
//   // }
//   createReaction(entityUrn: string,reactionType:string): void {
//     console.log(entityUrn)
//     const linkedinreaction={
//       root:entityUrn,
//       actor:this.linkedInPost.author_urn,
//       reactionType:reactionType,
//     }

//     this._linkedInService.createReaction(linkedinreaction ).subscribe(() => {  //currentUserId
//       console.log(" Liked!")
//       this.isLikedByAuthor = true
//     })
//   }
//   /**
//    * UnLike Post
//    */
//   deleteReaction(entityUrn: string): void {
//     console.log(" Unliked!")
//     this._linkedInService.unlike(entityUrn, this.linkedInPost.author_urn).subscribe(() => {
//       console.log(" Unliked!")
//       this.isLikedByAuthor = false
//     })
//   }
//   /**
//    * SetActiveComment
//    */
//   setActiveComment(activeComment: ActiveCommentInterface | null): void {
//     this.activeComment = activeComment;
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

// }

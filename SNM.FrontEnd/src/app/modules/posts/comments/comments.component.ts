import { HttpClient } from '@angular/common/http';
import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, EventEmitter, Input, Output, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { LinkedinComment } from 'app/core/models/linkedin-comments';
import { LinkedinPost } from 'app/core/models/linkedin-post';
import { LinkedInService } from 'app/core/services-api/linkedin.service';
import { ActiveCommentInterface } from 'app/core/types/activeComment.type';

@Component({
  selector: 'app-comments',
  templateUrl: './comments.component.html',
  styleUrls: ['./comments.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CommentsComponent {
  @Input() currentUserId!: string;
  @Input() postId!: string;

  commentsList: LinkedinComment[];
  comment: LinkedinComment;
  activeComment: ActiveCommentInterface | null = null;
  post: LinkedinPost;
  sharemessage: string;
orgId:string;
  isLikedByAuthor: boolean;

  private orgurn = localStorage.getItem('org_urn');

  @ViewChild('shareModal', { static: false }) shareModal: ElementRef;

  constructor(private _changeDetectorRef: ChangeDetectorRef, private linkedinservice: LinkedInService) {}

  ngOnInit(): void {
this.orgId="urn:li:organization:98526315"
    this.linkedinservice.getLatestPost(this.orgurn).subscribe(  //currentUserid
    (post: LinkedinPost) => {
    this.post = post;
    console.log('Latest Post:', this.post);
    });

    this.loadComments();

  }


like(entityUrn: string): void {
    this.linkedinservice.like(entityUrn, this.post.author_urn).subscribe(() => {  //currentUserId
      console.log(" Liked!")
      this.isLikedByAuthor = true
    })

}


unlike(entityUrn: string): void {
    this.linkedinservice.unlike(entityUrn, this.orgurn).subscribe(() => {
      console.log(" Unliked!")
      this.isLikedByAuthor = false
    })

}


  loadComments(): void {
    // this.linkedinservice.getComments("urn:li:ugcPost:7082603111759933440").subscribe(
    this.linkedinservice.getComments("urn:li:share:7101852118294085632").subscribe(
      (comments: LinkedinComment[]) => {

        this.commentsList = comments;
  
        this._changeDetectorRef.markForCheck();
      },
      (error) => {
        console.error('Erreur lors de la récupération des commentaires:', error);
      }
    );
    console.log(this.commentsList);
  }




  addComment({ text, entityUrn} : {text: string, entityUrn: string;}): void {
    console.log('addComment', text, entityUrn)
    this.linkedinservice.CreateComment(entityUrn, text, this.orgId).subscribe(
      createdComment => {
        this.commentsList = [...this.commentsList, createdComment];
        this._changeDetectorRef.markForCheck();

      }
    )
  }

  addSubComment({ text, entityUrn} : {text: string, entityUrn: string;}): void {
    console.log('addComment', text, entityUrn)
    this.linkedinservice.CreateSubComment(entityUrn, text, this.orgId).subscribe(
      createdComment => {
        this.commentsList = [...this.commentsList, createdComment];
        this._changeDetectorRef.markForCheck();
        this.activeComment = null

      }
    )
  }


editComment({ text, commentId}: { text: string; commentId: string; }): void {
    this.linkedinservice
      .editComment(commentId, text, this.orgId, this.post.postId)
      .subscribe((updatedComment) => {
        this.commentsList = this.commentsList.map((comment) => {
          if (comment.commentId === commentId) {
            return updatedComment;
          }
          return comment;
        });

        this.activeComment = null;
      });
  }


  deleteComment(commentId: string): void {
    this.linkedinservice.DeleteComment(commentId, this.orgId ,this.post.postId).subscribe(() => { //currentuser
      this.commentsList = this.commentsList.filter(
        (comment) => comment.commentId !== commentId
      );
    });
  }



  setActiveComment(activeComment: ActiveCommentInterface | null): void {
    this.activeComment = activeComment;
  }



  /*openShareModal() {
    const modalRef = this.modalService.open(this.shareModal);
    modalRef.result.then(
      (result) => {
        if (result === 'cancel') {
          console.log("Paratge annulé.")
        }
      },
      (reason) => {
        // Le partage a été fermé (par exemple, en cliquant en dehors de la fenêtre modale)
      }
    );
  }*/



  resharePost(postId: string, author_urn: string): void {
    this.linkedinservice.resharePost(postId, author_urn, this.sharemessage);

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

}

import { Component, EventEmitter, Input, Output } from '@angular/core';
import { ActiveCommentInterface } from 'app/core/types/activeComment.type';
import { ActiveCommentTypeEnum } from 'app/core/types/activeCommentType.enum';
import { LinkedinComment } from 'app/core/types/linkedin-post-details.type';
import { Observable } from 'rxjs';

@Component({
  selector: 'linkedin-post-comment',
  templateUrl: './linkedin-post-comment.component.html',
  styleUrls: ['./linkedin-post-comment.component.scss']
})
export class LinkedinPostCommentComponent {
  
  @Input() comment!: LinkedinComment;
  @Input() currentUserId!: string;
  @Input() activeComment!: ActiveCommentInterface | null;
  @Input() parentId!: string | null;


  replies$: Observable<LinkedinComment[]>;

  @Output() setActiveComment = new EventEmitter<ActiveCommentInterface | null>();
  @Output() addComment = new EventEmitter<{ text: string; parentId: string | null }>();
  @Output() editComment = new EventEmitter<{ text: string; commentId: string }>();
  @Output() deleteComment = new EventEmitter<string>();
  @Output() like = new EventEmitter<string>();
  @Output() unlike = new EventEmitter<string>();

  canEdit: boolean = false;
  canDelete: boolean = false;
  activeCommentType = ActiveCommentTypeEnum;
  replyId: string;
  orgId="urn:li:organization:98526315"
 // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------


  isAuthor(actorUrn: string): boolean {
    console.log(actorUrn,this.currentUserId)
    return actorUrn === this.currentUserId;
}
  isReplying(): boolean {

    if (!this.activeComment) {
      return false;
    }
    return (
      this.activeComment.id === this.comment.commentId &&
      this.activeComment.type === this.activeCommentType.replying
    );
  }
  isEditing(): boolean {

    if (!this.activeComment) {
      return false;
    }
    return (
      this.activeComment.id === this.comment.commentId &&
      this.activeComment.type === 'editing'
    );
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

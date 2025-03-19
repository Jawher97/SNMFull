import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { LinkedinComment } from 'app/core/models/linkedin-comments';
import { LinkedInService } from 'app/core/services-api/linkedin.service';
import { ActiveCommentInterface } from 'app/core/types/activeComment.type';
import { ActiveCommentTypeEnum } from 'app/core/types/activeCommentType.enum';
import { Observable } from 'rxjs';

@Component({
  selector: 'comment',
  templateUrl: './comment.component.html',
  styleUrls: ['./comment.component.scss']
})
export class CommentComponent {

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
  orgId: string;

  constructor(private _changeDetectorRef: ChangeDetectorRef, private linkedinservice: LinkedInService) {}


  ngOnInit() {
    this.replies$ = this.linkedinservice.getSubComments(this.comment.activityUrn);
    this.replyId = this.comment.activityUrn;
    console.log(this.replyId)
    this.orgId="urn:li:organization:98526315"
  }


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
trackByFn(index: number, item: any): any
{
    return item.id || index;
}
}

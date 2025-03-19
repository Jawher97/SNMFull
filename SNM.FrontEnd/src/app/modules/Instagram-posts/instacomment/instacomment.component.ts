import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { InstagramComments } from 'app/core/models/instagram-comments';
import { InstagramService } from 'app/core/services-api/instagram.service';
import { ActiveCommentInterface } from 'app/core/types/activeComment';
import { ActiveCommentTypeEnum } from 'app/core/types/activeCommentType.enum';
import { Observable } from 'rxjs';

@Component({
  selector: 'comment',
  templateUrl: './instacomment.component.html',
  styleUrls: ['./instacomment.component.scss']
})
export class InstacommentComponent {
  @Input() comment!: InstagramComments;

  @Input() activeComment!: ActiveCommentInterface | null;
  @Input() igMediaId!: string | null;


  replies$: Observable<InstagramComments[]>;

  @Output() setActiveComment = new EventEmitter<ActiveCommentInterface | null>();
  @Output() addComment = new EventEmitter<{ message: string; IgCommentId: string | null }>();
  @Output() deleteComment = new EventEmitter<string>();


  activeCommentType = ActiveCommentTypeEnum;
  replyId: string;

  constructor(private _changeDetectorRef: ChangeDetectorRef, private instagramservice: InstagramService) {}


  ngOnInit() {
    console.log(this.comment.id)
    this.replies$=this.instagramservice.getSubComments(this.comment.id)
    console.log(this.replies$)
     this.replyId = this.comment.id;
    console.log(this.replyId)
  }



isReplying(): boolean {
  if (!this.activeComment) {
    return false;
  }
  return (
    this.activeComment.id === this.comment.id &&
    this.activeComment.type === this.activeCommentType.replying
  );
}

}

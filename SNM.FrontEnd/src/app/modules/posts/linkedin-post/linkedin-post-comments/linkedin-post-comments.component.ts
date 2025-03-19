import { Component, Input } from '@angular/core';
import { LinkedinComment } from 'app/core/models/linkedin-comments';
import { LinkedinPost } from 'app/core/models/linkedin-post';
import { LinkedInService } from 'app/core/services-api/linkedin.service';
import { ActiveCommentInterface } from 'app/core/types/activeComment.type';

@Component({
  selector: 'linkedin-post-comments',
  templateUrl: './linkedin-post-comments.component.html',
  styleUrls: ['./linkedin-post-comments.component.scss']
})
export class LinkedinPostCommentsComponent {
  @Input() linkedInPost: LinkedinPost;
  @Input() commentsList: LinkedinComment[];
  @Input() currentUserId!: string;
  @Input() postId!: string;

  comment: LinkedinComment;
  activeComment: ActiveCommentInterface | null = null;
  isLikedByAuthor: boolean;
  orgId="urn:li:organization:98526315"


  /**
     * Constructor
     */
  constructor(
    
    private linkedinservice: LinkedInService)
     {}

  setActiveComment(activeComment: ActiveCommentInterface | null): void {
    this.activeComment = activeComment;
  }
  like(entityUrn: string): void {
    // this.linkedinservice.like(entityUrn, this.linkedInPost.author_urn).subscribe(() => {  //currentUserId
    //   console.log(" Liked!")
    //   this.isLikedByAuthor = true
    // })

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

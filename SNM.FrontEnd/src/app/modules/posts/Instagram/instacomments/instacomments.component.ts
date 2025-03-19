import { ChangeDetectorRef, Component } from '@angular/core';
import { InstagramComments } from 'app/core/models/instagram-comments';
import { InstagramService } from 'app/core/services-api/instagram.service';
import { ActiveCommentInterface } from 'app/core/types/activeComment';

@Component({
  selector: 'app-instacomments',
  templateUrl: './instacomments.component.html',
  styleUrls: ['./instacomments.component.scss']
})
export class InstacommentsComponent {
  commentsList: InstagramComments[];
  comment: InstagramComments;

  instagramPosts:any
 

  activeComment: ActiveCommentInterface | null = null;
  

constructor(private _changeDetectorRef: ChangeDetectorRef, private instagramservice: InstagramService) {}

ngOnInit(): void {
  this.instagramservice.posts$.subscribe((responseData: any) => {
    const latestPost = responseData.data[0]; // Obtenir le premier élément du tableau des posts
    this.instagramPosts = latestPost; })

    this.loadComments();
  }

  loadComments(): void {

    this.instagramservice.getComments("17988165941301486").subscribe(

      (comments: InstagramComments[]) => {

        

        this.commentsList = comments;
        console.log(this.commentsList)

        this._changeDetectorRef.markForCheck();

      },

      (error) => {

        console.error('Erreur lors de la récupération des commentaires:', error);

      }

    );

    console.log(this.commentsList);

  }

   addComment({message, igCommentId} : {message: string, igCommentId: string;}): void {
    

    console.log('addComment', message, igCommentId)

    this.instagramservice.CreateComment(igCommentId, message).subscribe(

      createdComment => {

        this.commentsList = [...this.commentsList, createdComment];

        this._changeDetectorRef.markForCheck();

      }

    )

  }
  like(igCommentId: string): void {

    this.instagramservice.getLikeComments(igCommentId).subscribe(() => {

      this.comment.isLiked = true

      console.log("Comment Liked!")

    })




}

  deleteComment(id: string): void {
    this.instagramservice.DeleteComment(id).subscribe(() => { //currentuser
      this.commentsList = this.commentsList.filter(
        (comment) => comment.id !== id
      );
    });
  }

  setActiveComment(activeComment: ActiveCommentInterface | null): void {
    this.activeComment = activeComment;
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

import { ChangeDetectorRef, Component, EventEmitter, Input, Output } from '@angular/core';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute } from '@angular/router';
import { PostService } from 'app/core/services-api/post.service';
import { ToastrService } from 'ngx-toastr';
import { LikesComponent } from '../likes/likes.component';
import { BehaviorSubject, Subject } from 'rxjs';
import { PostDetailsComponent } from '../post-details/post-details.component';
import { SharedDataService } from 'app/core/services-api/SharedDataService ';

@Component({
  selector: 'app-reply',
  templateUrl: './reply.component.html',
  styleUrls: ['./reply.component.scss']
})
export class ReplyComponent {
  SubcommentDetails:any={   
    commentId:'',
    message:'',    
    postId:'',
    fromId:'',
    photoUrl:'',
    commentUrn:'',
    likesCount:0,
    replies:[],
    fromPicture:'',
    videoUrl:'',
    FromName:'',
    FromPicture:''
   
  }
  reactions:any={

    name :'',
    picture :'',
    reactionType:2
 
 }
@Input() comment:any;
@Input() selectedPostValue : any;
@Input() replyExpandedStates:any;
@Input() currentIndex: number;
@Input() selectedPostSubject:any;
@Output() selectedPostSubjectChange: EventEmitter<Subject<any>> = new EventEmitter<Subject<any>>();

// Use this method to update the subject in ComponentA
isEditing = false;
editedMessage: string = '';
editingReplyIndex:any
showEmojiPickerReply: boolean = false;
showEmojiPickerUpdateReply:any=false;
sets = [
  'native',
  'google',
  'twitter',
  'facebook',
  'emojione',
  'apple',
  'messenger',
];

set: string = 'facebook';
constructor(private PostDetailsComponent:PostDetailsComponent,
  private sharedDataService: SharedDataService,
            private _changeDetectorRef: ChangeDetectorRef,
            private postService:PostService,
            private route: ActivatedRoute,
            public dialog: MatDialog,
            private toastr:ToastrService
){}
updateSelectedPostInComponentB(value: any) {
  this.sharedDataService.updateSelectedPost(value);
}
uploadMediaSuComment( fileList: FileList): void {
   
  // Return if canceled
  if (!fileList.length) {
    return;
  }

  const allowedTypes = ['image/jpeg', 'image/png', 'video/mp4', 'video/x-m4v', 'video/*'];
  for (let i = 0; i < fileList.length; i++) {
    const file = fileList[i];
    
    // Return if the file is not allowed
    if (!allowedTypes.includes(file.type)) {
      return;
    }
   
    this._readAsDataURL(file).then((data) => {
    
     console.log(file.type+"file.type")
      // Add the image or video object to the post
      if (file.type.includes("video/mp4")) {
        
        this.SubcommentDetails.videoUrl=data;
      }
      if (file.type.includes("image")) {
    
        this.SubcommentDetails.photoUrl=data;
      }
     
    // this.commentDetails.photoUrl="https://consultim-it.com/wp-content/uploads/2022/10/Logo-New-.png"
       this._changeDetectorRef.detectChanges();
    });
  }
}
private _readAsDataURL(file: File): Promise<string> {
  // Return a new promise
  return new Promise((resolve, reject) => {
    // Create a new reader
    const reader = new FileReader();

    // Resolve the promise on success
    reader.onload = (event) => {
      const dataURL = event.target.result as string;
      resolve(dataURL); // Resolve the promise with the dataURL
    };

    // Reject the promise on error
    reader.onerror = (e) => {
      reject(e);
    };

    // Read the file as data URL
    reader.readAsDataURL(file);
  });
}
CreateSubComment(postId:any,fromId:any,channelTypeName:any,channelId:any ,commentId:any,commentUrn:any,i:any){
  console.log(postId+"iddd")
  this.SubcommentDetails.postId=postId
  this.SubcommentDetails.fromId=fromId
  this.SubcommentDetails.commentUrn=commentUrn
  this.SubcommentDetails.commentId=commentId
  
      this.postService.CreateSubComment(this.SubcommentDetails,channelTypeName,channelId).subscribe({
        next:(resp)=>{
          console.log(JSON.stringify(resp.data)+"this.selectedPostValue")
          if(resp.succeeded){
       resp.data.fromName=this.selectedPostValue?.name
       resp.data.fromPicture=this.selectedPostValue?.photo
       resp.data.likesCount=0
console.log(i+':iiiiiiiiii')
            const updatedCommentsArray = [...(this.selectedPostValue?.comments[i]?.replies || []), resp.data];

          // Create a new object with the updated comments array
          this.selectedPostValue = {
            ...this.selectedPostValue,
            comments: this.selectedPostValue?.comments.map((c, index) =>
              index === i
                ? { ...c, replies: updatedCommentsArray }
                : c
            )
          };
            console.log(JSON.stringify(this.selectedPostValue)+"this.selectedPostValue"+JSON.stringify(updatedCommentsArray))
            console.log("this.selectedPostValue"+JSON.stringify(updatedCommentsArray))

         this.selectedPostSubjectChange.emit(this.selectedPostValue)
          // Update the selectedPost property to notify subscribers of the change
          this.selectedPostSubject.next(this.selectedPostValue);
          this._changeDetectorRef.detectChanges();

          this.SubcommentDetails={   
            commentId:'',
            mesage:'',  
            fromId:'',
            postId:'', 
            photoUrl:'',
            commentUrn:''
          }
          // window.location.reload();
          
            this.toastr.success("comment Add", 'SNM');
          }else{
            this.toastr.success(resp.message, 'SNM');
          }
        }})
}

DeleteReply(postId:any,fromId:any,channelTypeName:any,channelId:any ,comment:any,i:any){
 
  comment.postId=postId
  comment.fromId=fromId
// console.log(JSON.stringify(comment)+"comment")
  this.postService.DeleteComment(comment, channelTypeName, channelId).subscribe({
    next: (resp) => {
      console.log(JSON.stringify(i)+"comment")
      // Find the index of the comment in the comments array
      const commentIndex = this.selectedPostValue?.comments[i].replies.findIndex((c) => c.commentId === comment.commentId);
      

        // Remove the comment from the comments array
        const updatedCommentsArray = [
          ...this.selectedPostValue?.comments[i]?.replies.slice(0, commentIndex),
          ...this.selectedPostValue?.comments[i]?.replies.slice(commentIndex + 1)
        ];
        console.log(JSON.stringify(updatedCommentsArray)+"comment.commentId")
        // Create a new object with the updated comments array
        this.selectedPostValue = {
          ...this.selectedPostValue,
          comments: updatedCommentsArray
        };
        // console.log(JSON.stringify(comment)+"comment")
        this.selectedPostSubjectChange.emit(this.selectedPostValue);
  
        this.toastr.success("Comment Deleted", 'SNM');
      
    },
    error: (err) => {
      // Handle error, if necessary
      console.error("Error deleting comment:", err);
      // Optionally, you may want to notify the user about the error
      this.toastr.error("Error deleting comment", 'Error');
    }
  });
  
}

UpdateSubComment(postId: any, fromId: any, channelTypeName: any, channelId: any, reply: any, j: any, i: any) {
  reply.postId = postId;
  reply.fromId = fromId;
  reply.message = this.editedMessage;
  this.postService.UpdateComment(reply, channelTypeName, channelId).subscribe({
    next: (resp) => {
      if (resp.succeeded) {
        resp.data.fromName = this.selectedPostValue?.name;
        resp.data.fromPicture = this.selectedPostValue?.photo;

        const updatedRepliesArray = [...( this.selectedPostValue?.comments[i]?.replies || [])];

        if (this.editingReplyIndex !== null && this.editingReplyIndex < updatedRepliesArray.length) {
          updatedRepliesArray[this.editingReplyIndex] = resp.data;
        }

        this.selectedPostValue = {
          ...this.selectedPostValue,
          comments: this.selectedPostValue?.comments.map((c, index) =>
            index === i
              ? { ...c, replies: updatedRepliesArray }
              : c
          )
        };
        this.selectedPostSubjectChange.emit(this.selectedPostValue)
        this.selectedPostSubject.next(this.selectedPostValue);
        this._changeDetectorRef.detectChanges();
        this.SubcommentDetails = {
          commentId: '',
          message: '',
          fromId: '',
          postId: '',
          photoUrl: ''
        };

        this.isEditing = false;
        this.editingReplyIndex = null;
        this.editedMessage = '';

        this.toastr.success("Comment Updated", 'SNM');
      } else {
        this.toastr.error(resp.message, 'SNM');
      }
    },
    error: (error) => {
      console.error("UpdateSubComment Error:", error);
      // Handle error appropriately, e.g., show an error message to the user
    }
  });
}

DeleteReactionSubComment(comment: any, channelTypeName: any, channelId: any, i: any, j: any) {
  this.postService.DeleteReactionComment(comment, channelTypeName, channelId).subscribe({
    next: (resp) => {
      if (resp.succeeded) {
        this.selectedPostValue.comments[i].replies[j].isLikedByAuthor = false;
        this.selectedPostValue.comments[i].replies[j].likesCount -= 1;
        const reactionIndex = this.selectedPostValue?.comments[i]?.replies[j]?.reactions.findIndex(r => r.commentId === comment.commentId);

        // if (reactionIndex !== -1) {
          // Remove the deleted reaction from the array
          const updatedReactionsArray = [
            ...this.selectedPostValue?.comments[i]?.replies[j]?.reactions.slice(0, reactionIndex),
            ...this.selectedPostValue?.comments[i]?.replies[j]?.reactions.slice(reactionIndex + 1)
          ];

          // Update the selectedPostValue with the new reactions array
          this.selectedPostValue = {
            ...this.selectedPostValue,
            comments: this.selectedPostValue?.comments.map((c, index) =>
              index === i ? {
                ...c,
                replies: c.replies.map((r, subIndex) =>
                  subIndex === j ? { ...r, reactions: updatedReactionsArray } : r
                )
              } : c
            )
          };

          
          this.selectedPostSubjectChange.emit(this.selectedPostValue)
          console.log(JSON.stringify(this.selectedPostValue.comments[i].replies[j]) + "this.selectedValue");
          this.selectedPostSubject.next(this.selectedPostValue);
          this._changeDetectorRef.detectChanges();
        }
      // }
    },
    error: (error) => {
      // Handle error appropriately
      this.toastr.error('Error deleting reaction for comment:', error);
    }
  });
}
CreateReactionSubComment(comment: any, channelTypeName: any, channelId: any, i: any, j: any) {
  this.postService.CreateReactionComment(comment, channelTypeName, channelId).subscribe({
    next: (resp) => {
      if (resp.succeeded) {

        this.selectedPostValue.comments[i].replies[j].isLikedByAuthor = true;
        this.reactions.name = this.selectedPostValue?.name;
        this.reactions.picture = this.selectedPostValue?.photo;

        this.selectedPostValue.comments[i].replies[j].likesCount += 1;
        const updatedRepliesArray = [...this.selectedPostValue.comments[i].replies];
        updatedRepliesArray[j] = {
          ...updatedRepliesArray[j],
          reactions: [...(updatedRepliesArray[j]?.reactions || []), this.reactions],
        };

        // Create a new object with the updated replies array
        this.selectedPostValue = {
          ...this.selectedPostValue,
          comments: this.selectedPostValue?.comments.map((c, index) =>
            index === i ? { ...c, replies: updatedRepliesArray } : c
          ),
        };
        console.log(this.selectedPostValue+"this.selectedPostValue")
        this.selectedPostSubjectChange.emit(this.selectedPostValue)

        this.selectedPostSubject.next(this.selectedPostValue);
        this._changeDetectorRef.detectChanges();
      }
    },
    error: (error) => {
      // Handle error appropriately
      this.toastr.error('Error creating reaction for comment:', error);
    },
    complete: () => {
      // Reset temporary variables after the operation is complete
      this.reactions = {
        name: '',
        picture: '',
      };
    }
  });
}
toggleEmojiPickerReplyUpdate ():void {
  this.showEmojiPickerUpdateReply = !this.showEmojiPickerUpdateReply;
}
openDialog(reactions:any,count:any): void {
  console.log(count+"count")
  const dialogRef = this.dialog.open(LikesComponent, {
    data:{ reactions:reactions,name:this.selectedPostValue.name,photo:this.selectedPostValue.photo,count:count},
    width:'600px',
    height:'auto',
  });

  dialogRef.afterClosed().subscribe(result => {
    console.log('The dialog was closed');
   
  });
}
addEmoji(event): void {
  const text = `${this.SubcommentDetails?.message} ${event.emoji.native}`;
  this.SubcommentDetails.message = text;
  this.showEmojiPickerReply = false;
}
toggleEmojiPickerReply ():void {
  this.showEmojiPickerReply = !this.showEmojiPickerReply;
}
addEmojiReplyUpdate(event): void {
  const text = `${this.editedMessage} ${event.emoji.native}`;
  this.editedMessage = text;
  this.showEmojiPickerUpdateReply = false;
}
EditReply(index:any,message:any) {
   
  this.isEditing = true;
  this.editedMessage = this.isEditing ? message : '';
  this.editingReplyIndex = this.isEditing ? index : null;

}
cancel(){
  this.isEditing = false;
  this.editedMessage = '';
  this.editingReplyIndex =  null;
}

}

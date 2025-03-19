import { ChangeDetectorRef, Component,ElementRef,EventEmitter,HostListener,Input,Output,ViewChild ,NgZone } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { MatDrawerToggleResult } from '@angular/material/sidenav';
import { BehaviorSubject, debounceTime, filter, Subject, takeUntil, tap } from 'rxjs';
import { MatDialog } from '@angular/material/dialog';
import { LikesComponent } from '../likes/likes.component';
import { MatDrawer } from '@angular/material/sidenav';
import { PostService } from 'app/core/services-api/post.service';
import { Toast, ToastrService } from 'ngx-toastr';
import { SharedDataService } from 'app/core/services-api/SharedDataService ';
import { FacebookService } from 'app/core/services-api';
import { InstagramService } from 'app/core/services-api/instagram.service';
import { LinkedInService } from 'app/core/services-api/linkedin.service';
@Component({
  selector: 'app-post-details',
  templateUrl: './post-details.component.html',
  styleUrls: ['./post-details.component.scss']
})
export class PostDetailsComponent {
 //observable
  private selectedPostSubject = new BehaviorSubject<any>(null);
  selectedPost$ = this.selectedPostSubject.asObservable();
  @Input() set selectedPost(value: any) {
    if (value !== undefined) {
      this.selectedPostSubject.next(value);
    }
  }
 //POst
  selectedPostValue: any | undefined;
  //drawer
  @Input() matDrawer: MatDrawer;
  //comment 
  commentDetails:any={   
    commentId:'',
    message:'',    
    postId:'',
    fromId:'',
    photoUrl:'',
    likesCount:0,
    replies:[],
    fromPicture:'',
    videoUrl:'',
    FromName:'',
    FromPicture:''
  }
  // Reactions
  reactions:any={
     name :'',
     picture :'',
     reactionType:2  
  }
// comment Edit
  isEditingComment= false;
  editingCommentIndex:any;
  EditingComment: any ={
   body:''
  } 
  expandableCard:any= false ;

  //Reply button
  replyExpandedStates:any=[];
  //emoji
  showEmojiPickerComment: boolean = false;
  showEmojiPickerUpdate: boolean = false;
  commentExpanded: boolean = false;
  commenttoggle: boolean = false;
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
  constructor(
    private _changeDetectorRef: ChangeDetectorRef,
    private postService:PostService,
    public dialog: MatDialog,
    public toastr:ToastrService,
    private _facebookService:FacebookService,
    private _instgramService:InstagramService,
    private _linkedinService:LinkedInService, 
   
   ) {}

  

ngOnInit(): void {
    this.selectedPost$.subscribe((value) => {
      this.selectedPostValue = value;
      this._changeDetectorRef.detectChanges();       
      console.log(JSON.stringify(this.selectedPostValue)+"comments") 
      
      // switch (this.selectedPostValue?.channelTypeName) {
    

      //   case 'Facebook':
      //     this._facebookService.getLatestFacebookComment(this.selectedPostValue.postIdAPI,this.selectedPostValue.channelId).subscribe((comments)=>{
      //       console.log(comments+"comments")
      //       if(comments.succeeded){
      //       this.selectedPostValue.comments=comments.data
                
      //       this.selectedPostSubject.next(this.selectedPostValue);
      //       this._changeDetectorRef.detectChanges();
      //       }
      //     })
      //     break;
    
      //   case 'Instagram Profile':
      //       this._instgramService.getLatestInstagramComment(this.selectedPostValue.postIdAPI,this.selectedPostValue.channelId).subscribe((comments)=>{
      //         if(comments.succeeded){
      //         this.selectedPostValue.comments=comments.data               
      //         this.selectedPostSubject.next(this.selectedPostValue);
      //         this._changeDetectorRef.detectChanges();
      //           }
      //         })
      //     break;
    
      //   case 'LinkedIn':
      //       this._linkedinService.getLatestLinkedinComment(this.selectedPostValue.postIdAPI,this.selectedPostValue.channelId).subscribe((comments)=>{
      //         if(comments.succeeded){
      //         this.selectedPostValue.comments=comments.data             
      //         this.selectedPostSubject.next(this.selectedPostValue);
      //         this._changeDetectorRef.detectChanges();
      //           }
      //         })
      //     break;
      // }
     
     // this._changeDetectorRef.detectChanges();

    });
  
    console.log(this.selectedPostValue?.postIdAPI+"comments")
    
  }
  handleButtonClick() {
 
    this.expandableCard= !this.expandableCard;
    // You can call your method to fetch comments here
    if(this.expandableCard){
      this.GetComment();
      this._changeDetectorRef.detectChanges();
     
     
    }
 
  }

  //check if detect change in Reply Component
updateSelectedPost($event: any) {
    // Assuming $event is an object, create a new object to force a change
    const modifiedEvent = { ...$event }; 
    this.selectedPostSubject.next(modifiedEvent);
    console.log(JSON.stringify(modifiedEvent) + " modifiedEvent");
    this._changeDetectorRef.detectChanges();
  } 
GetComment(){
  console.log(this.selectedPostValue?.channelTypeName+'Button GetComment');
  switch (this.selectedPostValue?.channelTypeName) {
 
    case 'Facebook':
      this._facebookService.getLatestFacebookComment(this.selectedPostValue.postIdAPI,this.selectedPostValue.channelId).subscribe((comments)=>{
        if(comments.succeeded){
          this.selectedPostValue = { ...this.selectedPostValue, comments: comments.data };
            
        this.selectedPostSubject.next(this.selectedPostValue);
        this._changeDetectorRef.detectChanges();
        }
      })
      break;

    case 'Instagram Profile':
        this._instgramService.getLatestInstagramComment(this.selectedPostValue.postIdAPI,this.selectedPostValue.channelId).subscribe((comments)=>{
          if(comments.succeeded){
            this.selectedPostValue = { ...this.selectedPostValue, comments: comments.data };
              
          this.selectedPostSubject.next(this.selectedPostValue);
          this._changeDetectorRef.detectChanges();
            }
          })
      break;

    case 'LinkedIn':
        this._linkedinService.getLatestLinkedinComment(this.selectedPostValue.postIdAPI,this.selectedPostValue.channelId).subscribe((comments)=>{
          if(comments.succeeded){
            this.selectedPostValue = { ...this.selectedPostValue, comments: comments.data };
          console.log(JSON.stringify(comments.data)+"comments")    
          this.selectedPostSubject.next(this.selectedPostValue);
          this._changeDetectorRef.detectChanges();
            }
          })
      break;
  }
}

// Button Reply
toggleReplyExpansion(index: number): void {
    this.replyExpandedStates[index] = !this.replyExpandedStates[index];
  }


@HostListener('window:keydown', ['$event', 'PostId'])
handleKeyDown(event: KeyboardEvent, postId:any,fromId:any,channelTypeName:any,channelId:any ) {
  if (event.key === 'Enter' && postId) {

    this.CreateComment(postId,fromId,channelTypeName,channelId );
    
  }
}
//Media
uploadMedia( fileList: FileList): void {
   
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
        this.commentDetails.videoUrl=data;
       
      }
      if (file.type.includes("image")) {
        this.commentDetails.photoUrl=data;
      
      }
     
    // this.commentDetails.photoUrl="https://consultim-it.com/wp-content/uploads/2022/10/Logo-New-.png"
       this._changeDetectorRef.detectChanges();
    });
  }
  //  this.post.mediaData=post?.mediaData

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
deleteMedia(): void {
  this.commentDetails.photoUrl = '';
}

toggleEmojiPicker(showEmojiPicker:any): void {
  showEmojiPicker = !showEmojiPicker;
}
//Drawer
onClose() {
    if (this.matDrawer) {
      this.matDrawer.close();
      this.expandableCard= !this.expandableCard;
    }
  }
//Likes
openDialog(reactions:any): void {
    const dialogRef = this.dialog.open(LikesComponent, {
      data: {reactions:reactions,name:this.selectedPostValue.name,photo:this.selectedPostValue.photo,count:this.selectedPostValue.totalCountReactions},
      width:'600px',
      height:'auto',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
     
    });
  }
  openDialogComment(reactions:any,count:any): void {
    const dialogRef = this.dialog.open(LikesComponent, {
      data: {reactions:reactions,name:this.selectedPostValue.name,photo:this.selectedPostValue.photo,count:count},
      width:'600px',
      height:'auto',
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log('The dialog was closed');
     
    });
  }

//Reaction Post

CreateReactionPost(post:any,channelTypeName:any,channelId:any ){
 
  
     
   this.postService.CreateReactionPost(post,channelTypeName,channelId).subscribe({
           next:(resp)=>{
             if(resp.succeeded){
              console.log(JSON.stringify(this.selectedPostValue?.name)+"this.selectedPostValue")
              this.reactions.name=this.selectedPostValue?.name
              this.reactions.picture=this.selectedPostValue?.photo
              
              post.totalCountReactions=+1;
              post.isLikedByAuthor=true;
               const updatedReactionsArray = [...(this.selectedPostValue?.reactions || []),  this.reactions];

               // Create a new object with the updated comments array
               this.selectedPostValue = { ...this.selectedPostValue, reactions: updatedReactionsArray };
               console.log(JSON.stringify(this.selectedPostValue)+"this.selectedPostValue")
               // Update the selectedPost property to notify subscribers of the change
               this.selectedPostSubject.next(this.selectedPostValue);
               this._changeDetectorRef.detectChanges();
          
             }
              
 
           }})
 }
 DeleteReactionPost(post,channelTypeName:any,channelId:any ,PostIdAPI:any){
       this.postService.DeleteReactionPost(post,channelTypeName,channelId).subscribe({
         next:(resp)=>{
          post.isLikedByAuthor=false;
          post.totalCountReactions-=1;
          const postIndex = this.selectedPostValue?.reactions.findIndex(p => p.PostIdAPI === PostIdAPI);
       
          if (postIndex !== -1) {
            // Remove the comment from the comments array
            const updatedReactionsArray = [...this.selectedPostValue?.comments.slice(0, postIndex), ...this.selectedPostValue?.reactions.slice(postIndex + 1)];
  
            // Create a new object with the updated comments array
            this.selectedPostValue = {
              ...this.selectedPostValue,
              reactions: updatedReactionsArray
            };
  
     
            this.selectedPostSubject.next(this.selectedPostValue);
            this._changeDetectorRef.detectChanges();
          }
       
         }})
 }
 DeletePost(selectedpost:any ){
  this.postService.DeletePost(selectedpost).subscribe({
        next:(resp)=>{
          if(resp.succeeded){             
            this.toastr.success(resp.message , 'SNM');
          }else{
            this.toastr.success(resp.message, 'SNM');
          }
          

        }})
}


// Reaction  Comment
CreateReactionComment(comment: any,  channelTypeName: any, channelId: any, i: any) {
    this.postService.CreateReactionComment(comment, channelTypeName, channelId).subscribe({
      next: (resp) => {
        if (resp.succeeded) {
         
          this.selectedPostValue.comments[i].isLikedByAuthor = true;
          this.reactions.name = this.selectedPostValue?.name;
          this.reactions.picture = this.selectedPostValue?.photo;

          this.selectedPostValue.comments[i].likesCount=+1
          const updatedCommentsArray = [...(this.selectedPostValue?.comments[i]?.reactions || []), this.reactions];
  
          // Create a new object with the updated comments array
          this.selectedPostValue = {
            ...this.selectedPostValue,
            comments: this.selectedPostValue?.comments.map((c, index) =>
              index === i ? { ...c, reactions: updatedCommentsArray } : c
            )
          };
          console.log(JSON.stringify(this.selectedPostValue)+"this.selectedPostValue")
          this.selectedPostSubject.next(this.selectedPostValue);
          this._changeDetectorRef.detectChanges();
        }
      },
      
    error: (error) => {
      // Handle error appropriately
      console.error('Error deleting reaction for comment:', error);
    }
  });
}
DeleteReactionComment(comment: any, channelTypeName: any, channelId: any, i: any) {
    this.postService.DeleteReactionComment(comment, channelTypeName, channelId).subscribe({
      next: (resp) => {
        if (resp.succeeded) {
       
          const updatedComments = [...this.selectedPostValue.comments];
         
          console.log(this.selectedPostValue.comments[i]+comment.commentId +"comment.+':"+i)
          const reactionIndex = this.selectedPostValue.comments[i]?.reactions.findIndex(
            (r) => r.commentId === comment.commentId
          );
            console.log(reactionIndex+comment.commentId +"comment.commentId")
          // if (reactionIndex !== -1) {
            const updatedReactionsArray = [
              ...this.selectedPostValue.comments[i]?.reactions.slice(0, reactionIndex),
              ...this.selectedPostValue.comments[i]?.reactions.slice(reactionIndex + 1),
            ];
  
            this.selectedPostValue.comments[i] = {
              ...this.selectedPostValue.comments[i],
              reactions: updatedReactionsArray,
            };
  
            this.selectedPostValue = {
              ...this.selectedPostValue,
              comments: this.selectedPostValue.comments,
            };
            this.selectedPostValue.comments[i].likesCount -= 1;
            this.selectedPostValue.comments[i].isLikedByAuthor = false;
            this.selectedPostSubject.next(this.selectedPostValue);
            this._changeDetectorRef.detectChanges();
          // }
          this._changeDetectorRef.detectChanges();
      }
      },
      
    error: (error) => {
      // Handle error appropriately
      this.toastr.error('Error deleting reaction for comment:', error);
    }
  });
  
  }
  // Comment
  DeleteComment(postId:any,fromId:any,channelTypeName:any,channelId:any ,commentId:any){
 
    this.commentDetails.postId=postId
    this.commentDetails.fromId=fromId
    this.commentDetails.commentId=commentId
        this.postService.DeleteComment(this.commentDetails,channelTypeName,channelId).subscribe({
          next:(resp)=>{
            
              // Find the index of the comment in the comments array
              const commentIndex = this.selectedPostValue?.comments.findIndex(c => c.commentId === commentId);
              console.log(commentIndex+"commentIndexcommentIndex")
              if (commentIndex !== -1) {
                // Remove the comment from the comments array
                const updatedCommentsArray = [...this.selectedPostValue?.comments.slice(0, commentIndex), ...this.selectedPostValue?.comments.slice(commentIndex + 1)];
      
                // Create a new object with the updated comments array
                this.selectedPostValue = {
                  ...this.selectedPostValue,
                  comments: updatedCommentsArray
                };
      
                // Update the selectedPost property to notify subscribers of the change
                this.selectedPostSubject.next(this.selectedPostValue);
                this._changeDetectorRef.detectChanges();
              }
      
              this.commentDetails = {
                mesage: '',
                fromId: '',
                postId: '',
                photoUrl: ''
              };
      
              this.toastr.success("Comment Deleted", 'SNM');
            
          
  
          }})
  }
  UpdateComment(postId:any,fromId:any,channelTypeName:any,channelId:any,comment:any){
    comment.postId=postId
    comment.fromId=fromId
    comment.message= this.EditingComment
    this.postService.UpdateComment(comment,channelTypeName,channelId).subscribe({
      next:(resp)=>{
        if(resp.succeeded){
       
          resp.data.fromName=this.selectedPostValue?.name
          resp.data.fromPicture=this.selectedPostValue?.photo
          const updatedCommentsArray = [...(this.selectedPostValue?.comments || [])];
          
          // Replace the comment at the specified index
          if (this.editingCommentIndex !== null && this.editingCommentIndex < updatedCommentsArray.length) {
            updatedCommentsArray[this.editingCommentIndex] = resp.data;
          }
  
          // Create a new object with the updated comments array
          this.selectedPostValue = { ...this.selectedPostValue, comments: updatedCommentsArray };
          
          // Update the selectedPost property to notify subscribers of the change
          this.selectedPostSubject.next(this.selectedPostValue);
          this._changeDetectorRef.detectChanges();
          this.commentDetails={   
            commentId:'',
            mesage:'',  
            fromId:'',
            postId:'', 
            photoUrl:''
          }
          this.isEditingComment = false;
          this.editingCommentIndex = null;
          this.EditingComment = '';
        
          // window.location.reload();
          
            this.toastr.success("comment Add", 'SNM');
        }else{
          this.toastr.success(resp.message, 'SNM');
        }
        
  
      }})
  }
  CreateComment(postId:any,fromId:any,channelTypeName:any,channelId:any ){
    this.commentDetails.postId=postId
    this.commentDetails.fromId=fromId
    this.postService.createComment(this.commentDetails,channelTypeName,channelId).subscribe({
          next:(resp)=>{
            if(resp.succeeded){
              resp.data.fromName=this.selectedPostValue?.name
              resp.data.fromPicture=this.selectedPostValue?.photo
              resp.data.likesCount=0
              const updatedCommentsArray = [...(this.selectedPostValue?.comments || []), resp.data];
              this.selectedPostValue = { ...this.selectedPostValue, comments: updatedCommentsArray };       
              this.selectedPostSubject.next(this.selectedPostValue);
              this._changeDetectorRef.detectChanges();
              this.commentDetails={   
                commentId:'',
                mesage:'',  
                fromId:'',
                postId:'', 
                photoUrl:''
              }
                this.toastr.success("comment Add", 'SNM');
            }
            else{
              console.log(resp.message+"snm")
              this.toastr.error(resp.message, 'SNM');
            }
            

          }})
}
//EmojiCommentUpdate
  addEmojiUpdate(event): void {
 
  
    const text = `${this.EditingComment} ${event.emoji.native}`;
    this.EditingComment = text;
   
    console.log(this.EditingComment+"emoji"+text)
    this.showEmojiPickerUpdate = false;
  }
  toggleEmojiUpdate(showEmojiPickerUpdate:any): void {
    this.showEmojiPickerUpdate = !showEmojiPickerUpdate;
  }
  toggleEditComment(index:any,message:any){
    this.isEditingComment = !this.isEditingComment;
    this.editingCommentIndex = this.isEditingComment ? index : null;
    this.EditingComment = this.isEditingComment ? message : '';
     
  }
  cancel(){
    this.isEditingComment = false;
    this.editingCommentIndex =  null;
    this.EditingComment = '';
  }
 //EmojiComment 
  addEmojiComment(event): void {
 
    const text = `${this.commentDetails?.message} ${event.emoji.native}`;
    this.commentDetails.message = text;
   
    console.log(this.commentDetails.message+"emoji")
    this.showEmojiPickerComment = false;
  }
  toggleEmojiPickerComment ():void {
    this.showEmojiPickerComment = !this.showEmojiPickerComment;
  }
  
}

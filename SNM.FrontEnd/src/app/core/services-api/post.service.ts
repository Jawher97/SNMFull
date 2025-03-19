import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, catchError, map, Observable, of, switchMap, take, tap, throwError } from 'rxjs';
import { cloneDeep } from 'lodash-es';
import { Posts } from '../models/posts.model';
import { environment } from 'environments/environment.development';
import { LinkedinPost } from '../models/linkedin-post';
import { LinkedInService } from './linkedin.service';
import { GenericPublishingPosts } from '../models/genericPublishingPosts.model';
import { ToastrService } from 'ngx-toastr';
@Injectable({
    providedIn: 'root'
})
export class PostService {
    // Private
    private _post: BehaviorSubject<Posts | null> = new BehaviorSubject(null);
    private _linkedinpost: BehaviorSubject<LinkedinPost | null> = new BehaviorSubject(null);
    private _posts: BehaviorSubject<Posts[] | null> = new BehaviorSubject(null);
    private _linkedinposts: BehaviorSubject<LinkedinPost[] | null> = new BehaviorSubject(null);
    private publishingAgrgregator_Path;
    private Comment_Path;
    private Reaction_Path;


    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient, private linkedinservice: LinkedInService,  private toastr :ToastrService) {
        // Set the private defaults
        this.publishingAgrgregator_Path = environment.publishingAgrgregatorURL;
        this.Comment_Path=environment.Comment_Path;
        this.Reaction_Path=environment.Reaction_Path;
    }
    

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------


    /**
     * Getter for posts
     */
    get posts$(): Observable<Posts[]> {
        return this._posts.asObservable();
    }

    /**
     * Getter for post
     */
    get post$(): Observable<Posts> {
        return this._post.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // ----------------------------------------------------------------------------------------------------- 

    /**
     * Create post
     *
     * @param post
     */
    publishPost(post: GenericPublishingPosts): Observable<any> {
        return this._httpClient.post<GenericPublishingPosts>(this.publishingAgrgregator_Path + 'PublishPost', post).pipe(
            catchError((error) => {
              let errorMessage = 'An error occurred';
            
                errorMessage = error.error.Message;
              
              
              this.toastr.error(error.message, 'SNM');
              return throwError(errorMessage);
            })
          );;
    }
    createComment(comment:any,channelTypeName:any,channelId:any): Observable<any> {
      
       console.log(this.Comment_Path+comment)
        return this._httpClient.post<any>(this.Comment_Path+"/CommentPost" , comment,{ params: {'channelTypeName': channelTypeName,'channelId': channelId} }).pipe(
            catchError((error) => {
              let errorMessage = 'An error occurred';
            
                errorMessage = error.error.Message;
              
              
              this.toastr.error(error.message, 'SNM');
              return throwError(errorMessage);
            })
          );;
    

    }
    DeleteComment(comment:any,channelTypeName:any,channelId:any): Observable<any> {
      console.log(this.Comment_Path+comment)
      
       return this._httpClient.post<any>(this.Comment_Path+"/DeleteComment" , comment,{ params: {'channelTypeName': channelTypeName,'channelId': channelId} }).pipe(
           catchError((error) => {
             let errorMessage = 'An error occurred';
           
               errorMessage = error.error.Message;
             
             
             this.toastr.error(error.message, 'SNM');
             return throwError(errorMessage);
           })
         );;
   

   }
   
   CreateSubComment(comment:any,channelTypeName:any,channelId:any): Observable<any> {
      
    console.log(this.Comment_Path+comment)
     return this._httpClient.post<any>(this.Comment_Path+"/CreateSubComment" , comment,{ params: {'channelTypeName': channelTypeName,'channelId': channelId} }).pipe(
         catchError((error) => {
           let errorMessage = 'An error occurred';
         
             errorMessage = error.error.Message;
           
           
           this.toastr.error(error.message, 'SNM');
           return throwError(errorMessage);
         })
       );;
 

 }
 UpdateComment(comment:any,channelTypeName:any,channelId:any): Observable<any> {
      
  console.log(this.Comment_Path+comment)
   return this._httpClient.put<any>(this.Comment_Path+"/UpdateComment" , comment,{ params: {'channelTypeName': channelTypeName,'channelId': channelId} }).pipe(
       catchError((error) => {
         let errorMessage = 'An error occurred';
       
           errorMessage = error.error.Message;
         
         
         this.toastr.error(error.message, 'SNM');
         return throwError(errorMessage);
       })
     );;
}
CreateReactionComment(comment:any,channelTypeName:any,channelId:any){
  console.log(this.Reaction_Path+JSON.stringify(comment))
        return this._httpClient.post<any>(this.Reaction_Path+"/CreateReactionComment" , comment,{ params: {'channelTypeName': channelTypeName,'channelId': channelId} }).pipe(
            catchError((error) => {
              let errorMessage = 'An error occurred';
            
                errorMessage = error.error.Message;
              
              
              this.toastr.error(error.message, 'SNM');
              return throwError(errorMessage);
            })
          );;
}
DeleteReactionComment(comment:any,channelTypeName:any,channelId:any){
  console.log(this.Reaction_Path+comment)
        return this._httpClient.post<any>(this.Reaction_Path+"/DeleteReactionComment" , comment,{ params: {'channelTypeName': channelTypeName,'channelId': channelId} }).pipe(
            catchError((error) => {
              let errorMessage = 'An error occurred';
            
                errorMessage = error.error.Message;
              
              
              this.toastr.error(error.message, 'SNM');
              return throwError(errorMessage);
            })
          );;
}
DeleteReactionPost(post:any,channelTypeName:any,channelId:any){
  console.log(this.Reaction_Path+post)
        return this._httpClient.post<any>(this.Reaction_Path+"/DeleteReactionPost" , post,{ params: {'channelTypeName': channelTypeName,'channelId': channelId} }).pipe(
            catchError((error) => {
              let errorMessage = 'An error occurred';
            
                errorMessage = error.error.Message;
              
              
              this.toastr.error(error.message, 'SNM');
              return throwError(errorMessage);
            })
          );;
}
CreateReactionPost(post:any,channelTypeName:any,channelId:any){
  console.log(this.Reaction_Path+post)
  return this._httpClient.post<any>(this.Reaction_Path+"/CreateReactionPost" , post,{ params: {'channelTypeName': channelTypeName,'channelId': channelId} }).pipe(
      catchError((error) => {
        let errorMessage = 'An error occurred';
      
          errorMessage = error.error.Message;
        
        
        this.toastr.error(error.message, 'SNM');
        return throwError(errorMessage);
      })
    )}

    DeletePost(post:any){
      console.log(this.Reaction_Path+post)
        return this._httpClient.post<any>(this.publishingAgrgregator_Path + 'DeletePost' , post,{ params: {'channelTypeName': post.channelTypeName,'channelId':  post.channelId} }).pipe(
            catchError((error) => {
              let errorMessage = 'An error occurred';
            
                errorMessage = error.error.Message;
              
              
              this.toastr.error(error.message, 'SNM');
              return throwError(errorMessage);
            })
          );;
    }
}

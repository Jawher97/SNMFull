import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment, facebookCredentials,facebookPaths } from 'environments/environment.development';
import { BehaviorSubject, EMPTY, from, Observable, of, switchMap, take } from 'rxjs';
import { map, concatMap, finalize, tap } from 'rxjs/operators';
import { FacebookChannelPost } from '../models/facebookChannelPost';
import { Post } from '../models/post.model';
import { Account } from '../types/account';
import { ChannelProfile } from '../models/ChannelProfile.model';
const facebookAPIPath = `${environment.facebookAPIURL}`;


@Injectable({
  providedIn: 'root'
})
export class FacebookService {
  // Private
  private accountSubject: BehaviorSubject<Account | null>;
  public account: Observable<Account | null>;
  private _post: BehaviorSubject<Post | null> = new BehaviorSubject(null);
  private _posts: BehaviorSubject<Post[] | null> = new BehaviorSubject(null);

  private facebookPath = `${facebookPaths.facebookAPIURL}`;
  private _latestFacebookPost: BehaviorSubject<any[] | null> = new BehaviorSubject(null);
  private _FacebookSummaryt: BehaviorSubject<any[] | null> = new BehaviorSubject(null);
  
  private _latestFacebookComment: BehaviorSubject<any[] | null> = new BehaviorSubject(null);

  private _facebookProfile: BehaviorSubject<ChannelProfile | null> = new BehaviorSubject(null);
  // Observable navItem source
  private _authNavStatusSource = new BehaviorSubject<boolean>(false);
  // Observable navItem stream
  authNavStatus$ = this._authNavStatusSource.asObservable();
  private url = `https://www.facebook.com/v17.0/dialog/oauth?client_id=${facebookCredentials.facebookClientId}&redirect_uri=${facebookCredentials.facebookRedirectUri}&scope=${facebookCredentials.facebookScope}&response_type=code`;
  private getPost:any;
 private getCommentFacebook:any;
 private GetFacebookSummary:any;
  authenticateTimeout: any;
  
  

  /**
   * Constructor
   */
  constructor(private _httpClient: HttpClient) {
   
    // Set the private defaults
    this.accountSubject = new BehaviorSubject<Account | null>(null);
    this.account = this.accountSubject.asObservable();
    this.getPost=environment.getPostfacebook;
    this.getCommentFacebook=environment.getCommentFacebook
    this.GetFacebookSummary=environment.GetFacebookSummary
    
  }
  // -----------------------------------------------------------------------------------------------------
  // @ Accessors
  // -----------------------------------------------------------------------------------------------------


  /**
   * Getter for labels
   */
  // get labels$(): Observable<Label[]>
  // {
  //     return this._labels.asObservable();
  // }

  /**
   * Getter for posts
   */
  get posts$(): Observable<Post[]> {
    return this._posts.asObservable();
  }
  /**
       * Getter for post
       */
  get post$(): Observable<Post> {
    return this._post.asObservable();
  }
   /**
       * Getter for post
       */
  get latestFacebookPost$(): Observable<any[]> {
    return this._latestFacebookPost.asObservable();
  }
  get  facebookSummaryt$(): Observable<any[]> {
    return this. _FacebookSummaryt.asObservable();
  }
 
  get latestFacebookComment$(): Observable<any[]> {
    return this._latestFacebookComment.asObservable();
  }
  
  // -----------------------------------------------------------------------------------------------------
  // @ Public methods
  // -----------------------------------------------------------------------------------------------------
  
  // getLatestPost(memberUrn: string): Observable<LinkedinPost> {
  //   const accessToken = localStorage.getItem('LinkedIn access_token');
  //   const params = new HttpParams()
  //   .set('accessToken', accessToken)
  //   .set('memberUrn', memberUrn);
    
  
  //   return this.http.get<LinkedinPost>(this.latestPostUrl, {params: params}).pipe(
      
  //     tap(response => console.log('Response:', response))
  //   );
  //  }
  /**
     * Get all messages
     */
  getLatestFacebookPost(channelId:string): Observable<any> {
   
    return this._httpClient.get<any>(this.getPost, { params: {"channelId":channelId }}).pipe(
  
        tap((facebookPost: any[]) => {
            this._latestFacebookPost.next(facebookPost);
        })
    );
   
}
  getFacebookSummary(channelId:string): Observable<any> {
   
  return this._httpClient.get<any>(this.GetFacebookSummary+"/GetFacebookSummary", { params: {"channelId":channelId }}).pipe(

      tap((FacebookSummaryt: any[]) => {
          this._FacebookSummaryt.next(FacebookSummaryt);
      })
  );
 
}
getFacebookGroupSummary(channelId: string): Observable<any> {
  return this._httpClient.get<any>(this.GetFacebookSummary+"/GetFacebookGroupSummary", { params: {"channelId":channelId }}).pipe(

    tap((FacebookSummaryt: any[]) => {
        this._FacebookSummaryt.next(FacebookSummaryt);
    })
);
}
getLatestFacebookComment(PostAPIId:any,channelId:string): Observable<any> {
   
  return this._httpClient.get<any>(this.getCommentFacebook, { params: {"PostAPIId":PostAPIId,"channelId":channelId }}).pipe(

      tap((facebookComment: any[]) => {
          this._latestFacebookComment.next(facebookComment);
      })
  );
    }
   
 /**
     * Get LinkedIn Posts
     */
//  getLinkedInPosts(): Observable<LinkedinPost[]>
//  {
//      const accesstoken = localStorage.getItem('LinkedIn access_token');
//      const memberUrn = localStorage.getItem('org_urn');

//      const params = new HttpParams()
//      .set('accessToken', accesstoken)
//      .set('memberUrn', memberUrn);

//      return this._httpClient.get<LinkedinPost[]>(this.getPostsUrl+"GetLinkedinPosts", {params: params}).pipe(
//          tap((response: LinkedinPost[]) => {
//              this._linkedinposts.next(response);
//          })
//      );
//  }

  login() {
    // login with facebook then the API to get a JWT auth token
    // return this.loginFacebook().pipe(
    //     concatMap(accessToken => this.loginApi(accessToken))
    // );
    //https://www.facebook.com/v17.0/dialog/oauth?
    //response_type=token
    //&display=popup
    //&client_id=807360224265636
    //&redirect_uri=https%3A%2F%2Fdevelopers.facebook.com%2Ftools%2Fexplorer%2Fcallback
    //&auth_type=rerequest
    //&scope=pages_show_list%2Cpages_read_engagement%2Cpages_manage_posts
    return this.loginFacebook().pipe(
    //     concatMap(accessToken => this.loginApi(accessToken))
    map((accessToken) => {
      // Return the Access Token from observable
      console.log("Facebook Page Access token: "+accessToken);
      
      // this.getFacebookPages(accessToken);
      
      return accessToken;
    })
    );
}

loginFacebook() {
    // login with facebook and return observable with fb access token on success
    // const fbLoginPromise = new Promise<fb.StatusResponse>(resolve => FB.login(resolve , { scope: 'public_profile,email,pages_show_list,pages_manage_posts,pages_read_engagement' }));
    const fbLoginPromise = new Promise<fb.StatusResponse>(resolve => FB.login(resolve , { scope: 'public_profile,email,pages_show_list,pages_manage_posts,pages_read_engagement' }));
    return from(fbLoginPromise).pipe(
        concatMap(({ authResponse }) => authResponse ? of(authResponse.accessToken) : EMPTY)
    );
}
FacebookPagesLogin(){
  // login with facebook then the API to get a JWT auth token
  // return this.loginFacebook().pipe(
  //     concatMap(accessToken => this.loginApi(accessToken))
  // );
  
//  let url="https://www.facebook.com/v17.0/dialog/oauth?"
//   +"response_type=code"
//   +"&display=popup"
//   +"&client_id=526589669666269"
//   +"&redirect_uri=https://localhost:4200/facebookcallback/"
//   +"&auth_type=rerequest"
//   +"&scope=pages_show_list%2Cpages_read_engagement%2Cpages_manage_posts"
  // return window.open(url, 'Instagram Auth', features);
  // return this.connectFacebookGroup().pipe(
  // //     concatMap(accessToken => this.loginApi(accessToken))
  // map((accessToken) => {
  //   // Return the Access Token from observable
  //   console.log("Facebook Group access token: "+accessToken);
    
  //   this.getFacebookPages(accessToken);
    
  //   return accessToken;
  // })
  // );
  var w = 500;
  var h = 600;
  var left = Number((screen.width/2)-(w/2));
  var tops = Number((screen.height/2)-(h/2));
 // const authWindow = window.open(`${this.url}`, 'Instagram Auth', 'toolbar=no, location=no, directories=no, status=no, menubar=no, scrollbars=no, resizable=no, copyhistory=no, width='+w+', height='+h+', top='+top+', left='+left);
  const param= {
    url:this.url, width:"500", height:"600"
  };
  this.openCenteredWindow(param);
}
openCenteredWindow({url, width, height}) {
  const pos = {
      x: (screen.width / 2) - (width / 2),
      y: (screen.height/2) - (height / 2)
  };

  const features = `width=${width} height=${height} left=${pos.x} top=${pos.y}`;

  return window.open(url, 'Facebook Page Auth', features);
}
generateAccessToken(code: string, brandId:string,name:string) : Observable<any> {
  console.log("Tokennnnnnn " +code +brandId)
        return this._httpClient.get(this.facebookPath+"GenerateFacebookAccessToken", { params: { 'code': code ,'brandId':brandId,"name":name} }).pipe(
          tap(response => {
            
         
          console.log(response+"rrr")
       
          this._facebookProfile.next(response)
           
        }));

 
}
generateAccessTokenForGroups(code: string, brandId:string,name:string) : Observable<any> {
  console.log("Tokennnnnnn " +code +brandId)
        return this._httpClient.get(this.facebookPath+"GenerateFacebookAccessToken", { params: { 'code': code ,'brandId':brandId,"name":name} }).pipe(
          tap(response => {
            
         
          console.log(response+"rrr")
       
          this._facebookProfile.next(response)
           
        }));}

connectFacebookGroup() {
  // login with facebook and return observable with fb access token on success
  // const fbLoginPromise = new Promise<fb.StatusResponse>(resolve => FB.login(resolve , { scope: 'public_profile,email,user_managed_groups,groups_show_list,business_management,publish_to_groups/public_profile,email,pages_show_list,pages_manage_posts,pages_read_engagement' }));
  const fbLoginPromise = new Promise<fb.StatusResponse>(resolve => FB.login(resolve , { scope: 'groups_access_member_info' }));
  return from(fbLoginPromise).pipe(
      concatMap(({ authResponse }) => authResponse ? of(authResponse.accessToken) : EMPTY)
  );
}

  /**
   * Publish to facebook
   */
  publish(facebookChannelPost: any): Observable<FacebookChannelPost> {

    console.log(facebookChannelPost);

    return this.posts$.pipe(
      take(1),
      switchMap(posts => this._httpClient.post<any>(facebookAPIPath + 'GenerateFacebookAccessToken', facebookChannelPost).pipe(
        map((newBrand) => {

          // Update the posts with the new post
          //this._posts.next([...posts, newBrand]);

          // Return the new post from observable
          return newBrand;
        })
      ))
    );
  }


  // helper methods
  facebookLogin(accessToken:string) {
    let headers = new HttpHeaders ();
 
    headers.append('Content-Type', 'application/json');
    let body = JSON.stringify({ accessToken });  
    console.log(body);
    
    // return this.handleError;
  }

}

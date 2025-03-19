import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { LinkedinProfileData } from '../models/linkedin-profile-data';
import { environment, linkedInCredentials, linkedInPaths } from 'environments/environment.development';
import { BehaviorSubject, Observable, ReplaySubject, map, tap } from 'rxjs';
import { LinkedInChannel } from '../models/linkedinChannel';
import { LinkedInChannelDto } from '../models/brand.model';
import { boards } from 'app/mock-api/apps/scrumboard/data';
import { LinkedinPost } from '../models/linkedin-post';
import { LinkedinComment } from '../models/linkedin-comments';
import { LinkedinReaction } from '../models/LinkedIn/linkedin-reaction.model';
import { ChannelProfile } from '../models/ChannelProfile.model';

const linkedInPath = `${linkedInPaths.linkedInAuthAPIURL}`;
const linkedInOAuth = `${linkedInPaths.linkedinOAuthURL}`;
const linkedInAPI = `${linkedInPaths.linkedinAPIURL}`;

@Injectable({
  providedIn: 'root',
})
export class LinkedInService {

  notificationMessage = new EventEmitter();

  private tokenUrl = linkedInPath + 'AccessToken/AccessToken'; // URL de votre API de génération de token
  private url = linkedInPath + 'GetOrgDetails/GetOrgDetails';
  private deletePostUrl = linkedInPath + 'DeletePost/DeletePost'
  private createCommentUrl = linkedInPath + 'CreateComment/CreateComment'
  private createSubCommentUrl = linkedInPath + 'CreateSubComment/CreateSubComment'
  private reshareUrl = linkedInPath + 'CreateReshare/ResharePost'
  private likeUrl = linkedInPath + 'CreateReaction/CreateReaction'
  private unlikeUrl = linkedInPath + 'DeleteReaction/DeleteReaction'
  private editCommentUrl = linkedInPath + 'EditComment/EditComment'
  private DeleteCommentUrl = linkedInPath + 'DeleteComment/DeleteComment'
  private EditPostUrl = linkedInPath + 'EditPost/EditPost'
  private publishUrl = linkedInPath + 'PublishToLinkedIn/PublishToLinkedIn'
  private disableCommentsUrl = linkedInPath + 'DisableComments/DisableComments'

  private _linkedinChannels: BehaviorSubject<LinkedInChannelDto[] | null>;

  private _linkedinProfile: BehaviorSubject<ChannelProfile | null> = new BehaviorSubject(null);

  public code: BehaviorSubject<string | null> = new BehaviorSubject(null);


  private authUrl = linkedInOAuth + `${linkedInCredentials.linkedInClientId}&redirect_uri=${linkedInCredentials.linkedInRedirectUrl}&state=foobar&scope=${linkedInCredentials.linkedInScope}`; // URL de votre API d'autorisation LinkedIn
  private latestPostUrl = linkedInAPI + 'GetLatestPost';
  private commentsUrl = linkedInAPI + 'RetrieveComments';
  private subCommentsUrl = linkedInAPI + 'RetrieveSubComments';
  private postsUrl = linkedInAPI + 'PublishToLinkedIn';
  authWindow: Window;
  private latestLinkedinPost:any
  private latestLinkedinComment:any
  private _latestLinkedInPost: BehaviorSubject<any[] | null> = new BehaviorSubject(null);
  private _latestLinkedInComment: BehaviorSubject<any[] | null> = new BehaviorSubject(null);
  
  constructor(private http: HttpClient) {
    this.latestLinkedinPost=environment.latestLinkedinPost
    this.latestLinkedinComment=environment.latestLinkedinComment
   }
  // -----------------------------------------------------------------------------------------------------
  // @ Accessors
  // -----------------------------------------------------------------------------------------------------

  /**
     * Getter for posts
     */
  get linkedinProfile$(): Observable<ChannelProfile> {
    return this._linkedinProfile.asObservable();
  }

  login() {
    //  const authWindow = window.open(`${this.authUrl}`, 'LinkedIn Auth', 'width=500,height=600');

    let url = `https://www.linkedin.com/oauth/v2/authorization?response_type=code&state=true&client_id=${linkedInCredentials.linkedInClientId}&redirect_uri=${linkedInCredentials.linkedInRedirectUrl}&scope=${linkedInCredentials.linkedInScope}`;
    //  this.authWindow = window.open(url, 'LinkedIn Auth', 'width=500,height=600');
    this.authWindow = window.open(`https://www.linkedin.com/oauth/v2/authorization?response_type=code&state=true&client_id=${linkedInCredentials.linkedInClientId}&redirect_uri=${linkedInCredentials.linkedInRedirectUrl}&scope=${linkedInCredentials.linkedInScope}`, '_blank', 'toolbar=0,status=0,width=626,height=436');

    // this.closeWindow()
    // authWindow.opener.document.getElementById("demo").innerHTML = "HELLO!";
    // authWindow.onbeforeunload = () => {
    //   //Do something
    //   authWindow.opener.document.getElementById("demo").innerHTML = "HELLO! onbeforeunload";
    // }
    // if (authWindow) {

    //   authWindow.onbeforeunload = () => {
    //     //Do something
    //   }
    // }

    // window.location.href = `https://www.linkedin.com/oauth/v2/authorization?response_type=code&state=true&client_id=${
    //   environment.LINKEDIN_API_KEY}&redirect_uri=${environment.LINKEDIN_REDIRECT_URL}&scope=r_liteprofile%20r_emailaddress`;
  }
  get latestLinkedInPost$(): Observable<any[]> {
    return this._latestLinkedInPost.asObservable();
  }
  get latestLinkedInComment$(): Observable<any[]> {
    return this._latestLinkedInComment.asObservable();
  }
  
  createLinkedInChannel(linkedinChannel:LinkedInChannel)
  {

  }
  logaccessToken = (data: any) => {
    console.log("Data from popup: " + data);

  }
  getLatestLinkedInPost(channelId:string): Observable<any> {
   
    return this.http.get<any>(this.latestLinkedinPost, { params: {"channelId":channelId }}).pipe(
  
        tap((linkedInPost: any[]) => {
            this._latestLinkedInPost.next(linkedInPost);
        })
    );
   
}
getLatestLinkedinComment(PostAPIId:any,channelId:string): Observable<any> {
   
  return this.http.get<any>(this.latestLinkedinComment, { params: {"PostAPIId":PostAPIId,"channelId":channelId }}).pipe(

      tap((linkedinComment: any[]) => {
          this._latestLinkedInComment.next(linkedinComment);
      })
  );
}
  /**
   * Getter for brand
   */
  get code$(): Observable<string> {
    return this.code.asObservable();
  }
  closeWindow(code: string) {
    console.log(code)
    window.opener.document.getElementById("demo").innerHTML = code;
    window.close();
  }

  generateAccessToken(code: string, brandId:string): Observable<any> {

    return this.http.get<ChannelProfile>(linkedInPath + 'GenerateProfileAccessToken/GenerateProfileAccessToken', { params: { code: code,brandId:brandId } }).pipe(
      tap(response => {

        this._linkedinProfile.next(response)
        // if (response && response.length > 0) {
        //   const org_urn = response[0].companyUrn;
        //   localStorage.setItem('org_urn', org_urn);
        // }

      })
      // Vérifiez si les données de réponse sont correctes
    );

    // return this.http.get(linkedInPath + 'GenerateProfileAccessToken/GenerateProfileAccessToken', { params: { 'code': code} }).pipe(
    //   tap((response: LinkedinProfileData) => {
    //     this._linkedinProfile.next(response);
    //     console.log("LinkedinProfileData"+JSON.stringify(response));


    //   })
    // );
  }
  generateLinkedinAccessToken(code: string) {

    this.http.get(this.tokenUrl, { params: { 'code': code } }).subscribe((response: LinkedinProfileData) => {

      const accessToken = response.access_token;
      const refreshToken = response.refresh_token;
      const person = response.linkedinUrn;
      // Stocker l'access token localement
      localStorage.setItem('LinkedIn access_token', accessToken);
      localStorage.setItem('LinkedIn refresh_token', refreshToken);
      localStorage.setItem('person', person);

      this.getCompanyPages();

      window.close();


    });
  }


  getCompanyPages(): Observable<LinkedInChannelDto[]> {

    const accessToken = localStorage.getItem('LinkedIn access_token'); // Récupérer l'access token depuis le stockage local

    return this.http.get<LinkedInChannelDto[]>(this.url, { params: { accessToken: accessToken } }).pipe(
      tap(response => {
        console.log('Pages:', response);
        if (response && response.length > 0) {
          const org_urn = response[0].companyUrn;
          localStorage.setItem('org_urn', org_urn);
        }

      })
      // Vérifiez si les données de réponse sont correctes
    );

  }


  getLatestPost(memberUrn: string): Observable<LinkedinPost> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accessToken', accessToken)
      .set('memberUrn', memberUrn);


    return this.http.get<LinkedinPost>(this.latestPostUrl, { params: params }).pipe(

      tap(response => console.log('Response:', response))
    );
  }




  publishLinkedInPost(post: LinkedinPost): Observable<LinkedinPost> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const org_urn = localStorage.getItem('org_urn');

    const fd = new FormData();
    fd.append('AccessToken', accessToken);
    fd.append('Message', post.message);
    fd.append('author_urn', org_urn);


    const reqBody = { "AccessToken": accessToken, "Message": post.message, "author_urn": org_urn }

    return this.http.post<LinkedinPost>(this.publishUrl, reqBody)
  }


  publishLinkedInProfilePost(post: LinkedinPost): Observable<LinkedinPost> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const person = localStorage.getItem('person');


    const reqBody = { "AccessToken": accessToken, "Message": post.message, "author_urn": person }

    return this.http.post<LinkedinPost>(this.postsUrl, reqBody)
  }



  editPost(message: string, postId: string): Observable<LinkedinPost> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accessToken', accessToken)
      .set('message', message)
      .set('postId', postId);

    return this.http.put<LinkedinPost>(this.EditPostUrl, null, { params: params })
  }


  resharePost(postId: string, author_urn: string, comment: string): Observable<LinkedinPost> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accessToken', accessToken)
      .set('author_urn', author_urn)
      .set('shareId', postId)
      .set('message', comment);

    return this.http.post<LinkedinPost>(this.reshareUrl, { params: params })
  }


  deletePost(postId: string) {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accessToken', accessToken)
      .set('postId', postId);

    return this.http.delete(this.deletePostUrl, { params: params }).subscribe(() => console.log('Delete successful'));
  }





  like(entityUrn: string, orgId: string): Observable<{}> {

    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accessToken', accessToken)
      .set('authorUrn', orgId)
      .set('entityUrn', entityUrn)

    return this.http.post<{}>(linkedInAPI + 'CreateReaction', null, { params: params })

  }
  createReaction(reaction: LinkedinReaction): Observable<{}> {

    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('reaction', accessToken)

    return this.http.post<{}>(linkedInAPI + 'CreateReaction', reaction)

  }



  unlike(entityUrn: string, author_urn: string): Observable<{}> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accessToken', accessToken)
      .set('authorUrn', author_urn)
      .set('entityUrn', entityUrn)

    return this.http.delete(this.unlikeUrl, { params: params })

  }




  getComments(entityUrn: string): Observable<LinkedinComment[]> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('entityUrn', entityUrn)
      .set('accesstoken', accessToken);

    return this.http.get<LinkedinComment[]>(this.commentsUrl, { params: params }).pipe(
      tap(response => console.log('Response:', response))
    )
  }


  getSubComments(entityUrn: string): Observable<LinkedinComment[]> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('entityUrn', entityUrn)
      .set('accesstoken', accessToken);

    return this.http.get<LinkedinComment[]>(this.subCommentsUrl, { params: params }).pipe(
      tap(response => console.log('Response:', response))
    )
  }



  CreateComment(entityUrn: string, reply: string, orgId: string): Observable<LinkedinComment> {   //A compléter
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accesstoken', accessToken)
      .set('comment', reply)
      .set('author_urn', orgId)
      .set('entityUrn', entityUrn);

    return this.http.post<LinkedinComment>(this.createCommentUrl, null, { params: params })

  }

  CreateSubComment(entityUrn: string, reply: string, orgId: string): Observable<LinkedinComment> {   //A compléter
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accesstoken', accessToken)
      .set('comment', reply)
      .set('author_urn', orgId)
      .set('commentUrn', entityUrn);

    return this.http.get<LinkedinComment>(this.createSubCommentUrl, { params: params })
  }



  editComment(commentId: string, message: string, actorUrn: string, shareUrn: string): Observable<LinkedinComment> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accessToken', accessToken)
      .set('message', message)
      .set('shareUrn', shareUrn)
      .set('commentId', commentId)
      .set('actorUrn', actorUrn);

    return this.http.put<LinkedinComment>(this.editCommentUrl, null, { params: params })
  }


  DeleteComment(commentId: string, actorUrn: string, shareUrn: string): Observable<{}> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accessToken', accessToken)
      .set('shareUrn', shareUrn)
      .set('actorUrn', actorUrn)
      .set('commentId', commentId);

    return this.http.delete(this.DeleteCommentUrl, { params: params })
  }

  disableComments(actorUrn: string, shareUrn: string): Observable<{}> {
    const accessToken = localStorage.getItem('LinkedIn access_token');
    const params = new HttpParams()
      .set('accessToken', accessToken)
      .set('shareUrn', shareUrn)
      .set('actorUrn', actorUrn);

    return this.http.post(this.disableCommentsUrl, null, { params: params })

  }



  IsloggedIn() {
    return !!localStorage.getItem('LinkedIn access_token');
  }


}

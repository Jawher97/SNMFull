import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, Observable } from 'rxjs';
import { switchMap, take, map, tap } from 'rxjs/operators';
import { InstagramChannelPost } from '../models/instagrampost';
import { InstagramComments } from '../models/instagram-comments';
import { Instagraminsight } from '../models/instagraminsight';
import { environment, instagramCredentials,instagramPaths } from 'environments/environment.development';
import { ChannelProfile } from '../models/ChannelProfile.model';



@Injectable({
  providedIn: 'root'
})
export class InstagramService {
 token : string = 'IGQWRNQmpreS1WRVNQTXY4dUR0TUh3UmV6djBLRkZA4Y3c4b21aVGhZAd3RSend2c0tnUEJZAYXRHSlh0OFpBRzdFSFN3dlNFSjc3ZAzJZAbVBxNkIzVElFNEF2S0dEb2RCTHdJbFUzNkxVektVdwZDZD'
 private commentsUrl = `https://localhost:44310/api/v1/Instagram/comments`;
 private createCommentUrl =`https://localhost:44310/api/v1/Instagram/repliesinsta`;
 private repliesUrl=` https://localhost:44310/api/v1/Instagram/replies`;
 private deleteUrl = `https://localhost:44310/api/v1/Instagram/deletecomment`;
 private likesUrl = `https://localhost:44310/api/v1/Instagram/commentlike`;
 private _instagramProfile: BehaviorSubject<ChannelProfile | null> = new BehaviorSubject(null);
private instagramPath = `${instagramPaths.instagramAPIURL}`;
private _latestInstagramPost: BehaviorSubject<any | null> = new BehaviorSubject(null);
private _InstagramSummary: BehaviorSubject<any | null> = new BehaviorSubject(null);

private latestInstagramPost:any
private _latestInstagramComment: BehaviorSubject<any | null> = new BehaviorSubject(null);

private latestInstagramComment:any;
GetInstagramSummary:any;
 constructor(private http: HttpClient) { 
this.latestInstagramPost=environment.latestInstagramPost
this.latestInstagramComment=environment.latestInstagramComment
this.GetInstagramSummary=environment.InstagramSummary;

 }

  
  
  private tokenUrl = '';
  // private url = `https://api.instagram.com/oauth/authorize?client_id=${instagramCredentials.instagramClientId}&redirect_uri=${instagramCredentials.instagramRedirectUri}&response_type=code&scope=${instagramCredentials.instagramScope}`;
     private url = `https://api.instagram.com/oauth/authorize?client_id=${instagramCredentials.instagramClientId}&redirect_uri=${instagramCredentials.instagramRedirectUri}&scope=${instagramCredentials.instagramScope}&response_type=code`;
 

  login() {
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

    return window.open(url, 'Instagram Auth', features);
}
  generateAccessToken(code: string,brandId:string) : Observable<any> {

   return  this.http.get(this.instagramPath,  { params: { 'code': code ,'brandId':brandId} }).pipe(
    tap(response => {
      
   
    console.log(response+"rrr")
 
    this._instagramProfile.next(response)
     
  }));

 
   }
  //getterposts
  get posts$() {

    let params = new HttpParams().set('access_token', this.token);
    return this.http.get('https://localhost:44310/api/v1/Instagram/GetInstagramMedia', { params: params })
  }




 

  //publishtoinstagram
  publishPhoto(imageUrl: string, caption: string): Observable<any> {
    const postData = {
      image_url: imageUrl,
      caption: caption
    };

    return this.http.post('https://localhost:44310/api/v1/InstagramPost/InstagramPost', postData);
  }
  get latestInstagramPost$(): Observable<any[]> {
    return this._latestInstagramPost.asObservable();
  }
    get latestInstagramComment$(): Observable<any[]> {
    return this._latestInstagramComment.asObservable();
  }
  
  getLatestInstagramPost(channelId:string): Observable<any> {
   
    return this.http.get<any>(this.latestInstagramPost, { params: {"channelId":channelId }}).pipe(
  
        tap((InstagrmaPost: any) => {
            this._latestInstagramPost.next(InstagrmaPost);
        })
    );
   
}
getInstagramSummary(channelId:string): Observable<any> {
   
  return this.http.get<any>(this.GetInstagramSummary, { params: {"channelId":channelId }}).pipe(

      tap((InstagramSummary: any) => {
          this._InstagramSummary.next(InstagramSummary);
      })
  );
 
}

getLatestInstagramComment(PostAPIId:any,channelId:string): Observable<any> {
   
  return this.http.get<any>(this.latestInstagramComment, { params: {"PostAPIId":PostAPIId,"channelId":channelId }}).pipe(

      tap((instagramComment: any[]) => {
          this._latestInstagramComment.next(instagramComment);
      })
  );
}
}
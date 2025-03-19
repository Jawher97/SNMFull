import { HttpClient, HttpParams } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { TwitterProfileData } from '../models/twitter-profile-data';
import { environment } from 'environments/environment.development';
import { Observable,BehaviorSubject,  ReplaySubject, map, tap } from 'rxjs';
import { Twitterpost } from '../models/twitterpost';
import { LinkedinProfileData } from '../models/linkedin-profile-data';
import { ChannelProfile } from '../models/ChannelProfile.model';

const twitterPath = `${environment.twitterAuthAPIURL}`;
const twitterOAuth = `${environment.twitterOAuthURL}`
const authUrl = `${environment.twitterv2AuthURL}`
const twitterUrl = `${environment.twitterPath}`

@Injectable({
  providedIn: 'root'
})
export class TwitterService {
  //bch ttbadelChannelProfile
  private _twiiterProfile: BehaviorSubject<ChannelProfile | null> = new BehaviorSubject(null);
  private CodeUrl = twitterPath +  'Authorize/GetCode'; 
  private tokenUrl = twitterPath + 'GetToken/GetToken';
  private publishUrl = twitterUrl + 'CreateTwitterPost'

  private authUrl = authUrl;

  constructor(private http: HttpClient) {}


  login(): void {
    const authWindow = window.open(`${this.authUrl}`, 'Twitter Auth', 'width=500,height=600');  
      
    }

    generateAccessToken(code: string, brandId:string): Observable<any> {
console.log("Tokennnnnnn " +code +brandId)
      return this.http.get<any>(this.tokenUrl, { params: { code: code,brandId:brandId } }).pipe(
        tap(response => {
          console.log("Tokennnnnnn " +response)
        // const accessToken = response.accessToken;
        // console.log(accessToken+"accesstoken twittter")
        // const refreshToken = response.refreshToken;
        // const screen_name = response.userName;
        // // Stocker l'access token localement
        // localStorage.setItem('Twitter access_token', accessToken);
        // localStorage.setItem('Twitter refresh_token', refreshToken);
        // localStorage.setItem('screen_name', screen_name);
        // console.log('Twitter access_token', accessToken);
        console.log(response+"rrr")
        // window.close();
        this._twiiterProfile.next(response)
         
      }));
}

publishToTwitter(post: Twitterpost): Observable<Twitterpost> {
  const accesstoken = localStorage.getItem('Twitter access_token');

  // const params = new HttpParams()
  // .set('post', post)
  // .set('accessToken', accesstoken);
  const reqBody = {"AccessToken": accesstoken,"Message":post.message}
  
  return this.http.post<Twitterpost>(this.publishUrl, reqBody)
}


  }




//   generateAccessToken(oauth_verifier: string) {
//     this.oauth_token = localStorage.getItem('oauth_token');
//     const params = {
//       'oauth_token': this.oauth_token,
//       'oauth_verifier': oauth_verifier
//     };

//     this.http.get(this.tokenUrl, {params}).subscribe((response: TwitterProfileData) => {
      

//       const accessToken = response.oauth_token;
//       const oauth_token_secret = response.oauth_token_secret;
//       const userId = response.TwitterUserId;
//       const screen_name = response.screen_name;
//       // Stocker l'access token localement
//       localStorage.setItem('Twitter access_token', accessToken);
//       localStorage.setItem('Twitter oauth_token_secret', oauth_token_secret);
//       localStorage.setItem('Twitter UserId', userId);
//       localStorage.setItem('Twitter screen_name', screen_name);
      
      
//       window.close();
      
      
//     });
// }



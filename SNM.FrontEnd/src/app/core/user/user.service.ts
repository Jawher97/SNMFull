import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { map, Observable, ReplaySubject, tap } from 'rxjs';
import { User } from 'app/core/user/user.types';
import { environment, linkedInPaths } from 'environments/environment.development';
import { LinkedinProfileData } from '../models/linkedin-profile-data';

const linkedInPath = `${linkedInPaths.linkedInAuthAPIURL}`;
const facebookAppId = `${environment.facebookAppId}`;
@Injectable({
    providedIn: 'root'
})
export class UserService
{
    private _user: ReplaySubject<User> = new ReplaySubject<User>(1);
    private baseUrl = "https://localhost:44345/auth/User/";

    private tokenUrl = linkedInPath + 'AccessToken/AccessToken'; // URL de votre API de génération de token


    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient)
    {
        FB.init({
            appId :  facebookAppId,
            status : false,
            cookie : false,
            xfbml  : false,
            version : 'v8.0'
          });
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Setter & getter for user
     *
     * @param value
     */
    set user(value: User)
    {
        // Store the value
        this._user.next(value);
    }

    get user$(): Observable<User>
    {
        return this._user.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get the current logged in user data
     */
    get(): Observable<User>
    {
      const email = localStorage.getItem('email');
      const params = new HttpParams().set('email', email);
      return this._httpClient.get<User>(this.baseUrl + 'GetUser', {params: params}).pipe(
        tap((user) => {
            this._user.next(user);
            
        })
    );
    }

    /**
     * Update the user
     *
     * @param user
     */
    update(user: User): Observable<any>
    {
        return this._httpClient.put<User>(this.baseUrl + 'update', user).pipe(
            map((response) => {
                this._user.next(response);
            })
        );
    }


/****Facebook Login */
    fbLogin() {
        return new Promise((resolve, reject) => {
    
          FB.login(result => {
            if (result.authResponse) {
              return this._httpClient
                .post(`https://localhost:4200/brand/Consultim-IT/channels`, {access_token: result.authResponse.accessToken})
                .toPromise()
                .then(response => {
                const token = response;
                if (token) {
                  localStorage.setItem('id_token', JSON.stringify(token));
                }
                resolve(response);
                })
                .catch(() => reject());
            } else {
              reject();
            }
          }, { scope: 'public_profile,email,pages_show_list,groups_access_member_info,business_management' });
        });
      }
    
      isLoggedIn() {
        return new Promise((resolve, reject) => {
          this.getCurrentUser().then(user => resolve(true)).catch(() => reject(false));
        });
      }
    
      getCurrentUser() {
        return new Promise((resolve, reject) => {
          return this._httpClient.get(`http://localhost:8000/api/auth/me`).toPromise().then(response => {
            resolve(response);
          }).catch(() => reject());
        });
      }
    
      logout() {
        localStorage.removeItem('id_token');
        localStorage.clear();
      }


  /**Linkedin Login */    
  
}

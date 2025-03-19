import { Injectable } from "@angular/core";
import { environment, linkedInPaths } from "environments/environment.development";
import { BehaviorSubject, Observable, tap } from "rxjs";
import { LinkedinPost } from "../models/linkedin-post";
import { HttpClient, HttpParams } from "@angular/common/http";

const linkedInPath = `${linkedInPaths.linkedInAuthAPIURL}`;
const linkedInOAuth = `${linkedInPaths.linkedinOAuthURL}`;
const linkedinApi = `${linkedInPaths.linkedinAPIURL}`;

@Injectable({
    providedIn: 'root',
})
export class LinkedInPostService {

    private _linkedinpost: BehaviorSubject<LinkedinPost | null> = new BehaviorSubject(null);
    private _linkedinposts: BehaviorSubject<LinkedinPost[] | null> = new BehaviorSubject(null);
    /**
       * Constructor
       */
    constructor(private _httpClient: HttpClient, ) {

    }
    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
       * Getter for posts
       */
    get linkedinposts$(): Observable<LinkedinPost[]> {
        return this._linkedinposts.asObservable();
    }

    /**
     * Getter for post
     */
    get linkedinpost$(): Observable<LinkedinPost> {
        return this._linkedinpost.asObservable();
    }



    /**
         * Get LinkedIn Posts
         */
    getLinkedinPosts(): Observable<LinkedinPost[]> {
       
        const accesstoken = localStorage.getItem('LinkedIn access_token');
        const memberUrn = localStorage.getItem('org_urn');

        const params = new HttpParams()
            .set('accessToken', accesstoken)
            .set('memberUrn', memberUrn);
        return this._httpClient.get<LinkedinPost[]>(linkedinApi + "GetLinkedinPosts", { params: params }).pipe(
            tap((response) => {
                this._linkedinposts.next(response);
            })
        );

    }
}
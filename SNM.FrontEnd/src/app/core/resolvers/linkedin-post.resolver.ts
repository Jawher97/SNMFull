import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Resolve, RouterStateSnapshot } from "@angular/router";
import { LinkedinPost } from "../models/linkedin-post";
import { Observable } from "rxjs";
import { LinkedInPostService } from "../services-api/linkedIn-post.service";

@Injectable({
    providedIn: 'root'
})
export class LinkedinPostResolver implements Resolve<any>
{
    /**
     * Constructor
     */
    constructor(
        private _linkedinPostService: LinkedInPostService
    )
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Resolver
     *
     * @param route
     * @param state
     */
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<LinkedinPost[]>
    {        
        return this._linkedinPostService.getLinkedinPosts();
    }
}
import { Injectable } from "@angular/core";
import { FacebookService } from "../services-api";
import { ActivatedRouteSnapshot, RouterStateSnapshot } from "@angular/router";
import { Observable } from "rxjs";

@Injectable({
    providedIn: 'root'
})
export class FacebookPosts {
    
    /**
     * Constructor
     */
    constructor(
        private _facebookService: FacebookService
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
    // resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<FacebookPosts[]>
    // {
    //     return this._facebookService.getLatestFacebookPost();
    // }
}

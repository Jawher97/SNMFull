import {Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, Subject, takeUntil, tap } from 'rxjs';
import { Activity } from 'app/modules//activities/activities.types';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';

@Injectable({
    providedIn: 'root'
})
export class ActivitiesService
{
    // Private
    private _activities: BehaviorSubject<any> = new BehaviorSubject(null);
    private baseUrl = "https://localhost:44345/auth/User/";
    user: User;

    private _unsubscribeAll: Subject<any> = new Subject<any>();
    
    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient,private userService: UserService)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for activities
     */
    get activities(): Observable<any>
    {
        return this._activities.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get activities
     */
    getActivities(): Observable<any>
    {
        const email = localStorage.getItem('email');

        const params = new HttpParams()
        .set ('email',email);

        return this._httpClient.get<Activity[]>(this.baseUrl + 'getAllActivities', {params: params}).pipe(
            tap((response: Activity[]) => {
                this._activities.next(response);
            })
        );
    }
}

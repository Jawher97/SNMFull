import { Injectable } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { BehaviorSubject, Observable, tap } from 'rxjs';
import { LinkedInService } from 'app/core/services-api/linkedin.service';
import { environment, linkedInPaths  } from 'environments/environment.development';
import { LinkedInChannelDto } from 'app/core/models/brand.model';

const linkedinApi = `${linkedInPaths.linkedinAPIURL}`;

@Injectable({
    providedIn: 'root'
})
export class AnalyticsService
{
    private _data: BehaviorSubject<any> = new BehaviorSubject(null);
    private baseUrl = linkedinApi + 'GetAnalyticsData';
    
    OrgUrn: string = 'urn:li:organization:92781486';
    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient, private _linkedinservice: LinkedInService)
    {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
     * Getter for data
     */
    get data$(): Observable<any>
    {
        return this._data.asObservable();
        
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Get data
     */
        /**
     * Get data
     */
        getData(): Observable<any>
        {
            const accesstoken = localStorage.getItem('LinkedIn access_token')
            const params = new HttpParams()
            .set('OrgUrn', this.OrgUrn)
            .set('accesstoken', accesstoken);
            
            return this._httpClient.get(this.baseUrl, {params:params}).pipe(
                tap((response: any) => {
                    this._data.next(response);
                })
            );
        }

    // getData(): Observable<LinkedInChannel[]> 
    // {
    //     return this._linkedinservice.getCompanyPages().pipe(
    //         tap((response: any) => {

    //             this._data.next(response);
    //         })
    //     );

    // }
}

import { HttpClient } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { environment } from "environments/environment.development";
import { BehaviorSubject, Observable, ReplaySubject, map, tap } from "rxjs";
import { ChannelType } from "../models/channelType.model";
import { ActivatedRoute } from "@angular/router";


const channelTypePath = `${environment.channelTypeURL}`;
@Injectable({
    providedIn: 'root'
})

export class ChannelTypeService {
    // Private
    private _channelTypes: BehaviorSubject<ChannelType[] | null> = new BehaviorSubject(null);
    pop: Window | null = null;
    /**
     * Constructor
     */
    constructor(private route: ActivatedRoute,private _httpClient: HttpClient) {

    }
    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------
    /**
         * Getter for socialchannels
         */
    get channelTypes$(): Observable<ChannelType[]> {
        return this._channelTypes.asObservable();
    }
    getPop(): Window | null {
        console.log(this.pop + "popup reference");
        return this.pop;
      }
    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
    * Connect Social CHannel
    */
    connectChannel(url:string) {
      
         window.open(url,"windowName", 'toolbar=0,status=0,width=626,height=436');
      console.log(url+"eeeeeee")
       
       
      
}
    /**
    * Get Social Channels
    */
    getChannelTypes(brandId: string): Observable<ChannelType[]> {
        console.log(brandId+"brandId")
        return this._httpClient.get<ChannelType[]>(channelTypePath + "GetChannelTypesByBrandId", { params: { 'brandId': brandId } }).pipe(
            map(response => response.map(item => new ChannelType(item))),
            tap(channelTypes => this._channelTypes.next(channelTypes))
        );
    }


}
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject,  map, Observable, tap } from 'rxjs';
import { environment } from 'environments/environment.development';
import { SocialChannel } from '../models/socialChannel.model';
import { ActivationStatus } from '../enumerations/ActivationStatus';


@Injectable({
    providedIn: 'root'
})
export class SocialChannelService {
    // Private
    private channelPath: string;
    private _socialChannels: BehaviorSubject<SocialChannel[] | null> = new BehaviorSubject(null);
    private _socialChannel: BehaviorSubject<SocialChannel | null> = new BehaviorSubject(null);
    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient) {
        this.channelPath = environment.channelURL;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------
    /**
         * Getter for socialchannels
         */
    get socialChannels$(): Observable<SocialChannel[]> {
        return this._socialChannels.asObservable();
    } 
    get socialChannel$(): Observable<SocialChannel> {
        return this._socialChannel.asObservable();
    } 
   
    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
   
    /**
    * Get Social channels
    */
    getSocialChannels(brandId: string): Observable<SocialChannel[]> {
        return this._httpClient.get<SocialChannel[]>(this.channelPath + "GetByBrandId", { params: { 'brandId': brandId } }).pipe(
            map(response => response.map(item => new SocialChannel(item))),
            tap(boards => this._socialChannels.next(boards))
        );
    }
    getSocialChannelsByChannelTypeId(channelId: string,brandId:string): Observable<SocialChannel[]> {
        return this._httpClient.get<SocialChannel[]>(this.channelPath + "GetByChannelTypeId", { params: { 'channelypeId': channelId ,'brandId':brandId} }).pipe(
            map(response => response.map(item => new SocialChannel(item))),
            tap((boards: any) => {this._socialChannel.next(boards)})
        );
    }
    
    
    UpdateSocialChannels(channel: SocialChannel[]): Observable<SocialChannel[]> {
       // channel.isActivate=ActivationStatus.Active;
        console.log(channel+"ccccc")
        return this._httpClient.put<any[]>(this.channelPath + "Update",channel)
      
    }
    DeleteChannels(channels: any[]): Observable<any[]> {
        
         return this._httpClient.delete<any[]>(this.channelPath + "Delete/Channels", {
            body: channels
          });
       
     }
}
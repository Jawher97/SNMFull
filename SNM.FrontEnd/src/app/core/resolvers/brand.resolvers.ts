import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, Resolve, Router, RouterStateSnapshot } from '@angular/router';
import { catchError, Observable, throwError } from 'rxjs';
import { Brand, FacebookChannelDto } from '../models/brand.model';
import { BrandsService } from '../services-api/brands.service';


@Injectable({
    providedIn: 'root'
})
export class BrandsResolver implements Resolve<any>
{
    /**
     * Constructor
     */
    constructor(
        private _brandService: BrandsService
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
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Brand[]>
    {
        return this._brandService.getAll();
    }
}

@Injectable({
    providedIn: 'root'
})
export class BrandResolver implements Resolve<any>
{
    /**
     * Constructor
     */
    constructor(
        private _router: Router,
        private _brandService: BrandsService
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
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<Brand>
    {      
        return this._brandService.getBrandById(route.paramMap.get('displayName'))
                   .pipe(
                       // Error here means the requested task is not available
                       catchError((error) => {

                           // Log the error
                           console.error(error);

                           // Get the parent url
                           const parentUrl = state.url.split('/').slice(0, -1).join('/');

                           // Navigate to there
                           this._router.navigateByUrl(parentUrl);

                           // Throw an error
                           return throwError(error);
                       })
                   );
    }
}
@Injectable({
    providedIn: 'root'
})
export class ChannelResolver implements Resolve<any>
{
    brandId: string;
    
    /**
     * Constructor
     */
    constructor(
        private _router: Router,
        private _brandService: BrandsService
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
    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<FacebookChannelDto[]>
    { 
        
        this._brandService.brand$
            .pipe()
            .subscribe((brand: Brand) => {             

                // Get the brand
                this.brandId= brand.id;});
        return this._brandService.getFacebookChannels(this.brandId)
                   .pipe(
                       // Error here means the requested task is not available
                       catchError((error) => {

                           // Log the error
                           console.error(error);

                           // Get the parent url
                           const parentUrl = state.url.split('/').slice(0, -1).join('/');

                           // Navigate to there
                           this._router.navigateByUrl(parentUrl);

                           // Throw an error
                           return throwError(error);
                       })
                   );
    }
}
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject, filter, map, Observable, of, ReplaySubject, switchMap, take, tap, throwError } from 'rxjs';
import { Brand, FacebookChannelDto } from '../models/brand.model';
import { environment } from 'environments/environment.development';


@Injectable({
    providedIn: 'root'
})
export class BrandsService {
    // Private
    private brandPath: string;
    private facebookChannelPath: string;
    private _brands: ReplaySubject<Brand[]> = new ReplaySubject<Brand[]>(1);
    private _brand: ReplaySubject<Brand> = new ReplaySubject<Brand>(1);
    private _facebookChannels: BehaviorSubject<FacebookChannelDto[] | null>;
    private _brandsWithChannels: ReplaySubject<Brand[]> = new ReplaySubject<Brand[]>(1);

    /**
     * Constructor
     */
    constructor(private _httpClient: HttpClient) {
        this.brandPath = environment.brandURL;
        this.facebookChannelPath = environment.facebookChannelIURL;
        // Set the private defaults
        this._facebookChannels = new BehaviorSubject(null);
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Accessors
    // -----------------------------------------------------------------------------------------------------

    /**
         * Getter for facebook channels
         */
    get facebookChannels$(): Observable<FacebookChannelDto[]> {
        return this._facebookChannels.asObservable();
    }
    /**
    * Getter for brand
    */
    get brand$(): Observable<Brand> {
        return this._brand.asObservable();
    }
    /**
     * Getter for brands
     */
    get brands$(): Observable<Brand[]> {
        return this._brands.asObservable();
    }
    get brandsWithChannels$(): Observable<Brand[]> {
        return this._brandsWithChannels.asObservable();
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------
   
   
    /**
    * Get Facebook channels
    */
    getFacebookChannels(id: string): Observable<FacebookChannelDto[]> {

        return this._httpClient.get<FacebookChannelDto[]>(this.facebookChannelPath + "GetAllByBrandId", { params: { id } }).pipe(
            map(response => response.map(item => new FacebookChannelDto(item))),
            tap(boards => this._facebookChannels.next(boards))
        );
    }

    /**
     * Get all messages
     */
    getAll(): Observable<Brand[]> {
        return this._httpClient.get<Brand[]>(this.brandPath + "GetAll").pipe(
            tap((brands) => {
                this._brands.next(brands);
            })
        );
    }
    /**
    * Get all Brand with their channels
    */
    getAllBrandWithChannels(): Observable<Brand[]> {
        return this._httpClient.get<Brand[]>(this.brandPath + "GetBrandsAndChannels").pipe(
            tap((brands) => {
                this._brandsWithChannels.next(brands);
            })
        );
    }
  
    /**
        * Get board
        *
        * @param id
        */
    // getBrand(id: string): Observable<Brand> {
    //     id="08db4587-5d6c-47a1-8dd1-8a963a5bd8d0";
    //     return this._httpClient.get<Brand>(this.brandPath +"GetById", { params: { id } }).pipe(
    //         map(response => new Brand(response)),
    //         tap(brand => this._brand.next(brand))
    //     );
    // }
    /**
     * Get default brand
     */
    // getDefaultBrand(): Observable<Brand> {

    //     return this._brands.pipe(
    //         take(1),
    //         map((brands) => {

    //             // Find the brand
    //             const brand = brands.find(item => item.isDefault === true) || null;

    //             // Update the brand
    //             this._brand.next(brand);

    //             // Return the brand
    //             return brand;
    //         }),
    //         switchMap((brand) => {

    //             if (!brand) {
    //                 return throwError('Could not found any brand !');
    //             }

    //             return of(brand);
    //         })
    //     );
    // } 
    /**
     * Get brand by id
     */
    getBrandById(id: string): Observable<Brand> {

        return this._brands.pipe(
            take(1),
            map((brands) => {

                // Find the brand
                const brand = brands.find(item => item.displayName === id) || null;

                // Update the brand
                this._brand.next(brand);

                // Return the brand
                return brand;
            }),
            switchMap((brand) => {

                if (!brand) {
                    return throwError('Could not found brand with id of ' + id + '!');
                }

                return of(brand);
            })
        );
    }
    /**
     * Create a brand
     *
     * @param brand
     */
    create(brand: Brand): Observable<Brand> {
        const formData: FormData = new FormData();
        
        formData.append('photo', brand.photo);
        formData.append('description', brand.description);
        formData.append('displayName', brand.displayName);
        return this.brands$.pipe(
            take(1),
            switchMap(brands => this._httpClient.post<Brand>(this.brandPath + 'CreateBrand', formData).pipe(
                map((newBrand) => {
                    brand.id = newBrand.id;
                    // Update the brands with the new brand
                    this._brands.next([...brands, brand]);

                    // Return the new brand from observable
                    return brand;
                })
            ))
        );
    }

    /**
     * Update the brand
     *
     * @param id
     * @param brand
     */
    updateBrand(id: string, brand: Brand): Observable<Brand> {
        return this.brands$.pipe(
            take(1),
            switchMap(brands => this._httpClient.patch<Brand>(this.brandPath + 'UpdateBrand', brand
                // {
                //     // id,
                //     brand
                // }
            ).pipe(
                map((updatedBrand: Brand) => {

                    // Find the index of the updated brand
                    const index = brands.findIndex(item => item.id === id);
                    brand.id=id
                    // Update the brand
                    brands[index] = brand;
                    console.log(JSON.stringify(brand)+"updatedBrand")
                    this._brand.next(brand);
                    // Update the brands
                    this._brands.next(brands);

                    // Return the updated brand
                    return updatedBrand;
                })
            ))
        );
    }

    /**
     * Delete the brand
     *
     * @param id
     */
    deleteBrand(id: string): Observable<boolean> {
        return this.brands$.pipe(
            take(1),
            switchMap(brands => this._httpClient.delete<boolean>(this.brandPath + 'DeleteBrand', { params: { 'id': id } }).pipe(
                map((isDeleted: boolean) => {

                    // Find the index of the deleted brand
                    const index = brands.findIndex(item => item.id === id);

                    // Delete the brand
                    brands.splice(index, 1);

                    // Update the brands
                    this._brands.next(brands);

                    // Return the deleted status
                    return isDeleted;
                })
            ))
        );
    }

    /**
     * Update the avatar of the given brand
     *
     * @param id
     * @param avatar
     */
    uploadAvatar(id: string, avatar: File): Observable<Brand> {
        return this.brands$.pipe(
            take(1),
            switchMap(brands => this._httpClient.post<Brand>('api/apps/brands/avatar', {
                id,
                avatar
            }, {
                headers: {
                    // eslint-disable-next-line @typescript-eslint/naming-convention
                    'Content-Type': avatar.type
                }
            }).pipe(
                map((updatedBrand) => {

                    // Find the index of the updated brand
                    const index = brands.findIndex(item => item.id === id);

                    // Update the brand
                    brands[index] = updatedBrand;

                    // Update the brands
                    this._brands.next(brands);

                    // Return the updated brand
                    return updatedBrand;
                }),
                switchMap(updatedBrand => this.brand$.pipe(
                    take(1),
                    filter(item => item && item.id === id),
                    tap(() => {

                        // Update the brand if it's selected
                        this._brand.next(updatedBrand);

                        // Return the updated brand
                        return updatedBrand;
                    })
                ))
            ))
        );
    }
}
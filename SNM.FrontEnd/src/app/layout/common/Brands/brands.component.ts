import { ChangeDetectionStrategy, ChangeDetectorRef, Component, Input, OnDestroy, OnInit, TemplateRef, ViewChild, ViewContainerRef, ViewEncapsulation } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Overlay, OverlayRef } from '@angular/cdk/overlay';
import { TemplatePortal } from '@angular/cdk/portal';
import { MatButton } from '@angular/material/button';
import { BehaviorSubject, Observable, Subject, debounceTime, switchMap, takeUntil } from 'rxjs';
import { Brand } from 'app/core/models/brand.model';
import { BrandsService } from 'app/core/services-api/brands.service';

@Component({
    selector: 'brands',
    templateUrl: './brands.component.html',
    encapsulation: ViewEncapsulation.None,
    changeDetection: ChangeDetectionStrategy.OnPush,
    exportAs: 'brands'
})
export class BrandsComponent implements OnInit, OnDestroy {
    @ViewChild('brandsOrigin') private _brandsOrigin: MatButton;
    @ViewChild('brandsPanel') private _brandsPanel: TemplateRef<any>;
    mode: 'view' | 'modify' | 'add' | 'edit' = 'view';
    brandForm: UntypedFormGroup;
    brands: Brand[];
    currentBrand$: Observable<Brand>;

    private _overlayRef: OverlayRef;
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _formBuilder: UntypedFormBuilder,
        private _brandsService: BrandsService,
        private _overlay: Overlay,
        private _viewContainerRef: ViewContainerRef,
    ) {
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void {
        // Initialize the form
        this.brandForm = this._formBuilder.group({
            id: [null],
            displayName: ['', Validators.required],
            description: [''],
            timeZone: [''],
            photo: [''],
            //  link       : ['', Validators.required],
            // useRouter  : ['', Validators.required]
        });

        // Get the brands
        this._brandsService.brands$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe((brands: Brand[]) => {
                // Load the brands
                this.brands = brands;
                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
        // Get the current Brand
        this.currentBrand$ = this._brandsService.brand$;
       
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();

        // Dispose the overlay
        if (this._overlayRef) {
            this._overlayRef.dispose();
        }
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Public methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Open the brands panel
     */
    selectedBrandChanged(brandId: string): void {
        this._brandsService.getBrandById(brandId).subscribe()
    }
    /**
     * Open the brands panel
     */
    openPanel(): void {
        // Return if the brands panel or its origin is not defined
        if (!this._brandsPanel || !this._brandsOrigin) {
            return;
        }

        // Make sure to start in 'view' mode
        this.mode = 'view';

        // Create the overlay if it doesn't exist
        if (!this._overlayRef) {
            this._createOverlay();
        }

        // Attach the portal to the overlay
        this._overlayRef.attach(new TemplatePortal(this._brandsPanel, this._viewContainerRef));
    }

    /**
     * Close the brands panel
     */
    closePanel(): void {
        this._overlayRef.detach();
    }

    /**
    * Upload image to given post
    *
    * @param post
    * @param fileList
    */
    //   uploadImage(post: Post, fileList: FileList): void {
    //     // Return if canceled
    //     if (!fileList.length) {
    //       return;
    //     }

    //     const allowedTypes = ['image/jpeg', 'image/png', 'video/mp4'];
    //     for (let i = 0; i < fileList.length; i++) {

    //       const file = fileList[i];
    //       // Return if the file is not allowed
    //       if (!allowedTypes.includes(file.type)) {
    //         return;
    //       }
    //       this._readAsDataURL(file).then((data) => {
    //         const image = {
    //           id: '630d2e9a-d110-47a0-ac03-256073a0f56d',
    //           folderId: null,
    //           name: 'Scanned image 20201012-2',
    //           createdBy: 'Brian Hughes',
    //           createdAt: 'September 14, 2020',
    //           modifiedAt: 'September 14, 2020',
    //           size: '7.4 MB',
    //           type: file.type,
    //           src: data,
    //           contents: null,
    //           description: null
    //         }

    //         // Update the image
    //         post?.images.push(image);

    //         // Update the post
    //         this.postChanged.next(post);
    //       });
    //     }


    //   }
    /**
     * Change the mode
     */
    changeMode(mode: 'view' | 'modify' | 'add' | 'edit'): void {
        // Change the mode
        this.mode = mode;
    }

    /**
     * Prepare for a new brand
     */
    newBrand(): void {
        // Reset the form
        this.brandForm.reset();

        // Enter the add mode
        this.mode = 'add';
    }

    /**
     * Edit a brand
     */
    editBrand(brand: Brand): void {
        // Reset the form with the brand
        this.brandForm.reset(brand);

        // Enter the edit mode
        this.mode = 'edit';
    }

    /**
     * Save brand
     */
    save(): void {
        // Get the data from the form
        const brand = this.brandForm.value;

        // If there is an id, update it...
        if (brand.id) {
            this._brandsService.updateBrand(brand.id, brand).subscribe();
        }
        // Otherwise, create a new brand...
        else {
            this._brandsService.create(brand).subscribe();
        }

        // Go back the modify mode
        this.mode = 'modify';
    }

    /**
     * Delete brand
     */
    delete(): void {
        // Get the data from the form
        const brand = this.brandForm.value;

        // Delete
        this._brandsService.deleteBrand(brand.id).subscribe();

        // Go back the modify mode
        this.mode = 'modify';
    }

    /**
     * Track by function for ngFor loops
     *
     * @param index
     * @param item
     */
    trackByFn(index: number, item: any): any {
        return item.id || index;
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Private methods
    // -----------------------------------------------------------------------------------------------------

    /**
     * Create the overlay
     */
    private _createOverlay(): void {
        // Create the overlay
        this._overlayRef = this._overlay.create({
            hasBackdrop: true,
            backdropClass: 'fuse-backdrop-on-mobile',
            scrollStrategy: this._overlay.scrollStrategies.block(),
            positionStrategy: this._overlay.position()
                .flexibleConnectedTo(this._brandsOrigin._elementRef.nativeElement)
                .withLockedPosition(true)
                .withPush(true)
                .withPositions([
                    {
                        originX: 'start',
                        originY: 'bottom',
                        overlayX: 'start',
                        overlayY: 'top'
                    },
                    {
                        originX: 'start',
                        originY: 'top',
                        overlayX: 'start',
                        overlayY: 'bottom'
                    },
                    {
                        originX: 'end',
                        originY: 'bottom',
                        overlayX: 'end',
                        overlayY: 'top'
                    },
                    {
                        originX: 'end',
                        originY: 'top',
                        overlayX: 'end',
                        overlayY: 'bottom'
                    }
                ])
        });

        // Detach the overlay from the portal on backdrop click
        this._overlayRef.backdropClick().subscribe(() => {
            this._overlayRef.detach();
        });
    }
    uploadMedia(fileList: FileList): void {
   
        // Return if canceled
        if (!fileList.length) {
          return;
        }
    
        const allowedTypes = ['image/jpeg', 'image/png', 'video/mp4', 'video/x-m4v', 'video/*'];
        for (let i = 0; i < fileList.length; i++) {
          const file = fileList[i];
          // Return if the file is not allowed
          if (!allowedTypes.includes(file.type)) {
            return;
          }
          console.log(file.type+"file.type")
          
          this._readAsDataURL(file).then((data) => {
          
            this.brandForm.get('photo').setValue(data);
            console.log( this.brandForm.get('photo').value)
            // Add the image or video object to the post
          
           
           // this.errorMessages=[]
            // Update the post
          
             this._changeDetectorRef.detectChanges();
          });
        }
        
    
      }
      deleteMedia(): void {
        this.brandForm.get('photo').setValue('');
       
      }
      private _readAsDataURL(file: File): Promise<string> {
        // Return a new promise
        return new Promise((resolve, reject) => {
          // Create a new reader
          const reader = new FileReader();
    
          // Resolve the promise on success
          reader.onload = (event) => {
            const dataURL = event.target.result as string;
         
        
              resolve(dataURL);
          
          };
    
          // Reject the promise on error
          reader.onerror = (e) => {
            reject(e);
          };
    
          // Read the file as data URL
          reader.readAsDataURL(file);
        });
      }
}

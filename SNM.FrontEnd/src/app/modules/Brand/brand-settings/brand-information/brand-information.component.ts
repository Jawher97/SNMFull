import { ChangeDetectorRef, Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { UntypedFormBuilder, UntypedFormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { FuseConfirmationService } from '@fuse/services/confirmation';
import { Brand } from 'app/core/models/brand.model';
import { BrandsService } from 'app/core/services-api/brands.service';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-brand-information',
  templateUrl: './brand-information.component.html',
  styleUrls: ['./brand-information.component.scss']
})
export class BrandInformationComponent implements OnInit {
  @ViewChild('avatarFileInput') private _avatarFileInput: ElementRef;
  @ViewChild('FileInput') private _FileInput: ElementRef;

  
  editMode: boolean = false;
  brand: Brand ;
  // brand: Brand = {
  //   "displayName": "Consultim-IT",
  //   "description": "Consultim Solutions",

  //   "photo": "string",
  //   "coverPhoto": "assets/images/cards/14-640x480.jpg",
  //   "id": "08db4587-5d6c-47a1-8dd1-8a963a5bd8d0",



  //   "icon": "string| null",
  //   "link": "string| null",
  //   "useRouter": false,
  //   "lastActivity": "string | null",
  //   "socialChannels": null,
  //   "brandMembers": null,
  // };
  brandForm: UntypedFormGroup;
  brands: Brand[];
  @ViewChild('avatarFileInput') avatarFileInput: ElementRef<HTMLInputElement>;
  private _unsubscribeAll: Subject<any> = new Subject<any>();
  /**
     * Constructor
     */
  constructor(
    // private _activatedRoute: ActivatedRoute,
    private _changeDetectorRef: ChangeDetectorRef,
    // private _brandsListComponent: BrandsListComponent,
    private _brandService: BrandsService,
    private _formBuilder: UntypedFormBuilder,
    private _fuseConfirmationService: FuseConfirmationService,
    // private _renderer2: Renderer2,
    private _router: Router,
    // private _overlay: Overlay,
    // private _viewContainerRef: ViewContainerRef
  ) {
  }
  // -----------------------------------------------------------------------------------------------------
  // @ Lifecycle hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    // Open the drawer
    // this._brandsListComponent.matDrawer.open();

    // Create the brand form
    this.brandForm = this._formBuilder.group({
      id: [''],
     // avatar: [null],
      coverPhoto: [null],
      photo: [null],
      displayName: ['', [Validators.required]],
      description: [''],
      // emails: this._formBuilder.array([]),
      // phoneNumbers: this._formBuilder.array([]),
      // description: [''],
      // company: [''],
      // birthday: [null],
      // address: [null],
      // notes: [null],
      // tags: [[]]
    });
  
    // Get the brand
    this._brandService.brand$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((brand: Brand) => {
        // Get the brand
        this.brand = brand;
      // console.log(JSON.stringify(this.brand)+"this.brandobservable")
         // Patch values to the form
         this.brandForm.patchValue(brand);
        // Toggle the edit mode off
        this.toggleEditMode(false);
        // Mark for check
        this._changeDetectorRef.markForCheck();
      });
      this.brandForm.get('photo').valueChanges.subscribe((newValue) => {

      });
  }
  // -----------------------------------------------------------------------------------------------------
  // @ Public methods
  // -----------------------------------------------------------------------------------------------------

  /**
   * Close the drawer
   */
  // closeDrawer(): Promise<MatDrawerToggleResult>
  // {
  //     return this._brandsListComponent.matDrawer.close();
  // }

  /**
   * Toggle edit mode
   *
   * @param editMode
   */
  toggleEditMode(editMode: boolean | null = null): void {
    if (editMode === null) {
      this.editMode = !this.editMode;

    }
    else {
      this.editMode = editMode;
     
       
    }

    // Mark for check
    this._changeDetectorRef.markForCheck();
  }

  /**
   * Update the brand
   */
  updateBrand(): void {
    // Get the brand object
   // const brand = this.brandForm.getRawValue();
    console.log(JSON.stringify(this.brandForm.value)+"brandForm")
    // Go through the brand object and clear empty values
    // brand.emails = brand.emails.filter(email => email.email);

    // brand.phoneNumbers = brand.phoneNumbers.filter(phoneNumber => phoneNumber.phoneNumber);

    // Update the brand on the server
    this._brandService.updateBrand(this.brandForm.value.id, this.brandForm.value).subscribe(() => {
      this._changeDetectorRef.detectChanges()
      // Toggle the edit mode off
      this.toggleEditMode(false);
    });
  }

  /**
   * Delete the brand
   */
  deleteBrand(): void {
    // Open the confirmation dialog
    const confirmation = this._fuseConfirmationService.open({
      title: 'Delete brand',
      message: 'Are you sure you want to delete this brand? This action cannot be undone!',
      actions: {
        confirm: {
          label: 'Delete'
        }
      }
    });

    // Subscribe to the confirmation dialog closed action
    // confirmation.afterClosed().subscribe((result) => {

    //     // If the confirm button pressed...
    //     if ( result === 'confirmed' )
    //     {
    //         // Get the current brand's id
    //         const id = this.brand.id;

    //         // Get the next/previous brand's id
    //         const currentBrandIndex = this.brands.findIndex(item => item.id === id);
    //         const nextBrandIndex = currentBrandIndex + ((currentBrandIndex === (this.brands.length - 1)) ? -1 : 1);
    //         const nextBrandId = (this.brands.length === 1 && this.brands[0].id === id) ? null : this.brands[nextBrandIndex].id;

    //         // Delete the brand
    //         this._brandService.deleteBrand(id)
    //             .subscribe((isDeleted) => {

    //                 // Return if the brand wasn't deleted...
    //                 if ( !isDeleted )
    //                 {
    //                     return;
    //                 }

    //                 // Navigate to the next brand if available
    //                 if ( nextBrandId )
    //                 {
    //                     this._router.navigate(['../', nextBrandId], {relativeTo: this._activatedRoute});
    //                 }
    //                 // Otherwise, navigate to the parent
    //                 else
    //                 {
    //                     this._router.navigate(['../'], {relativeTo: this._activatedRoute});
    //                 }

    //                 // Toggle the edit mode off
    //                 this.toggleEditMode(false);
    //             });

    //         // Mark for check
    //         this._changeDetectorRef.markForCheck();
    //     }
    // });

  }

  /**
   * Upload avatar
   *
   * @param fileList
   */
  uploadAvatar(fileList: FileList,type:any): void {
    console.log(type+"file.type")
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
      
        if(type=="photo"){
          this.brandForm.get('photo').setValue(data);
      
        }
        if(type=="coverPhoto"){
          this.brandForm.get('coverPhoto').setValue(data);
          
        }
    
         this._changeDetectorRef.detectChanges();
        // this._brandService.uploadAvatar(this.brand.id, file).subscribe();
      });
    }
    
    
    

  }
  deleteMedia(type:any): void {
    if(type=="photo"){
    this.brandForm.get('photo').setValue('');}
    if(type=="coverPhoto"){
      this.brandForm.get('coverPhoto').setValue('');}
   
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

    // Upload the avatar
   
  

  /**
   * Remove the avatar
   */
  removeAvatar(type:any): void {
    // Get the form control for 'avatar'
    if(type=="coverPhoto"){
      const avatarFormControl = this.brandForm.get('coverPhoto');

      // Set the avatar as null
      avatarFormControl.setValue(null);
  
      // Set the file input value as null
      this._FileInput.nativeElement.value = null;
  
      // Update the brand
      this.brand.coverPhoto = null;
    }
    if(type=="photo"){
      const avatarFormControl = this.brandForm.get('photo');

      // Set the avatar as null
      avatarFormControl.setValue(null);
  
      // Set the file input value as null
      this._avatarFileInput.nativeElement.value = null;
  
      // Update the brand
      this.brand.photo = null;
    }
  
  }

  /**
   * Open tags panel
   */
  // openTagsPanel(): void
  // {
  //     // Create the overlay
  //     this._tagsPanelOverlayRef = this._overlay.create({
  //         backdropClass   : '',
  //         hasBackdrop     : true,
  //         scrollStrategy  : this._overlay.scrollStrategies.block(),
  //         positionStrategy: this._overlay.position()
  //                               .flexibleConnectedTo(this._tagsPanelOrigin.nativeElement)
  //                               .withFlexibleDimensions(true)
  //                               .withViewportMargin(64)
  //                               .withLockedPosition(true)
  //                               .withPositions([
  //                                   {
  //                                       originX : 'start',
  //                                       originY : 'bottom',
  //                                       overlayX: 'start',
  //                                       overlayY: 'top'
  //                                   }
  //                               ])
  //     });

  //     // Subscribe to the attachments observable
  //     this._tagsPanelOverlayRef.attachments().subscribe(() => {

  //         // Add a class to the origin
  //         this._renderer2.addClass(this._tagsPanelOrigin.nativeElement, 'panel-opened');

  //         // Focus to the search input once the overlay has been attached
  //         this._tagsPanelOverlayRef.overlayElement.querySelector('input').focus();
  //     });

  //     // Create a portal from the template
  //     const templatePortal = new TemplatePortal(this._tagsPanel, this._viewContainerRef);

  //     // Attach the portal to the overlay
  //     this._tagsPanelOverlayRef.attach(templatePortal);

  //     // Subscribe to the backdrop click
  //     this._tagsPanelOverlayRef.backdropClick().subscribe(() => {

  //         // Remove the class from the origin
  //         this._renderer2.removeClass(this._tagsPanelOrigin.nativeElement, 'panel-opened');

  //         // If overlay exists and attached...
  //         if ( this._tagsPanelOverlayRef && this._tagsPanelOverlayRef.hasAttached() )
  //         {
  //             // Detach it
  //             this._tagsPanelOverlayRef.detach();

  //             // Reset the tag filter
  //             this.filteredTags = this.tags;

  //             // Toggle the edit mode off
  //             this.tagsEditMode = false;
  //         }

  //         // If template portal exists and attached...
  //         if ( templatePortal && templatePortal.isAttached )
  //         {
  //             // Detach it
  //             templatePortal.detach();
  //         }
  //     });
  // }

  /**
   * Toggle the tags edit mode
   */
  // toggleTagsEditMode(): void
  // {
  //     this.tagsEditMode = !this.tagsEditMode;
  // }

  /**
   * Filter tags
   *
   * @param event
   */
  // filterTags(event): void
  // {
  //     // Get the value
  //     const value = event.target.value.toLowerCase();

  //     // Filter the tags
  //     this.filteredTags = this.tags.filter(tag => tag.title.toLowerCase().includes(value));
  // }

  /**
   * Filter tags input key down event
   *
   * @param event
   */
  // filterTagsInputKeyDown(event): void
  // {
  //     // Return if the pressed key is not 'Enter'
  //     if ( event.key !== 'Enter' )
  //     {
  //         return;
  //     }

  //     // If there is no tag available...
  //     if ( this.filteredTags.length === 0 )
  //     {
  //         // Create the tag
  //         this.createTag(event.target.value);

  //         // Clear the input
  //         event.target.value = '';

  //         // Return
  //         return;
  //     }

  //     // If there is a tag...
  //     const tag = this.filteredTags[0];
  //     const isTagApplied = this.brand.tags.find(id => id === tag.id);

  //     // If the found tag is already applied to the brand...
  //     if ( isTagApplied )
  //     {
  //         // Remove the tag from the brand
  //         this.removeTagFromBrand(tag);
  //     }
  //     else
  //     {
  //         // Otherwise add the tag to the brand
  //         this.addTagToBrand(tag);
  //     }
  // }

  /**
   * Create a new tag
   *
   * @param title
   */
  // createTag(title: string): void
  // {
  //     const tag = {
  //         title
  //     };

  //     // Create tag on the server
  //     this._brandService.createTag(tag)
  //         .subscribe((response) => {

  //             // Add the tag to the brand
  //             this.addTagToBrand(response);
  //         });
  // }

  /**
   * Update the tag title
   *
   * @param tag
   * @param event
   */
  // updateTagTitle(tag: Tag, event): void
  // {
  //     // Update the title on the tag
  //     tag.title = event.target.value;

  //     // Update the tag on the server
  //     this._brandService.updateTag(tag.id, tag)
  //         .pipe(debounceTime(300))
  //         .subscribe();

  //     // Mark for check
  //     this._changeDetectorRef.markForCheck();
  // }

  /**
   * Delete the tag
   *
   * @param tag
   */
  // deleteTag(tag: Tag): void
  // {
  //     // Delete the tag from the server
  //     this._brandService.deleteTag(tag.id).subscribe();

  //     // Mark for check
  //     this._changeDetectorRef.markForCheck();
  // }

  /**
   * Add tag to the brand
   *
   * @param tag
   */
  // addTagToBrand(tag: Tag): void
  // {
  //     // Add the tag
  //     this.brand.tags.unshift(tag.id);

  //     // Update the brand form
  //     this.brandForm.get('tags').patchValue(this.brand.tags);

  //     // Mark for check
  //     this._changeDetectorRef.markForCheck();
  // }

  /**
   * Remove tag from the brand
   *
   * @param tag
   */
  // removeTagFromBrand(tag: Tag): void
  // {
  //     // Remove the tag
  //     this.brand.tags.splice(this.brand.tags.findIndex(item => item === tag.id), 1);

  //     // Update the brand form
  //     this.brandForm.get('tags').patchValue(this.brand.tags);

  //     // Mark for check
  //     this._changeDetectorRef.markForCheck();
  // }

  /**
   * Toggle brand tag
   *
   * @param tag
   */
  // toggleBrandTag(tag: Tag): void
  // {
  //     if ( this.brand.tags.includes(tag.id) )
  //     {
  //         this.removeTagFromBrand(tag);
  //     }
  //     else
  //     {
  //         this.addTagToBrand(tag);
  //     }
  // }

  /**
   * Should the create tag button be visible
   *
   * @param inputValue
   */
  // shouldShowCreateTagButton(inputValue: string): boolean
  // {
  //     return !!!(inputValue === '' || this.tags.findIndex(tag => tag.title.toLowerCase() === inputValue.toLowerCase()) > -1);
  // }

  /**
   * Add the email field
   */
  // addEmailField(): void
  // {
  //     // Create an empty email form group
  //     const emailFormGroup = this._formBuilder.group({
  //         email: [''],
  //         label: ['']
  //     });

  //     // Add the email form group to the emails form array
  //     (this.brandForm.get('emails') as UntypedFormArray).push(emailFormGroup);

  //     // Mark for check
  //     this._changeDetectorRef.markForCheck();
  // }

  /**
   * Remove the email field
   *
   * @param index
   */
  // removeEmailField(index: number): void
  // {
  //     // Get form array for emails
  //     const emailsFormArray = this.brandForm.get('emails') as UntypedFormArray;

  //     // Remove the email field
  //     emailsFormArray.removeAt(index);

  //     // Mark for check
  //     this._changeDetectorRef.markForCheck();
  // }

  /**
   * Add an empty phone number field
   */
  // addPhoneNumberField(): void
  // {
  //     // Create an empty phone number form group
  //     const phoneNumberFormGroup = this._formBuilder.group({
  //         country    : ['us'],
  //         phoneNumber: [''],
  //         label      : ['']
  //     });

  //     // Add the phone number form group to the phoneNumbers form array
  //     (this.brandForm.get('phoneNumbers') as UntypedFormArray).push(phoneNumberFormGroup);

  //     // Mark for check
  //     this._changeDetectorRef.markForCheck();
  // }

  /**
   * Remove the phone number field
   *
   * @param index
   */
  // removePhoneNumberField(index: number): void
  // {
  //     // Get form array for phone numbers
  //     const phoneNumbersFormArray = this.brandForm.get('phoneNumbers') as UntypedFormArray;

  //     // Remove the phone number field
  //     phoneNumbersFormArray.removeAt(index);

  //     // Mark for check
  //     this._changeDetectorRef.markForCheck();
  // }

  /**
   * Get country info by iso code
   *
   * @param iso
   */
  // getCountryByIso(iso: string): Country
  // {
  //     return this.countries.find(country => country.iso === iso);
  // }

  /**
   * Track by function for ngFor loops
   *
   * @param index
   * @param item
   */
  trackByFn(index: number, item: any): any {
    return item.id || index;
  }
}


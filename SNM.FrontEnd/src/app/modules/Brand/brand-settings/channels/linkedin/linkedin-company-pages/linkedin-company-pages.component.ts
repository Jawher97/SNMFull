// import { HttpClient } from '@angular/common/http';
// import { ChangeDetectorRef, Component } from '@angular/core';
// import { FacebookChannelDto, LinkedInChannelDto } from 'app/core/models/brand.model';
// import { LinkedInChannel } from 'app/core/models/linkedinChannel';
// import { BrandsService } from 'app/core/services-api/brands.service';
// import { LinkedInService } from 'app/core/services-api/linkedin.service';

// import { environment, linkedInPaths } from "environments/environment.development";
// import { Subject, takeUntil } from 'rxjs';

// const linkedInOAuth = `${linkedInPaths.linkedinOAuthURL}`;

// @Component({
//   selector: 'linkedin-company-pages',
//   templateUrl: './linkedin-company-pages.component.html',
//   styleUrls: ['./linkedin-company-pages.component.scss']
// })
// export class LinkedinCompanyPagesComponent {
  
//   socialChannels: LinkedInChannelDto[];

//   private _unsubscribeAll: Subject<any> = new Subject<any>();

//   constructor(private linkedinservice: LinkedInService, private _changeDetectorRef: ChangeDetectorRef) {}

    
//   ngOnInit() {

//       this.linkedinservice.getCompanyPages().subscribe(
//         (pages: LinkedInChannelDto[]) => {

//           this.socialChannels = pages;
//           // Mark for check
//          this._changeDetectorRef.markForCheck();
//         },
//         (error) => {
//           console.error('Erreur lors de la récupération des pages LinkedIn:', error);
//         }
//       );
//       console.log(this.socialChannels);
    

//   }

  

//   login() {
//   this.linkedinservice.login();    
//   this.linkedinservice.getCompanyPages().subscribe(
//     (pages: LinkedInChannelDto[]) => {

//       this.socialChannels = pages;
//       // Mark for check
//      this._changeDetectorRef.markForCheck();
//     },
//     (error) => {
//       console.error('Erreur lors de la récupération des pages LinkedIn:', error);
//     }
//   );
// }



//   saveSelectedPages() {
//     //const selectedPages = this.companyPages.filter((page) => page.selected);
//     // Enregistrer les pages sélectionnées dans votre backend
//   }

//   /**
//      * Track by function for ngFor loops
//      *
//      * @param index
//      * @param item
//      */
// trackByFn(index: number, item: any): any
// {
//     return item.id || index;
// }



// }

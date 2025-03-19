import { ChangeDetectorRef, Component, Input } from '@angular/core';
import { InstagramService } from 'app/core/services-api/instagram.service';
import { instagramCredentials } from 'environments/environment.development';

@Component({
  selector: 'instagram-post-details',
  templateUrl: './instagram-post-details.component.html',
  styleUrls: ['./instagram-post-details.component.scss']
})
export class InstagramPostDetailsComponent {

  instagramPosts: any

  /**
     * Constructor
     */
  constructor(

    private _instagramService: InstagramService,
    private _changeDetectorRef: ChangeDetectorRef,) {
  }
  @Input() instagramPostId: string;
  // -----------------------------------------------------------------------------------------------------
  // @ Lifecycle hooks
  // -----------------------------------------------------------------------------------------------------

  /**
   * On init
   */
  ngOnInit(): void {
    /** Instagram */
    this._instagramService.posts$.subscribe((responseData: any) => {
      const latestPost = responseData.data[0]; // Obtenir le premier élément du tableau des posts
      this.instagramPosts = latestPost; // Assigner directement le dernier post
      console.log(JSON.stringify(this.instagramPosts));
      this._changeDetectorRef.markForCheck();
    });
  }
}

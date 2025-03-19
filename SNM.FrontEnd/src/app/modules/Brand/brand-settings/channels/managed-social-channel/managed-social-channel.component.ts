import { Component,ChangeDetectorRef, } from '@angular/core';
import { MatDialogRef,MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Subject, takeUntil } from 'rxjs';
import { Inject} from '@angular/core';
import { ChannelProfile } from 'app/core/models/ChannelProfile.model';
import { SocialChannelService } from 'app/core/services-api/socialChannel.service';
import { SocialChannel } from 'app/core/models/socialChannel.model';
import { ToastrService } from 'ngx-toastr';
import { ChannelType } from 'app/core/models/channelType.model';
import { ChannelTypeService } from 'app/core/services-api/channelType.service';
import { IChannelType } from 'app/core/types/channelType.types';
import { ActivationStatus } from 'app/core/enumerations/ActivationStatus';
@Component({
  selector: 'app-managed-social-channel',
  templateUrl: './managed-social-channel.component.html',
  styleUrls: ['./managed-social-channel.component.scss']
})
export class ManagedSocialChannelComponent {
profile:any
channelTypeList: IChannelType[];
private _unsubscribeAll: Subject<any> = new Subject<any>();
selectedPages: any[] = [];
/**
 * Constructor
 */
constructor(private _matDialogRef: MatDialogRef<ManagedSocialChannelComponent>, 
   @Inject(MAT_DIALOG_DATA) public data: DialogData,
   private _channelService:SocialChannelService,
   public toastr:ToastrService,
   private _channelTypesService:ChannelTypeService,
   private _changeDetectorRef: ChangeDetectorRef){}
ngOnInit(): void
{

  
}
updateSelectedPages(page: any, index: number) {
  if (page.selected) {
    page.isActivated=ActivationStatus.Active
    // If the page is selected, add it to the selectedPages array
    this.selectedPages.push(page);
    console.log(this.selectedPages+"pageselect")
  } else {
    // If the page is deselected, remove it from the selectedPages array
    const pageIndex = this.selectedPages.findIndex(selectedPage => selectedPage === page);
    if (pageIndex !== -1) {
      this.selectedPages.splice(pageIndex, 1);
    }
  }
}
updateSelectedChannelProfile(channelProfile: any, index: number) {
  if (channelProfile.selected) {
    channelProfile.isActivate = ActivationStatus.Active;
    // If the channelProfile is selected, add it to the selectedPages array
    this.selectedPages.push(channelProfile);
    console.log(this.selectedPages + " pageselect");
  } else {
    // If the channelProfile is deselected, remove it from the selectedPages array
    const channelProfileIndex = this.selectedPages.findIndex(selectedPage => selectedPage === channelProfile);
    if (channelProfileIndex !== -1) {
      this.selectedPages.splice(channelProfileIndex, 1);
    }
  }
}
save() {
 console.log(this.selectedPages+"ppppp")
this._channelService.UpdateSocialChannels(this.selectedPages).subscribe(()=>
this._matDialogRef.close()

)

}
// UpdatePage(channel:SocialChannel){
//   console.log(channel+"ccccc")
// this._channelService.UpdateSocialChannels(channel).subscribe(()=>
// this.toastr.success("Page Added Successfully")
// )
// }
// Close the dialog
close() { 
  this._matDialogRef.close();
}

ngOnDestroy(): void {
  console.log("distroyed");
  // Unsubscribe from all subscriptions
  this._unsubscribeAll.next(null);
  this._unsubscribeAll.complete();

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
}
export interface DialogData {
  channelProfile:any
  name: string;

}
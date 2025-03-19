import { Component ,ChangeDetectorRef, ViewChild,ElementRef} from '@angular/core';
import { MatTabChangeEvent } from '@angular/material/tabs';
import { Brand } from 'app/core/models/brand.model';
import { ChannelType } from 'app/core/models/channelType.model';
import { FacebookService } from 'app/core/services-api';
import { BrandsService } from 'app/core/services-api/brands.service';
import { ChannelTypeService } from 'app/core/services-api/channelType.service';
import { InstagramService } from 'app/core/services-api/instagram.service';
import { SocialChannelService } from 'app/core/services-api/socialChannel.service';
import { IChannelType } from 'app/core/types/channelType.types';
import { UserService } from 'app/core/user/user.service';
import { User } from 'app/core/user/user.types';
import { Observable, Subject, map, takeUntil } from 'rxjs';
import { ChartType } from 'chart.js';
import { Chart,registerables } from 'chart.js';
@Component({
  selector: 'app-summary',
  templateUrl: './summary.component.html',
  styleUrls: ['./summary.component.scss']
})
export class SummaryComponent {
  private _unsubscribeAll: Subject<any> = new Subject<any>();
  @ViewChild('myChart') myChart: ElementRef; 
  chart: any;
  currentBrand:Brand;
  user: User;
   selectedTabIndex: number = 0;
  selectedTabId: string="";
  selectedTablabel: string;

  channelTypeList: IChannelType[]= [];
  summary:any;
  constructor( 
    private _brandsService: BrandsService,
    private _instgramService:InstagramService,
    private _channelTypesService:ChannelTypeService,
    private _changeDetectorRef: ChangeDetectorRef,
    private _userService: UserService ,
    private _socialChannelService:SocialChannelService, private _facebookService:FacebookService,){}
 
  ngOnInit(): void {
  this._brandsService.brand$
  .pipe(takeUntil(this._unsubscribeAll))
  .subscribe((brand: Brand) => {
    // Get the brandName
    this.currentBrand = brand;
   
    this._channelTypesService.getChannelTypes(this.currentBrand?.id).subscribe();
    // Mark for check
    this._changeDetectorRef.markForCheck();
  });
 
 

 this._channelTypesService.channelTypes$
 .pipe(takeUntil(this._unsubscribeAll))
 .subscribe((channels: ChannelType[]) => {
   
    this.channelTypeList = channels
  
   
    this.selectedTabId = this.channelTypeList[this.selectedTabIndex].id;
    this.selectedTablabel=this.channelTypeList?.[this.selectedTabIndex]?.name;
  
    if(this.selectedTabId!=""){
      this.getSummary(this.selectedTabId)
    }
    // Mark for check
    this._changeDetectorRef.markForCheck();
 });
      this._userService.user$
      .pipe(takeUntil(this._unsubscribeAll))
      .subscribe((user: User) => {
          this.user = user;
          console.log(this.user)
          // Mark for check
          this._changeDetectorRef.markForCheck();
      });

 
      const user: User = JSON.parse(localStorage.getItem('user'));
      if(user)
      {
      this._userService.user = user;
      }
     

// Subscribe to changes in the selected tab label
// this.getSelectedTabLabel();
}
getSelectedTabLabel() {
  console.log( this.channelTypeList?.[this.selectedTabIndex]?.name)
  this.selectedTabId = this.channelTypeList?.[this.selectedTabIndex]?.name;
  console.log(this.selectedTabId)
}
onTabChange(event: MatTabChangeEvent): void {

    // Detect changes to selectedTabIndex and update accordingly
    this.selectedTabId = this.channelTypeList[this.selectedTabIndex].id;
    this.selectedTablabel = this.channelTypeList?.[this.selectedTabIndex]?.name;
    console.log('Selected tab index changed:', this.selectedTabIndex);
    this.getSummary(this.selectedTabId)
    
  
  this._changeDetectorRef.detectChanges();
}
getSummary(channeltypeId:any) {
  this._socialChannelService.getSocialChannelsByChannelTypeId( channeltypeId,this.currentBrand?.id).subscribe(channels=>{
channels.forEach(channel => {
  if(this.selectedTablabel=='Facebook Page'){
    this._facebookService.getFacebookSummary(channel.id).subscribe((result)=>{
      if(result.succeeded){
        this.summary=result.data
        this._changeDetectorRef.detectChanges();
      }
  })
  }
        
        if(this.selectedTablabel=='Facebook Group'){
          this._facebookService.getFacebookGroupSummary(channel.id).subscribe((result)=>{
            if(result.succeeded){
              this.summary=result.data
              this._changeDetectorRef.detectChanges();
            }
        })
        }
       
       

        if(this.selectedTablabel=='Instagram Profile') {
          this._instgramService.getInstagramSummary(channel.id).subscribe((result)=>{
            if(result.succeeded){
              console.log(JSON.stringify(result.data))
              this.summary=result.data     
              this.summary.photo = channel.photo;
              
              this.summary.name = channel.displayName;
              this._changeDetectorRef.detectChanges();
            }
            console.log(JSON.stringify(this.summary))
        })
        }
            


        if(this.selectedTablabel == 'LinkedIn Page'){

        }
});
    
           
   
    })
}
RenderChart(labeldata:any) {
  this.chart = new Chart('myChart', {
      type: 'pie',
      data: {
        labels: labeldata,
        datasets: [{
          label: 'Recomended Macros',
          data: [ ],
          backgroundColor: [
              '#fe5656',
              '#3eb8ee',
              '#edee3e',
          ],
          borderColor: [
            'rgba(255, 99, 132, 1)',
            'rgba(54, 162, 235, 1)',
            'rgba(255, 206, 86, 1)',
            'rgba(75, 192, 192, 1)',
            'rgba(153, 102, 255, 1)',
            'rgba(255, 159, 64, 1)',
            'rgba(255, 99, 132, 1)'
          ],
          borderWidth: 1
        }],
        
      },     
    });
  }
}

// import { ChangeDetectionStrategy, ChangeDetectorRef, Component, ElementRef, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
// import { Router } from '@angular/router';
// import { Subject, takeUntil } from 'rxjs';
// import { ApexOptions } from 'ng-apexcharts';
// import { AnalyticsService } from 'app/modules/analytics/analytics.service';


// import domToImage from 'dom-to-image';
// import jsPDF, { jsPDFOptions } from 'jspdf';
// import moment from 'moment';
// import { LinkedInChannelDto } from 'app/core/models/brand.model';
// import { LinkedInService } from 'app/core/services-api/linkedin.service';
// import { LinkedInChannel } from 'app/core/models/linkedinChannel';


// @Component({
//     selector       : 'analytics',
//     templateUrl    : './analytics.component.html',
//     encapsulation  : ViewEncapsulation.None,
//     changeDetection: ChangeDetectionStrategy.OnPush
// })
// export class AnalyticsComponent implements OnInit, OnDestroy
// {
//     chartVisitors: ApexOptions;
//     chartConversions: ApexOptions;
//     chartImpressions: ApexOptions;
//     chartVisits: ApexOptions;
//     chartVisitorsVsPageViews: ApexOptions;
//     chartNewVsReturning: ApexOptions;
//     chartGender: ApexOptions;
//     chartAge: ApexOptions;
//     chartLanguage: ApexOptions;
//     data: any;

//     private _unsubscribeAll: Subject<any> = new Subject<any>();
//     private commentsValueData: any[] = [];
//     private commentsLabels: any[] = [];

//     private impressionsValueData: any[] = [];
//     private impressionsLabels: any[] = [];

//     private sharesValueData: any[] = [];
//     private sharesLabels: any[] = [];

//     defaultSelectedPage: string;
//     /**
//      * Constructor
//      */
//     constructor(
//         private _analyticsService: AnalyticsService,
//         private _router: Router,
//         private _linkedinService: LinkedInService,
//         private _changeDetectorRef: ChangeDetectorRef
//     )
//     {
//     }

//     // -----------------------------------------------------------------------------------------------------
//     // @ Lifecycle hooks
//     // -----------------------------------------------------------------------------------------------------

//     /**
//      * On init
//      */
//     ngOnInit(): void
//     {
//         this.getCompanyPages();
//         // Get the data
//         this._analyticsService.data$
//             .pipe(takeUntil(this._unsubscribeAll))
//             .subscribe((data) => {

//                 // Store the data
//                 this.data = data;

//                 console.log(this.data)

//                  //prepare data
//                 console.log(this.data.commentsdata)
//                 if(this.data.commentsdata!=null) {
//                     for(let i=0; i<this.data.commentsdata.length; i++) {
                        
//                         this.commentsValueData.push(this.data.commentsdata[i].y);
//                         this.commentsLabels.push(this.data.commentsdata[i].x)
//                     }
//                 }

//                 console.log(this.data.impressionsdata)
//                 if(this.data.impressionsdata!=null) {
//                     for(let i=0; i<this.data.impressionsdata.length; i++) {
                        
//                         this.impressionsValueData.push(this.data.impressionsdata[i].y);
//                         this.impressionsLabels.push(this.data.impressionsdata[i].x)
//                     }
//                 }

//                 console.log(this.data.sharedata)
//                 if(this.data.sharedata!=null) {
//                     for(let i=0; i<this.data.sharedata.length; i++) {
                        
//                         this.sharesValueData.push(this.data.sharedata[i].y);
//                         this.sharesLabels.push(this.data.sharedata[i].x)
//                     }
//                 }


//                 // Prepare the chart data
//                 this._prepareChartData();

//             });

           

//         // Attach SVG fill fixer to all ApexCharts
//         window['Apex'] = {
//             chart: {
//                 events: {
//                     mounted: (chart: any, options?: any): void => {
//                         this._fixSvgFill(chart.el);
//                     },
//                     updated: (chart: any, options?: any): void => {
//                         this._fixSvgFill(chart.el);
//                     }
//                 }
//             }
//         };
//     }

//     /**
//      * On destroy
//      */
//     ngOnDestroy(): void
//     {
//         // Unsubscribe from all subscriptions
//         this._unsubscribeAll.next(null);
//         this._unsubscribeAll.complete();
//     }

//     // -----------------------------------------------------------------------------------------------------
//     // @ Public methods
//     // -----------------------------------------------------------------------------------------------------

//     /**
//      * Track by function for ngFor loops
//      *
//      * @param index
//      * @param item
//      */
//     trackByFn(index: number, item: any): any
//     {
//         return item.id || index;
//     }


//     selectedPage: LinkedInChannelDto;
//     commentCount: number;
//     shareCount: number;
//     impressionsCount: number;

//     // generateStats(): void{
//     //     console.log(this.socialChannels)
//     //     this.selectedPage = this.socialChannels.find(page => page.companyUrn === this._analyticsService.OrgUrn);

//     //     this.commentCount = this.selectedPage?.insight.totalComments
//     //     this.shareCount = this.selectedPage?.insight.shareCount;
//     //     this.impressionsCount = this.selectedPage?.insight.impressionCount
//     // }

// 	onSelected(value:string): void {
// 		this.selectedPage = this.socialChannels.find(page => page.companyName === value);;
//         console.log(this.selectedPage)
//         this._analyticsService.OrgUrn = this.selectedPage?.companyUrn;
//         console.log(this._analyticsService.OrgUrn)

//         if(this.selectedPage)
//         {
//             this.commentCount = this.selectedPage?.insight.totalComments
//             this.shareCount = this.selectedPage?.insight.shareCount;
//             this.impressionsCount = this.selectedPage?.insight.impressionCount
//         }    
//         else 
//         {
//             this.commentCount = 0;
//             this.shareCount = 0;
//             this.impressionsCount = 0;
//         }
//     }

    

//     socialChannels: LinkedInChannelDto[];

//     /**
//      * LinkedIn Company Pages
//      */
//     getCompanyPages()
//     {
//       this._linkedinService.getCompanyPages().subscribe(
//         (pages: LinkedInChannelDto[]) => {
//           this.socialChannels = pages;
//           // Mark for check
//          this._changeDetectorRef.markForCheck()});
//     }
    

//     /**
//      * Download Dashboard
//      */

//     pdfName: string = 'dashboard_analytics'
//     @ViewChild('pdfTable', { static: false })
//     pdfTable!: ElementRef;
    
//     public downloadAsPDF(): void {
//         const width = this.pdfTable.nativeElement.clientWidth;
//         const height = this.pdfTable.nativeElement.clientHeight + 40;
//         let orientation: "p" | "portrait" | "l" | "landscape";
//         let imageUnit = 'pt';

//         if (width > height) {
//         orientation = 'l';
//         } else {
//         orientation = 'p';
//         }
//         domToImage
//         .toPng(this.pdfTable.nativeElement, {
//         width: width,
//         height: height
//         })
//         .then(result => {

//             let jsPdfOptions: jsPDFOptions = {
//                 orientation: orientation as jsPDFOptions["orientation"],
//                 unit: imageUnit as jsPDFOptions["unit"],
//                 format: [width + 50, height + 220],
//               };

//         const pdf = new jsPDF(jsPdfOptions);

//         pdf.setFontSize(35);
//         pdf.setTextColor('#8d5bf0');
//         pdf.setFont('times', 'bold');
//         pdf.text(this.pdfName? this.pdfName.toUpperCase() : 'Untitled dashboard'.toUpperCase(), 25, 75);

//         pdf.setFont('times', 'normal');
//         pdf.setFontSize(20);
//         pdf.setTextColor('#131523');
//         pdf.text('Report date: ' + moment().format('ll'), 25, 115);
//         pdf.addImage(result, 'PNG', 25, 185, width, height);
//         pdf.save('file_name'+ '.pdf');
//         })
//         .catch(error => {
//             console.log('Error while exporting to PDF')
//         });
//         }

//     // -----------------------------------------------------------------------------------------------------
//     // @ Private methods
//     // -----------------------------------------------------------------------------------------------------

//     /**
//      * Fix the SVG fill references. This fix must be applied to all ApexCharts
//      * charts in order to fix 'black color on gradient fills on certain browsers'
//      * issue caused by the '<base>' tag.
//      *
//      * Fix based on https://gist.github.com/Kamshak/c84cdc175209d1a30f711abd6a81d472
//      *
//      * @param element
//      * @private
//      */
//     private _fixSvgFill(element: Element): void
//     {
//         // Current URL
//         const currentURL = this._router.url;

//         // 1. Find all elements with 'fill' attribute within the element
//         // 2. Filter out the ones that doesn't have cross reference so we only left with the ones that use the 'url(#id)' syntax
//         // 3. Insert the 'currentURL' at the front of the 'fill' attribute value
//         Array.from(element.querySelectorAll('*[fill]'))
//              .filter(el => el.getAttribute('fill').indexOf('url(') !== -1)
//              .forEach((el) => {
//                  const attrVal = el.getAttribute('fill');
//                  el.setAttribute('fill', `url(${currentURL}${attrVal.slice(attrVal.indexOf('#'))}`);
//              });
//     }

//     /**
//      * Prepare the chart data from the data
//      *
//      * @private
//      */
//     private _prepareChartData(): void
//     {
//         // Views
//         this.chartVisitors = {
//             chart     : {
//                 animations: {
//                     speed           : 400,
//                     animateGradually: {
//                         enabled: false
//                     }
//                 },
//                 fontFamily: 'inherit',
//                 foreColor : 'inherit',
//                 width     : '100%',
//                 height    : '100%',
//                 type      : 'area',
//                 toolbar   : {
//                     show: false
//                 },
//                 zoom      : {
//                     enabled: false
//                 }
//             },
//             colors    : ['#818CF8'],
//             dataLabels: {
//                 enabled: false
//             },
//             fill      : {
//                 colors: ['#312E81']
//             },
//             grid      : {
//                 show       : true,
//                 borderColor: '#334155',
//                 padding    : {
//                     top   : 10,
//                     bottom: -40,
//                     left  : 0,
//                     right : 0
//                 },
//                 position   : 'back',
//                 xaxis      : {
//                     lines: {
//                         show: true
//                     }
//                 }
//             },
//             series : [ 
//                 {
//                     name: 'Views',
//                     data: this.data.viewsdata
//                 }

//             ],
//             stroke    : {
//                 width: 2
//             },
//             tooltip   : {
//                 followCursor: true,
//                 theme       : 'dark',
//                 x           : {
//                     format: 'MMM dd, yyyy'
//                 },
//                 y           : {
//                     formatter: (value: number): string => `${value}`
//                 }
//             },
//             xaxis     : {
//                 axisBorder: {
//                     show: false
//                 },
//                 axisTicks : {
//                     show: false
//                 },
//                 crosshairs: {
//                     stroke: {
//                         color    : '#475569',
//                         dashArray: 0,
//                         width    : 2
//                     }
//                 },
//                 labels    : {
//                     offsetY: -20,
//                     style  : {
//                         colors: '#CBD5E1'
//                     }
//                 },
//                 tickAmount: 20,
//                 tooltip   : {
//                     enabled: false
//                 },
//                 type      : 'datetime'
//             },
//             yaxis     : {
//                 axisTicks : {
//                     show: false
//                 },
//                 axisBorder: {
//                     show: false
//                 },
//                 min       : (min): number => min - 750,
//                 max       : (max): number => max + 250,
//                 tickAmount: 5,
//                 show      : false
//             }
//         };

//         // Comments
//         this.chartConversions = {
//             chart  : {
//                 animations: {
//                     enabled: false
//                 },
//                 fontFamily: 'inherit',
//                 foreColor : 'inherit',
//                 height    : '100%',
//                 type      : 'area',
//                 sparkline : {
//                     enabled: true
//                 }
//             },
//             colors : ['#38BDF8'],
//             fill   : {
//                 colors : ['#38BDF8'],
//                 opacity: 0.5
//             },
//             series: [
//                 {
//                     name: 'Comments',
//                     data: this.commentsValueData
//                 }
//             ],
//             stroke : {
//                 curve: 'smooth'
//             },
//             tooltip: {
//                 followCursor: true,
//                 theme       : 'dark'
//             },
//             xaxis  : {
//                 type      : 'category',
//                 categories: this.commentsLabels
//             },
//             yaxis  : {
//                 labels: {
//                     formatter: (val): string => val.toString()
//                 }
//             }
//         };

//         // Impressions
//         this.chartImpressions = {
//             chart  : {
//                 animations: {
//                     enabled: false
//                 },
//                 fontFamily: 'inherit',
//                 foreColor : 'inherit',
//                 height    : '100%',
//                 type      : 'area',
//                 sparkline : {
//                     enabled: true
//                 }
//             },
//             colors : ['#34D399'],
//             fill   : {
//                 colors : ['#34D399'],
//                 opacity: 0.5
//             },
//             series: [
//                 {
//                     name: 'Impressions',
//                     data: this.impressionsValueData
//                 }
//             ],
//             stroke : {
//                 curve: 'smooth'
//             },
//             tooltip: {
//                 followCursor: true,
//                 theme       : 'dark'
//             },
//             xaxis  : {
//                 type      : 'category',
//                 categories: this.impressionsLabels
//             },
//             yaxis  : {
//                 labels: {
//                     formatter: (val): string => val.toString()
//                 }
//             }
//         };

//         // Shares
//         this.chartVisits = {
//             chart  : {
//                 animations: {
//                     enabled: false
//                 },
//                 fontFamily: 'inherit',
//                 foreColor : 'inherit',
//                 height    : '100%',
//                 type      : 'area',
//                 sparkline : {
//                     enabled: true
//                 }
//             },
//             colors : ['#FB7185'],
//             fill   : {
//                 colors : ['#FB7185'],
//                 opacity: 0.5
//             },
//             series: [
//                 {
//                     name: 'Shares',
//                     data: this.sharesValueData
//                 }
//             ],
//             stroke : {
//                 curve: 'smooth'
//             },
//             tooltip: {
//                 followCursor: true,
//                 theme       : 'dark'
//             },
//             xaxis  : {
//                 type      : 'category',
//                 categories: this.sharesLabels
//             },
//             yaxis  : {
//                 labels: {
//                     formatter: (val): string => val.toString()
//                 }
//             }
//         };

        
//         // Visitors vs Page Views
//         // this.chartVisitorsVsPageViews = {
//         //     chart     : {
//         //         animations: {
//         //             enabled: false
//         //         },
//         //         fontFamily: 'inherit',
//         //         foreColor : 'inherit',
//         //         height    : '100%',
//         //         type      : 'area',
//         //         toolbar   : {
//         //             show: false
//         //         },
//         //         zoom      : {
//         //             enabled: false
//         //         }
//         //     },
//         //     colors    : ['#64748B', '#94A3B8'],
//         //     dataLabels: {
//         //         enabled: false
//         //     },
//         //     fill      : {
//         //         colors : ['#64748B', '#94A3B8'],
//         //         opacity: 0.5
//         //     },
//         //     grid      : {
//         //         show   : false,
//         //         padding: {
//         //             bottom: -40,
//         //             left  : 0,
//         //             right : 0
//         //         }
//         //     },
//         //     legend    : {
//         //         show: false
//         //     },
//         //     series    : this.data.visitorsVsPageViews.series,
//         //     stroke    : {
//         //         curve: 'smooth',
//         //         width: 2
//         //     },
//         //     tooltip   : {
//         //         followCursor: true,
//         //         theme       : 'dark',
//         //         x           : {
//         //             format: 'MMM dd, yyyy'
//         //         }
//         //     },
//         //     xaxis     : {
//         //         axisBorder: {
//         //             show: false
//         //         },
//         //         labels    : {
//         //             offsetY: -20,
//         //             rotate : 0,
//         //             style  : {
//         //                 colors: 'var(--fuse-text-secondary)'
//         //             }
//         //         },
//         //         tickAmount: 3,
//         //         tooltip   : {
//         //             enabled: false
//         //         },
//         //         type      : 'datetime'
//         //     },
//         //     yaxis     : {
//         //         labels    : {
//         //             style: {
//         //                 colors: 'var(--fuse-text-secondary)'
//         //             }
//         //         },
//         //         max       : (max): number => max + 250,
//         //         min       : (min): number => min - 250,
//         //         show      : false,
//         //         tickAmount: 5
//         //     }
//         // };

//         // // New vs. returning
//         // this.chartNewVsReturning = {
//         //     chart      : {
//         //         animations: {
//         //             speed           : 400,
//         //             animateGradually: {
//         //                 enabled: false
//         //             }
//         //         },
//         //         fontFamily: 'inherit',
//         //         foreColor : 'inherit',
//         //         height    : '100%',
//         //         type      : 'donut',
//         //         sparkline : {
//         //             enabled: true
//         //         }
//         //     },
//         //     colors     : ['#3182CE', '#63B3ED'],
//         //     labels     : this.data.newVsReturning.labels,
//         //     plotOptions: {
//         //         pie: {
//         //             customScale  : 0.9,
//         //             expandOnClick: false,
//         //             donut        : {
//         //                 size: '70%'
//         //             }
//         //         }
//         //     },
//         //     series     : this.data.newVsReturning.series,
//         //     states     : {
//         //         hover : {
//         //             filter: {
//         //                 type: 'none'
//         //             }
//         //         },
//         //         active: {
//         //             filter: {
//         //                 type: 'none'
//         //             }
//         //         }
//         //     },
//         //     tooltip    : {
//         //         enabled        : true,
//         //         fillSeriesColor: false,
//         //         theme          : 'dark',
//         //         custom         : ({
//         //                               seriesIndex,
//         //                               w
//         //                           }): string => `<div class="flex items-center h-8 min-h-8 max-h-8 px-3">
//         //                                             <div class="w-3 h-3 rounded-full" style="background-color: ${w.config.colors[seriesIndex]};"></div>
//         //                                             <div class="ml-2 text-md leading-none">${w.config.labels[seriesIndex]}:</div>
//         //                                             <div class="ml-2 text-md font-bold leading-none">${w.config.series[seriesIndex]}%</div>
//         //                                         </div>`
//         //     }
//         // };

//         // // Gender
//         // this.chartGender = {
//         //     chart      : {
//         //         animations: {
//         //             speed           : 400,
//         //             animateGradually: {
//         //                 enabled: false
//         //             }
//         //         },
//         //         fontFamily: 'inherit',
//         //         foreColor : 'inherit',
//         //         height    : '100%',
//         //         type      : 'donut',
//         //         sparkline : {
//         //             enabled: true
//         //         }
//         //     },
//         //     colors     : ['#319795', '#4FD1C5'],
//         //     labels     : this.data.gender.labels,
//         //     plotOptions: {
//         //         pie: {
//         //             customScale  : 0.9,
//         //             expandOnClick: false,
//         //             donut        : {
//         //                 size: '70%'
//         //             }
//         //         }
//         //     },
//         //     series     : this.data.gender.series,
//         //     states     : {
//         //         hover : {
//         //             filter: {
//         //                 type: 'none'
//         //             }
//         //         },
//         //         active: {
//         //             filter: {
//         //                 type: 'none'
//         //             }
//         //         }
//         //     },
//         //     tooltip    : {
//         //         enabled        : true,
//         //         fillSeriesColor: false,
//         //         theme          : 'dark',
//         //         custom         : ({
//         //                               seriesIndex,
//         //                               w
//         //                           }): string => `<div class="flex items-center h-8 min-h-8 max-h-8 px-3">
//         //                                              <div class="w-3 h-3 rounded-full" style="background-color: ${w.config.colors[seriesIndex]};"></div>
//         //                                              <div class="ml-2 text-md leading-none">${w.config.labels[seriesIndex]}:</div>
//         //                                              <div class="ml-2 text-md font-bold leading-none">${w.config.series[seriesIndex]}%</div>
//         //                                          </div>`
//         //     }
//         // };

//         // // Age
//         // this.chartAge = {
//         //     chart      : {
//         //         animations: {
//         //             speed           : 400,
//         //             animateGradually: {
//         //                 enabled: false
//         //             }
//         //         },
//         //         fontFamily: 'inherit',
//         //         foreColor : 'inherit',
//         //         height    : '100%',
//         //         type      : 'donut',
//         //         sparkline : {
//         //             enabled: true
//         //         }
//         //     },
//         //     colors     : ['#DD6B20', '#F6AD55'],
//         //     labels     : this.data.age.labels,
//         //     plotOptions: {
//         //         pie: {
//         //             customScale  : 0.9,
//         //             expandOnClick: false,
//         //             donut        : {
//         //                 size: '70%'
//         //             }
//         //         }
//         //     },
//         //     series     : this.data.age.series,
//         //     states     : {
//         //         hover : {
//         //             filter: {
//         //                 type: 'none'
//         //             }
//         //         },
//         //         active: {
//         //             filter: {
//         //                 type: 'none'
//         //             }
//         //         }
//         //     },
//         //     tooltip    : {
//         //         enabled        : true,
//         //         fillSeriesColor: false,
//         //         theme          : 'dark',
//         //         custom         : ({
//         //                               seriesIndex,
//         //                               w
//         //                           }): string => `<div class="flex items-center h-8 min-h-8 max-h-8 px-3">
//         //                                             <div class="w-3 h-3 rounded-full" style="background-color: ${w.config.colors[seriesIndex]};"></div>
//         //                                             <div class="ml-2 text-md leading-none">${w.config.labels[seriesIndex]}:</div>
//         //                                             <div class="ml-2 text-md font-bold leading-none">${w.config.series[seriesIndex]}%</div>
//         //                                         </div>`
//         //     }
//         // };

//         // // Language
//         // this.chartLanguage = {
//         //     chart      : {
//         //         animations: {
//         //             speed           : 400,
//         //             animateGradually: {
//         //                 enabled: false
//         //             }
//         //         },
//         //         fontFamily: 'inherit',
//         //         foreColor : 'inherit',
//         //         height    : '100%',
//         //         type      : 'donut',
//         //         sparkline : {
//         //             enabled: true
//         //         }
//         //     },
//         //     colors     : ['#805AD5', '#B794F4'],
//         //     labels     : this.data.language.labels,
//         //     plotOptions: {
//         //         pie: {
//         //             customScale  : 0.9,
//         //             expandOnClick: false,
//         //             donut        : {
//         //                 size: '70%'
//         //             }
//         //         }
//         //     },
//         //     series     : this.data.language.series,
//         //     states     : {
//         //         hover : {
//         //             filter: {
//         //                 type: 'none'
//         //             }
//         //         },
//         //         active: {
//         //             filter: {
//         //                 type: 'none'
//         //             }
//         //         }
//         //     },
//         //     tooltip    : {
//         //         enabled        : true,
//         //         fillSeriesColor: false,
//         //         theme          : 'dark',
//         //         custom         : ({
//         //                               seriesIndex,
//         //                               w
//         //                           }): string => `<div class="flex items-center h-8 min-h-8 max-h-8 px-3">
//         //                                             <div class="w-3 h-3 rounded-full" style="background-color: ${w.config.colors[seriesIndex]};"></div>
//         //                                             <div class="ml-2 text-md leading-none">${w.config.labels[seriesIndex]}:</div>
//         //                                             <div class="ml-2 text-md font-bold leading-none">${w.config.series[seriesIndex]}%</div>
//         //                                         </div>`
//         //     }
//         // };
//     }
// }

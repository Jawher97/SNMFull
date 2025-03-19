import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnDestroy, OnInit, ViewChild, ViewEncapsulation } from '@angular/core';
import { MatDrawer } from '@angular/material/sidenav';
import { FuseNavigationItem } from '@fuse/components/navigation';
import { FuseMediaWatcherService } from '@fuse/services/media-watcher';
import { Subject, takeUntil } from 'rxjs';

@Component({
  selector: 'app-brand',
  templateUrl: './brand.component.html',
  styleUrls: ['./brand.component.scss'],
  encapsulation  : ViewEncapsulation.None,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class BrandComponent  implements OnInit, OnDestroy
{
    @ViewChild('matDrawer', {static: true}) matDrawer: MatDrawer;
    drawerMode: 'side' | 'over';
    drawerOpened: boolean;
    menuData: FuseNavigationItem[];
    private _unsubscribeAll: Subject<any> = new Subject<any>();

    /**
     * Constructor
     */
    constructor(
        private _changeDetectorRef: ChangeDetectorRef,
        private _fuseMediaWatcherService: FuseMediaWatcherService
    )
    {
        this.menuData = [
            {
                id      : 'other-components.common',
                title   : 'BRAND SETTINGS',
                subtitle: '',
                type    : 'group',
                children: [
                    {
                        id   : 'other-components.common.overview',
                        title: 'Brand Information',
                        type : 'basic',
                        link : './brand-information'
                    },
                   
                    {
                        id   : 'other-components.common.languages',
                        title: 'Social Channels',
                        type : 'basic',
                        link : './channels'
                    },
                    {
                        id      : 'Posts',
                        title   : 'Posts',
                        type    : 'basic',
                        // icon    : 'heroicons_outline:qrcode',
                        link : './Posts'
                        // children: [
                        //     {
                        //         id   : 'linkedin.post',
                        //         title: 'linkedin',
                        //         type : 'basic',
                        //         link : '/posts/linkedin-posts'
                        //     }
                        // ] // This will be filled from defaultNavigation so we don't have to manage multiple sets of the same navigation
                    }, {
                        id      : 'Summary',
                        title   : 'Summary',
                        type    : 'basic',
                        // icon    : 'heroicons_outline:qrcode',
                        link : './Summary'
                        // children: [
                        //     {
                        //         id   : 'linkedin.post',
                        //         title: 'linkedin',
                        //         type : 'basic',
                        //         link : '/posts/linkedin-posts'
                        //     }
                        // ] // This will be filled from defaultNavigation so we don't have to manage multiple sets of the same navigation
                    },
                    {
                        id   : 'other-components.common.messages',
                        title: 'Team Members',
                        type : 'basic',
                        link : 'members'
                    },
                    {
                        id   : 'other-components.common.messages',
                        title: 'Publishing',
                        type : 'basic',
                        link : 'publishing'
                    },
                    {
                        id   : 'other-components.common.messages',
                        title: 'Roles & Permissions',
                        type : 'basic',
                        link : 'roles-permissions'
                    },
                    {
                        id   : 'other-components.common.notifications',
                        title: 'Notifications',
                        type : 'basic',
                        link : 'notifications'
                    }
                ]
            },
            {
                id  : 'other-components.divider-1',
                type: 'divider'
            },
            {
                id      : 'other-components.third-party',
                title:'GENERAL SETTINGS',
                subtitle: '',
                type    : 'group',
                children: [
                    {
                        id   : 'other-components.third-party.apex-charts',
                        title: 'Preference',
                        type : 'basic',
                        link : '/ui/other-components/third-party/apex-charts'
                    },
                    {
                        id   : 'other-components.third-party.quill-editor',
                        title: 'All Members',
                        type : 'basic',
                        link : '/ui/other-components/third-party/quill-editor'
                    },
                    {
                        id   : 'other-components.third-party.quill-editor',
                        title: 'Portal Settings',
                        type : 'basic',
                        link : '/settings'
                    },
                    {
                        id   : 'other-components.third-party.quill-editor',
                        title: 'Audit Log',
                        type : 'basic',
                        link : '/activity'
                    }
                ]
            }
        ];
    }

    // -----------------------------------------------------------------------------------------------------
    // @ Lifecycle hooks
    // -----------------------------------------------------------------------------------------------------

    /**
     * On init
     */
    ngOnInit(): void
    {
        // Subscribe to media query change
        this._fuseMediaWatcherService.onMediaChange$
            .pipe(takeUntil(this._unsubscribeAll))
            .subscribe(({matchingAliases}) => {

                // Set the drawerMode and drawerOpened
                if ( matchingAliases.includes('md') )
                {
                    this.drawerMode = 'side';
                    this.drawerOpened = true;
                }
                else
                {
                    this.drawerMode = 'over';
                    this.drawerOpened = false;
                }

                // Mark for check
                this._changeDetectorRef.markForCheck();
            });
    }

    /**
     * On destroy
     */
    ngOnDestroy(): void
    {
        // Unsubscribe from all subscriptions
        this._unsubscribeAll.next(null);
        this._unsubscribeAll.complete();
    }
}


/* tslint:disable:max-line-length */
import { FuseNavigationItem } from '@fuse/components/navigation';

export const defaultNavigation: FuseNavigationItem[] = [
    {
        id   : 'example',
        title: 'Example',
        type : 'basic',
        icon : 'heroicons_outline:chart-pie',
        link : '/example'
    }
];
export const compactNavigation: FuseNavigationItem[] = [
    {
        id   : 'example',
        title: 'Example',
        type : 'basic',
        icon : 'heroicons_outline:chart-pie',
        link : '/example'
    }
];
export const futuristicNavigation: FuseNavigationItem[] = [
    {
        id   : 'example',
        title: 'Example',
        type : 'basic',
        icon : 'heroicons_outline:chart-pie',
        link : '/example'
    }
];
export const horizontalNavigation: FuseNavigationItem[] = [
    {
        id      : 'brand.dashboards',
        title   : 'Dashboards',
        type    : 'basic',
        icon    : 'heroicons_outline:home',
        link : '/brand/home',
        children: [
            {
                id   : 'brand.home',
                title: 'Modern',
                type : 'basic',
                link : '/brand/home'
            }
        ] // This will be filled from defaultNavigation so we don't have to manage multiple sets of the same navigation
    },
    // {
    //     id      : 'post',
    //     title   : 'Posts',
    //     type    : 'basic',
    //     icon    : 'heroicons_outline:qrcode',
    //     link : '/posts'
    //     // children: [] // This will be filled from defaultNavigation so we don't have to manage multiple sets of the same navigation
    // },
    // {
    //     id      : 'Posts',
    //     title   : 'Posts',
    //     type    : 'basic',
    //     icon    : 'heroicons_outline:qrcode',
    //     link : './Posts'
    //     // children: [
    //     //     {
    //     //         id   : 'linkedin.post',
    //     //         title: 'linkedin',
    //     //         type : 'basic',
    //     //         link : '/posts/linkedin-posts'
    //     //     }
    //     // ] // This will be filled from defaultNavigation so we don't have to manage multiple sets of the same navigation
    // }
    
];

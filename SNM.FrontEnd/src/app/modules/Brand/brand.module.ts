import { NgModule } from '@angular/core';
import { Route, RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { SharedModule } from 'app/shared/shared.module';
import { MatDividerModule } from '@angular/material/divider';
import { FuseNavigationModule } from '@fuse/components/navigation';
import { MatIconModule } from '@angular/material/icon';
import { MatSidenavModule } from '@angular/material/sidenav';
import { FuseHighlightModule } from '@fuse/components/highlight';
import { FuseAlertModule } from '@fuse/components/alert';
import { FuseScrollResetModule } from '@fuse/directives/scroll-reset';
import { FuseCardModule } from '@fuse/components/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { BrandResolver } from 'app/core/resolvers/brand.resolvers';
import { BrandComponent } from 'app/modules/Brand/brand.component';
import { HomeComponent } from './brand-settings/home/home.component';
import { PickerModule } from '@ctrl/ngx-emoji-mart';
const brandRoutes: Route[] = [
    {
      path: '',
      component: HomeComponent,
      // resolve  : {
      //   brands : BrandsResolver,
      // }

    }
  ,
  {
    path: ':displayName',
    component: BrandComponent,
    resolve: {
      brand: BrandResolver,
    },
    children: [

      {
        path: '',
        children: [
          { path: '', loadChildren: () => import('app/modules/Brand/brand-settings/brand-settings.module').then(m => m.BrandSettingsModule) }
        ]
      },

    ]
  }

];


@NgModule({
  declarations: [
    BrandComponent,
    HomeComponent,
  ],
  imports: [
    RouterModule.forChild(brandRoutes),
    MatButtonModule,
    MatIconModule,
    MatInputModule,
    MatSidenavModule,
    MatFormFieldModule,
    MatButtonModule,
    MatDividerModule,
    FuseHighlightModule,
    FuseAlertModule,
    FuseNavigationModule,
    FuseScrollResetModule,
    FuseCardModule,
    SharedModule,PickerModule
  ]
})
export class BrandModule { }

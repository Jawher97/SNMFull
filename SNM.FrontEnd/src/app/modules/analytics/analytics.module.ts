import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatButtonModule } from '@angular/material/button';
import { MatButtonToggleModule } from '@angular/material/button-toggle';
import { MatDividerModule } from '@angular/material/divider';
import { MatIconModule } from '@angular/material/icon';
import { MatMenuModule } from '@angular/material/menu';
import { MatProgressBarModule } from '@angular/material/progress-bar';
import { MatSortModule } from '@angular/material/sort';
import { MatTableModule } from '@angular/material/table';
import { MatTooltipModule } from '@angular/material/tooltip';
import { NgApexchartsModule } from 'ng-apexcharts';
import { SharedModule } from 'app/shared/shared.module';
// import { AnalyticsComponent } from 'app/modules/analytics/analytics.component';
import { analyticsRoutes } from 'app/modules/analytics/analytics.routing';
import { MatFormFieldModule } from '@angular/material/form-field';

@NgModule({
    declarations: [
        // AnalyticsComponent
    ],
    imports     : [
        RouterModule.forChild(analyticsRoutes),
        MatButtonModule,
        MatButtonToggleModule,
        MatDividerModule,
        MatFormFieldModule,
        MatIconModule,
        MatMenuModule,
        MatProgressBarModule,
        MatSortModule,
        MatTableModule,
        MatTooltipModule,
        NgApexchartsModule,
        SharedModule
    ]
})
export class AnalyticsModule
{
}

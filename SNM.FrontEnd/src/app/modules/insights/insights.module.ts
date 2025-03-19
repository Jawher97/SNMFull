import { NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { SharedModule } from 'app/shared/shared.module';
import { activitiesRoutes } from 'app/modules/activities/activities.routing';
import { InsightsComponent } from './insights.component';

@NgModule({
    declarations: [
        InsightsComponent
    ],
    imports     : [
        MatIconModule,
        SharedModule
    ]
})
export class InsightsModule
{
}
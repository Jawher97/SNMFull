import { ChangeDetectorRef, NgModule } from '@angular/core';
import { RouterModule } from '@angular/router';
import { MatIconModule } from '@angular/material/icon';
import { SharedModule } from 'app/shared/shared.module';
import { ActivitiesComponent } from 'app/modules/activities/activities.component';
import { activitiesRoutes } from 'app/modules/activities/activities.routing';

@NgModule({
    declarations: [
        ActivitiesComponent
    ],
    
    imports     : [
        RouterModule.forChild(activitiesRoutes),
        MatIconModule,
        SharedModule
    ]
})
export class ActivitiesModule
{
}

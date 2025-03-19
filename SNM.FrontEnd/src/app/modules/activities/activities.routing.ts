import { Route } from '@angular/router';
import { ActivitiesComponent } from 'app/modules/activities/activities.component';
import { ActivitiesResolver } from 'app/modules/activities/activities.resolvers';

export const activitiesRoutes: Route[] = [
    {
        path     : '',
        component: ActivitiesComponent,
        resolve  : {
            activities: ActivitiesResolver
        }
    }
];

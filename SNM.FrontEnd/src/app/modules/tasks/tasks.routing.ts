import { Route } from '@angular/router';
import { CanDeactivateTasksDetails } from 'app/modules/tasks/tasks.guards';
import { TasksResolver, TasksTagsResolver, TasksTaskResolver } from 'app/modules/tasks/tasks.resolvers';
import { TasksComponent } from 'app/modules/tasks/tasks.component';
import { TasksListComponent } from 'app/modules/tasks/list/list.component';
import { TasksDetailsComponent } from 'app/modules/tasks/details/details.component';

export const tasksRoutes: Route[] = [
    {
        path     : '',
        component: TasksComponent,
        resolve  : {
            tags: TasksTagsResolver
        },
        children : [
            {
                path     : '',
                component: TasksListComponent,
                resolve  : {
                    tasks: TasksResolver
                },
                children : [
                    {
                        path         : ':id',
                        component    : TasksDetailsComponent,
                        resolve      : {
                            task: TasksTaskResolver
                        },
                        canDeactivate: [CanDeactivateTasksDetails]
                    }
                ]
            }
        ]
    }
];

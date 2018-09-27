import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { VisitTaskComponent } from './visit-task/visit-task.component';
import { TaskDetailComponent } from './visit-task/task-detail/task-detail.component';
import { ScheduleComponent } from './schedule/schedule.component';
import { ScheduleDetailComponent } from './schedule/schedule-detail/schedule-detail.component';
import { AssignTaskComponent } from './schedule/schedule-detail/assign-task/assign-task.component';
const routes: Routes = [
    {
        path: 'visit-task',
        component: VisitTaskComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'task-detail',
        component: TaskDetailComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'task-detail/:id',
        component: TaskDetailComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'schedule',
        component: ScheduleComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'schedule-detail',
        component: ScheduleDetailComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'schedule-detail/:id',
        component: ScheduleDetailComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'assign-task/:id/:taskId/:visitNum/:scheduleId/:isPush',
        component: AssignTaskComponent,
        canActivate: [AppRouteGuard],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TobaccoManagementRoutingModule { }

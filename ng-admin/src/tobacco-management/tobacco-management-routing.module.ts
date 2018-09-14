import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { VisitTaskComponent } from './visit-task/visit-task.component';
import { TaskDetailComponent } from './visit-task/task-detail/task-detail.component';
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
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class TobaccoManagementRoutingModule { }

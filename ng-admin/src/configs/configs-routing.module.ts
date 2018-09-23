import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { DataConfigComponent } from './data-config/data-config.component';

const routes: Routes = [
    {
        path: 'data-config',
        component: DataConfigComponent,
        canActivate: [AppRouteGuard],
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ConfigsRoutingModule { }


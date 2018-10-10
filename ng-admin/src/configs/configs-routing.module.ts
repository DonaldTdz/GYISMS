import { Routes, RouterModule } from '@angular/router';
import { NgModule } from '@angular/core';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { DataConfigComponent } from './data-config/data-config.component';
import { ACLGuard } from '@delon/acl';

const routes: Routes = [
    {
        path: 'data-config',
        component: DataConfigComponent,
        canActivate: [AppRouteGuard, ACLGuard],
        data: { guard: 'CityAdmin' },
    }
];
@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class ConfigsRoutingModule { }


import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { DefaultLayoutComponent } from 'layout/default-layout.component';
import { OrganizationComponent } from './organization/organization.commponent';
import { EmployeeComponent } from './employee/employee.component';
import { RetailCustomerComponent } from './retail-customer/retail-customer.component';
import { GrowerComponent } from './grower/grower.component';
import { GrowerDetailComponent } from './grower/grower-detail/grower-detail.component';
import { ACLGuard } from '@delon/acl';

const routes: Routes = [
    {
        path: 'organization',
        component: OrganizationComponent,
        canActivate: [AppRouteGuard, ACLGuard],
        data: { guard: 'CityAdmin' },
    },
    {
        path: 'employee',
        component: EmployeeComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'retail-customer',
        component: RetailCustomerComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'grower',
        component: GrowerComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'grower-detail',
        component: GrowerDetailComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'grower-detail/:id',
        component: GrowerDetailComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'grower-detail/:id/:type',
        component: GrowerDetailComponent,
        canActivate: [AppRouteGuard],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BasicRoutingModule { }

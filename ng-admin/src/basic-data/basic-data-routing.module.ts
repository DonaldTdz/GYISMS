import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { DefaultLayoutComponent } from 'layout/default-layout.component';
import { OrganizationComponent } from './organization/organization.commponent';
import { EmployeeComponent } from './employee/employee.component';
import { RetailCustomerComponent } from './retail-customer/retail-customer.component';
const routes: Routes = [
    {
        path: '',
        component: DefaultLayoutComponent,
        canActivate: [AppRouteGuard],
        canActivateChild: [AppRouteGuard],
        children: [
            {
                path: 'organization',
                component: OrganizationComponent,
                canActivate: [AppRouteGuard],
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
        ],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class BasicRoutingModule { }

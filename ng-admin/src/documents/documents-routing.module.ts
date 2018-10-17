import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { ACLGuard } from '@delon/acl';
import { DocumentComponent } from './document/document.component';

const routes: Routes = [
    {
        path: 'document',
        component: DocumentComponent,
        canActivate: [AppRouteGuard, ACLGuard],
        data: { guard: 'CityAdmin' },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DocumentsRoutingModule { }

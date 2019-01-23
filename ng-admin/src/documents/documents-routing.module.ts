import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { ACLGuard } from '@delon/acl';
import { DocumentComponent } from './document/document.component';
import { DocumentDetailComponent } from './document/detail/document-detail.component';

const routes: Routes = [
    {
        path: 'document',
        component: DocumentComponent,
        canActivate: [AppRouteGuard, ACLGuard],
        data: { guard: 'EnterpriseAdmin' },
    },
    {
        path: 'doc-detail',
        component: DocumentDetailComponent,
        canActivate: [AppRouteGuard, ACLGuard],
        data: { guard: 'EnterpriseAdmin' },
    },
    {
        path: 'doc-detail/:id',
        component: DocumentDetailComponent,
        canActivate: [AppRouteGuard, ACLGuard],
        data: { guard: 'EnterpriseAdmin' },
    }
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class DocumentsRoutingModule { }

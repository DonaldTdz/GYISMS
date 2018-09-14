import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbpModule } from 'abp-ng2-module/dist/src/abp.module';
import { LocalizationService } from 'abp-ng2-module/dist/src/localization/localization.service';
import { LayoutModule } from '../layout/layout.module';
import { SharedModule } from '@shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { TobaccoManagementRoutingModule } from './tobacco-management-routing.module';
import { VisitTaskComponent } from './visit-task/visit-task.component';
import { TaskDetailComponent } from './visit-task/task-detail/task-detail.component';


@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        LayoutModule,
        SharedModule,
        AbpModule,
        TobaccoManagementRoutingModule
    ],
    declarations: [
        VisitTaskComponent,
        TaskDetailComponent
    ],
    entryComponents: [
    ],
    providers: [LocalizationService],
})
export class TobaccoManagementModule { }

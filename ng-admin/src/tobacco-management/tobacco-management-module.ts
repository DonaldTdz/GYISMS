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
import { ScheduleComponent } from './schedule/schedule.component';
import { ScheduleDetailComponent } from './schedule/schedule-detail/schedule-detail.component';
import { ChooseTaskModalComponent } from './schedule/schedule-detail/choose-task-modal/choose-task-modal.component';
import { ChooseEmployeeModalComponent } from './schedule/schedule-detail/choose-employee-modal/choose-employee-modal.component';


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
        TaskDetailComponent,
        ScheduleComponent,
        ScheduleDetailComponent,
        ChooseTaskModalComponent,
        ChooseEmployeeModalComponent
    ],
    entryComponents: [
    ],
    providers: [LocalizationService],
})
export class TobaccoManagementModule { }

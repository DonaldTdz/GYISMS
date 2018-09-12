import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbpModule } from 'abp-ng2-module/dist/src/abp.module';
import { LocalizationService } from 'abp-ng2-module/dist/src/localization/localization.service';
import { LayoutModule } from '../layout/layout.module';
import { SharedModule } from '@shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { MeetingManagementRoutingModule } from './meeting-management-routing.module';
import { MeetingRoomComponent } from './meeting-room/meeting-room.component';
import { RoomDetailComponent } from './meeting-room/room-detail/room-detail.component';
import { MessageEmployeeModalComponent } from './meeting-room/room-detail/message-employee-modal/message-employee-modal.component';


@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        MeetingManagementRoutingModule,
        LayoutModule,
        SharedModule,
        AbpModule,
    ],
    declarations: [
        MeetingRoomComponent,
        RoomDetailComponent,
        MessageEmployeeModalComponent
    ],
    entryComponents: [
    ],
    providers: [LocalizationService],
})
export class MeetingManagementModule { }

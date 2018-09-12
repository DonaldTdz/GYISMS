import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AppRouteGuard } from '@shared/auth/auth-route-guard';
import { MeetingRoomComponent } from './meeting-room/meeting-room.component';
import { RoomDetailComponent } from './meeting-room/room-detail/room-detail.component';
const routes: Routes = [
    {
        path: 'meeting-room',
        component: MeetingRoomComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'room-detail',
        component: RoomDetailComponent,
        canActivate: [AppRouteGuard],
    },
    {
        path: 'room-detail/:id',
        component: RoomDetailComponent,
        canActivate: [AppRouteGuard],
    },
];

@NgModule({
    imports: [RouterModule.forChild(routes)],
    exports: [RouterModule],
})
export class MeetingManagementRoutingModule { }

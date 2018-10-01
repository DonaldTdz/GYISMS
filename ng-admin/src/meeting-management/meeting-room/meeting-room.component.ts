import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Router } from '@angular/router';
import { PagedResultDtoOfMeetingRoom, MeetingRoomServiceProxy } from '@shared/service-proxies/meeting-management';
import { MeetingRoom } from '@shared/entity/meeting-management';

@Component({
    moduleId: module.id,
    selector: 'meeting-room',
    templateUrl: 'meeting-room.component.html',
    styleUrls: ['meeting-room.component.scss']
})
export class MeetingRoomComponent extends AppComponentBase implements OnInit {
    search: any = {};
    loading = false;
    roomList: MeetingRoom[] = [];

    constructor(injector: Injector, private meetingService: MeetingRoomServiceProxy, private router: Router) {
        super(injector);
    }

    ngOnInit(): void {
        this.refreshData();
    }

    refreshData(reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.search = {};
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        let params: any = {};
        params.SkipCount = this.query.skipCount();
        params.MaxResultCount = this.query.pageSize;
        params.Name = this.search.name;
        this.meetingService.getAll(params).subscribe((result: PagedResultDtoOfMeetingRoom) => {
            this.loading = false;
            this.roomList = result.items;
            this.query.total = result.totalCount;
        })
    }

    createRoom() {
        this.router.navigate(['app/meeting/room-detail', 0])
    }
    goDetail(id: number) {
        this.router.navigate(['app/meeting/room-detail', id])
    }
}

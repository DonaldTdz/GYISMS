import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Router } from '@angular/router';

@Component({
    moduleId: module.id,
    selector: 'meeting-room',
    templateUrl: 'meeting-room.component.html',
    styleUrls: ['meeting-room.component.scss']
})
export class MeetingRoomComponent extends AppComponentBase implements OnInit {
    constructor(injector: Injector, private router: Router) {
        super(injector);
    }

    ngOnInit(): void {
        // this.refreshData();
    }
}

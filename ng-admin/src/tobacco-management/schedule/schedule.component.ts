import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Schedule } from '@shared/entity/tobacco-management';
import { Router } from '@angular/router';
import { PagedResultDtoOfSchedule, ScheduleServiceProxy } from '@shared/service-proxies/tobacco-management';

@Component({
    moduleId: module.id,
    selector: 'schedule',
    templateUrl: 'schedule.component.html',
    styleUrls: ['schedule.component.scss']
})
export class ScheduleComponent extends AppComponentBase implements OnInit {
    search: any = {};
    loading = false;
    scheduleList: Schedule[] = [];

    constructor(injector: Injector, private scheduleService: ScheduleServiceProxy,
        private router: Router) {
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
        params.Desc = this.search.desc;
        this.scheduleService.getScheduleList(params).subscribe((result: PagedResultDtoOfSchedule) => {
            this.loading = false;
            this.scheduleList = result.items;
            this.query.total = result.totalCount;
        })
    }

    createSchedule() {
        this.router.navigate(['app/task/schedule-detail'])
    }

    goDetail(id: string) {
        this.router.navigate(['app/task/schedule-detail', id])
    }
}

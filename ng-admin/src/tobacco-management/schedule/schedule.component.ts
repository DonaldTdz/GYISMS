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
    scheduleTypes = [
        { text: '每月', value: 1 },
        { text: '每周', value: 2 },
        { text: '每日', value: 3 },
        { text: '自定义', value: 4 }
    ];

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
        params.Name = this.search.name;
        params.scheduleType = this.search.scheduleType;
        this.scheduleService.getScheduleList(params).subscribe((result: PagedResultDtoOfSchedule) => {
            this.loading = false;
            this.scheduleList = result.items;
            this.query.total = result.totalCount;
        })
    }

    createSchedule() {
        this.router.navigate(['app/task/schedule-detail'])
    }

    goDetail(id: string, allPercentage: any) {
        this.router.navigate(['app/task/schedule-detail', id, allPercentage])
    }
}

import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ScheduleSum, ScheduleDetailTask } from '@shared/entity/tobacco-management';
import { ScheduleDetailServiceProxy } from '@shared/service-proxies/tobacco-management/schdule-detail-service';

@Component({
    moduleId: module.id,
    selector: 'report-form',
    templateUrl: 'report-form.component.html',
    styleUrls: ['report-form.component.scss']
})
export class ReportFormComponent extends AppComponentBase implements OnInit {

    sheduleSum: ScheduleSum[] = [];
    totalSum = 0;
    completeSum = 0;
    expiredSum = 0;
    sheduleDetailTask: ScheduleDetailTask[] = [];
    sumSearch: any = { areaCode: null, startTime: null, endTime: null, taskId: null }
    detailSearch: any = { areaCode: null, startTime: null, endTime: null, taskId: null, growerName: '', employeeName: '' }
    constructor(injector: Injector, private sheduleDetailService: ScheduleDetailServiceProxy) {
        super(injector);
    }
    ngOnInit(): void {
    }

    /**
     * 获取任务的汇总信息（按区域、任务类型、任务名）
     */
    getSheduleSum() {
        this.sheduleDetailService.getSheduleSum(this.sumSearch).subscribe(data => {
            this.sheduleSum = data.items.map(i => {
                return i;
            });
            this.totalSum = data.totalSum;
            this.completeSum = data.completeSum;
            this.expiredSum = data.expiredSum;
        });
    }

    /**
     * 获取任务明细信息
     */
    getSheDulDetailByTask() {
        this.detailSearch.skipCount = this.query.skipCount();
        this.detailSearch.maxResultCount = this.query.pageSize;
        this.sheduleDetailService.getSheDulDetailByTask(this.detailSearch).subscribe(data => {
            this.sheduleDetailTask = data.map(i => {
                return i;
            });
        });
    }
}

import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { ScheduleSum, ScheduleDetailTask, VisitTaskName } from '@shared/entity/tobacco-management';
import { ScheduleDetailServiceProxy, VisitTaskServiceProxy } from '@shared/service-proxies/tobacco-management';
import { addDays, addMonths } from 'date-fns';

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
    completeRate = '';
    sheduleDetailTask: ScheduleDetailTask[] = [];
    sumSearch: any = { areaCode: null, startTime: null, endTime: null, taskId: null }
    detailSearch: any = { areaCode: null, startTime: null, endTime: null, taskId: null, growerName: '', employeeName: '' }
    areas = [
        { text: '昭化区', value: 1 },
        { text: '剑阁县', value: 2 },
        { text: '旺苍县', value: 3 }
    ]
    taskTypes = [
        { text: '技术服务', value: 1 },
        { text: '生产管理', value: 2 },
        { text: '政策宣传', value: 3 },
        { text: '临时任务', value: 4 },
    ];
    tasks: VisitTaskName[] = [];
    tasksDe: VisitTaskName[] = [];
    taskType = null;
    loading = false;
    taskTypeDe = null;
    loadingDe = false;
    firstDay: Date;
    lastDay: Date;
    dateRange = [];//[];  
    dateRangeDe = [];//[]; 
    // dateRangeDe = [new Date(), addDays(new Date(), 3)];//[];  
    shedateFormat = 'yyyy-MM-dd';
    shedateFormatDe = 'yyyy-MM-dd';
    constructor(injector: Injector, private sheduleDetailService: ScheduleDetailServiceProxy, private taskService: VisitTaskServiceProxy) {
        super(injector);
    }
    ngOnInit(): void {
        this.getNowDate();
        this.resetTime(1);
        this.getSheduleSum();
        this.resetTime(2);
        this.getSheDulDetailByTask();

    }

    /**
     * 获取任务的汇总信息（按区域、任务类型、任务名）
     */
    getSheduleSum(reset = false) {
        if (reset) {
            this.sumSearch = { areaCode: null, startTime: null, endTime: null, taskId: null }
            this.resetTime(1);
        }
        this.loading = true;
        this.sheduleDetailService.getSheduleSum(this.sumSearch).subscribe(data => {
            this.loading = false;
            this.sheduleSum = data.items.map(i => {
                return i;
            });
            this.totalSum = data.totalSum;
            this.completeSum = data.completeSum;
            this.expiredSum = data.expiredSum;
            if (this.completeSum == 0 || this.totalSum == 0) {
                this.completeRate = '0%';
            } else {
                this.completeRate = Math.round(this.completeSum / this.totalSum * 100).toString() + '%';
            }
        });
    }

    /**
     * 获取任务明细信息
     */
    getSheDulDetailByTask(reset = false, search = false) {
        if (reset) {
            this.query.pageIndex = 1;
            this.detailSearch = { areaCode: null, startTime: null, endTime: null, taskId: null, growerName: '', employeeName: '' }
            this.resetTime(2);
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.detailSearch.skipCount = this.query.skipCount();
        this.detailSearch.maxResultCount = this.query.pageSize;
        this.loadingDe = true;
        this.sheduleDetailService.getSheDulDetailByTask(this.detailSearch).subscribe(data => {
            this.loadingDe = false;
            this.sheduleDetailTask = data.items.map(i => {
                i.completeNum = i.completeNum == null ? 0 : i.completeNum;
                i.expired = i.expired == null ? 0 : i.expired;
                i.expired = i.status == 0 ? i.visitNum - i.completeNum : 0;
                return i;
            });
            this.query.total = data.totalCount;
        });
    }

    taskChange() {
        this.tasks = [];
        this.sumSearch.taskId = null;
        this.taskService.getTaskName({ type: this.taskType }).subscribe(data => {
            this.tasks = data;
        });
    }
    changeTime(times) {
        if (times != null) {
            this.sumSearch.startTime = this.dateFormat(this.dateRange[0]);
            this.sumSearch.endtime = this.dateFormat(this.dateRange[1]);
        }
    }

    changeTimeDe(times) {
        if (times != null) {
            this.detailSearch.startTime = this.dateFormat(this.dateRangeDe[0]);
            this.detailSearch.endtime = this.dateFormat(this.dateRangeDe[1]);
        }
    }

    taskChangeDe() {
        this.tasksDe = [];
        this.detailSearch.taskId = null;
        this.taskService.getTaskName({ type: this.taskTypeDe }).subscribe(data => {
            this.tasksDe = data;
        });
    }
    /**
     * 时间范围重置
     * @param type 1表示汇总时间重置
     */
    resetTime(type) {
        if (type == 1) {
            this.dateRange = [this.firstDay, this.lastDay];//[]; 
            this.sumSearch.startTime = this.dateFormat(this.dateRange[0]);
            this.sumSearch.endtime = this.dateFormat(this.dateRange[1]);
            console.log("this.dateRange");
            console.log(this.dateRange);
        } else {
            this.dateRangeDe = [this.firstDay, this.lastDay];//[]; 
            this.detailSearch.startTime = this.dateFormat(this.dateRangeDe[0]);
            this.detailSearch.endtime = this.dateFormat(this.dateRangeDe[1]);
            console.log("this.dateRangeDe");
        }
        console.log(this.dateRangeDe);
    }

    getNowDate() {
        var nowDate = new Date();
        var year = nowDate.getFullYear();
        var moth = nowDate.getMonth();
        this.firstDay = new Date(year, moth, 1);
        // this.firstDay = new Date(nowDate.setDate(1));
        this.lastDay = addDays(new Date(year, moth + 1, 1), -1)
    }
}

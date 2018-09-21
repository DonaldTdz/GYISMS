import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { VisitTaskServiceProxy, PagedResultDtoOfVisitTask } from '@shared/service-proxies/tobacco-management';
import { VisitTask, ScheduleTask } from '@shared/entity/tobacco-management';
import { NzModalRef } from 'ng-zorro-antd';
@Component({
    moduleId: module.id,
    selector: 'choose-task-modal',
    templateUrl: 'choose-task-modal.component.html',
    styleUrls: ['choose-task-modal.component.scss']
})

export class ChooseTaskModalComponent implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    q: any = {
        pi: 0,
        ps: 5,
        name: ''
    };
    eloading = false;
    isVisible = false;
    taskList: VisitTask[] = [];
    scheduleTaskList: ScheduleTask[] = [];
    scheduleTask: ScheduleTask = new ScheduleTask();
    checkedNumber = 0;
    scheduleId: string;
    isSelectedAll: boolean = false; // 是否全选
    checkboxCount: number = 0; // 所有Checkbox数量
    checkedLength: number = 0; // 已选中的数量
    successMsg = '';
    confirmModal: NzModalRef;
    constructor(private taskService: VisitTaskServiceProxy) {
    }

    ngOnInit(): void {

    }
    isCancelCheck(x: any) {
        this.checkedLength = this.taskList.filter(v => v.checked).length;
        this.checkboxCount = this.taskList.length;
        if (this.checkboxCount - this.checkedLength > 0) {
            this.isSelectedAll = false;
        } else {
            this.isSelectedAll = true;
        }
    }

    checkAll(e) {
        var v = this.isSelectedAll;
        this.taskList.forEach(u => {
            u.checked = v;
        });
        if (this.isSelectedAll == false) {
            this.checkedLength == 0;
        } else {
            this.checkedLength == this.taskList.filter(v => v.checked).length;
        }
    }

    show(id?: string) {
        if (id) {
            this.scheduleId = id;
        }
        this.taskList = new Array<VisitTask>();
        this.isVisible = true;
        this.refreshData();
        this.isSelectedAll = false;
    }

    /**
     * 获取
     */
    refreshData() {
        this.eloading = true;
        let params: any = {};
        params.SkipCount = this.q.pi;
        params.MaxResultCount = this.q.ps;
        params.Name = this.q.name;
        this.taskService.getVisitTaskList(params).subscribe((result: PagedResultDtoOfVisitTask) => {
            this.eloading = false;
            this.taskList = result.items;
            this.taskList.map(v => {
                v.checked = false;
            });
            // console.log(this.taskList);

            this.q.total = result.totalCount;
        });
    }

    /**
     * 取消按钮事件
     */
    handleCancel = (e) => {
        this.isVisible = false;
        this.eloading = false;
    }

    SelectTask(): void {
        var visitTaskList = this.taskList.filter(v => v.checked);
        if (visitTaskList) {
            //visitTaskList.forEach(v => {
            //this.scheduleTask.taskId = v.id;
            //this.scheduleTask.visitNum = v.visitNum;
            //this.scheduleTask.taskName = v.name;
            //this.scheduleTask.scheduleId = this.scheduleId;
            //this.scheduleTaskList.push(ScheduleTask.fromJS(v));
            //});
            this.scheduleTaskList = ScheduleTask.fromVisitTaskJSArray(visitTaskList, this.scheduleId);
            this.eloading = true;
            this.successMsg = '保存成功';
            this.saveTaskInfo();
        }
        this.modalSelect.emit(visitTaskList);
        this.isVisible = false;
    }

    saveTaskInfo() {
        this.taskService.updateScheduleTask(this.scheduleTaskList).finally(() => { this.eloading = false; })
            .subscribe((result: any) => {
                this.scheduleTaskList = result;
            });
    }
}

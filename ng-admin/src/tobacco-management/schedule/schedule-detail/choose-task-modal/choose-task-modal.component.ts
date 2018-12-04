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
        // params.SkipCount = this.q.pi;
        params.ScheduleId = this.scheduleId;
        let i: number = 0;
        this.taskService.getVisitTaskListWithStatus(params).subscribe((result: VisitTask[]) => {
            this.eloading = false;
            this.taskList = result;
            this.taskList.map(v => {
                if (!v.visitNum) {
                    v.visitNum = 1;
                }
                if (v.checked == true) {
                    i++;
                }
            });
            if ((this.taskList.length != 0) && (this.taskList.length == i)) {
                this.isSelectedAll = true;
            }
            console.log(this.taskList);
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
            visitTaskList.forEach(v => {
                if (v.scheduleTaskId) {
                    this.scheduleTask.id = v.scheduleTaskId;
                }
                this.scheduleTask.taskId = v.id;
                this.scheduleTask.visitNum = v.visitNum;
                this.scheduleTask.taskName = v.name;
                this.scheduleTask.scheduleId = this.scheduleId;
                this.scheduleTaskList.push(ScheduleTask.fromJS(this.scheduleTask));
                this.scheduleTask = new ScheduleTask();
            });

            // this.scheduleTaskList = ScheduleTask.fromVisitTaskJSArray(visitTaskList, this.scheduleId);
            this.eloading = true;
            this.successMsg = '保存成功';
            this.saveTaskInfo();
        }
        // this.modalSelect.emit(visitTaskList);
    }

    saveTaskInfo() {
        this.taskService.updateScheduleTask(this.scheduleTaskList).finally(() => { this.eloading = false; })
            .subscribe((result: any) => {
                // this.scheduleTaskList = result;
                this.scheduleTaskList = [];
                result.forEach(x => {
                    this.taskList.forEach(v => {
                        if (v.scheduleTaskId == x.id) {
                            v.scheduleTaskId = x.id;
                            return;
                        }
                    });
                });

                this.modalSelect.emit();
                this.isVisible = false;
            });
    }
}

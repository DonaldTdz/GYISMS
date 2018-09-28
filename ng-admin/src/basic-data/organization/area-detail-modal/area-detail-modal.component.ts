import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { VisitTaskServiceProxy } from '@shared/service-proxies/tobacco-management';
import { NzModalRef } from 'ng-zorro-antd';
import { Employee } from '@shared/entity/basic-data';
import { FormGroup, Validators, FormBuilder } from '@angular/forms';
@Component({
    moduleId: module.id,
    selector: 'area-detail-modal',
    templateUrl: 'area-detail-modal.component.html',
    styleUrls: ['area-detail-modal.component.scss']
})

export class AreaDetailModalComponent implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    eloading = false;
    isVisible = false;
    employee: Employee = new Employee();
    employeeId: string;
    successMsg = '';
    confirmModal: NzModalRef;
    validateForm: FormGroup;


    constructor(private taskService: VisitTaskServiceProxy, private fb: FormBuilder) {
    }

    ngOnInit(): void {
        this.validateForm = this.fb.group({
            teName: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            teDesc: [null, Validators.compose([Validators.maxLength(500)])],
        });
    }

    show(id?: string) {
        if (id) {
            this.employeeId = id;
        }
        this.isVisible = true;
    }

    /**
     * 获取
     */
    // getTaskInfo() {
    //     //新增
    //     this.employee.name = null;
    //     this.employee.desc = null;
    //     this.employee.seq = null;
    // }

    // /**
    //  * 取消按钮事件
    //  */
    // handleCancel = (e) => {
    //     this.isVisible = false;
    //     this.eloading = false;
    // }

    // SelectTask(): void {
    //     if (employee) {
    //             this.scheduleTask.taskId = v.id;
    //             this.scheduleTask.visitNum = v.visitNum;
    //             this.scheduleTask.taskName = v.name;
    //             this.scheduleTask.scheduleId = this.scheduleId;
    //             this.scheduleTaskList.push(ScheduleTask.fromJS(this.scheduleTask));
    //             this.scheduleTask = new Employee();
    //         });

    //         // this.scheduleTaskList = ScheduleTask.fromVisitTaskJSArray(visitTaskList, this.scheduleId);
    //         this.eloading = true;
    //         this.successMsg = '保存成功';
    //         this.saveTaskInfo();
    //     }
    //     // this.modalSelect.emit(visitTaskList);
    // }

    // saveTaskInfo() {
    //     this.taskService.updateScheduleTask(this.scheduleTaskList).finally(() => { this.eloading = false; })
    //         .subscribe((result: any) => {
    //             // this.scheduleTaskList = result;
    //             this.scheduleTaskList = [];
    //             result.forEach(x => {
    //                 this.taskList.forEach(v => {
    //                     if (v.scheduleTaskId == x.id) {
    //                         v.scheduleTaskId = x.id;
    //                         return;
    //                     }
    //                 });
    //             });

    //             this.modalSelect.emit();
    //             this.isVisible = false;
    //         });
    // }
}

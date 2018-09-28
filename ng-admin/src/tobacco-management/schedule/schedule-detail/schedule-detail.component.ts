import { Component, Injector, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Schedule, VisitTask, ScheduleTask } from '@shared/entity/tobacco-management';
import { ScheduleServiceProxy, VisitTaskServiceProxy } from '@shared/service-proxies/tobacco-management';
import { NzModalService, NzModalRef } from 'ng-zorro-antd';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ChooseTaskModalComponent } from './choose-task-modal/choose-task-modal.component';
import { ChooseEmployeeModalComponent } from './choose-employee-modal/choose-employee-modal.component';

@Component({
    moduleId: module.id,
    selector: 'schedule-detail',
    templateUrl: 'schedule-detail.component.html',
    styleUrls: ['schedule-detail.component.scss']
})
export class ScheduleDetailComponent extends AppComponentBase implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('selectTaskModal') selectTaskModal: ChooseTaskModalComponent;
    @ViewChild('selectEmployeeModal') selectEmployeeModal: ChooseEmployeeModalComponent;

    id: string = '';
    validateForm: FormGroup;
    schedule: Schedule = new Schedule();
    scheduleTask: ScheduleTask = new ScheduleTask();
    scheduleTaskList: ScheduleTask[] = [];
    types: any[] = [{ text: '每月', value: 1 }, { text: '每周', value: 2 }, { text: '每日', value: 3 }];
    isConfirmLoading = false;
    successMsg = '';
    confirmModal: NzModalRef;
    isDelete = false;
    isPush = true;
    isSaved = false;
    taskList: VisitTask[] = [];
    loading = false;
    isNewInfo: boolean;

    constructor(injector: Injector, private scheduleService: ScheduleServiceProxy,
        private taskService: VisitTaskServiceProxy,
        private router: Router, private fb: FormBuilder, private actRouter: ActivatedRoute, private modal: NzModalService) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
    }
    ngOnInit(): void {
        this.validateForm = this.fb.group({
            name: [null, Validators.compose([Validators.required, Validators.maxLength(200)])],
            desc: [null, Validators.compose([Validators.maxLength(500)])],
            type: null,
            beginTime: null,
            endTime: null,
            // taskName: [null, Validators.compose([Validators.required, Validators.maxLength(200)])],
            // visitNum: [null, Validators.compose([Validators.pattern("^\\d+$")])]
        });
        this.getScheduleInfo();
        this.isNewInfo = typeof (this.id) == 'undefined';
    }

    getTaskList() {
        // let params: any = {};
        // params.SkipCount = this.query.skipCount();
        // params.MaxResultCount = this.query.pageSize;
        this.taskService.getScheduleTaskListNoPage(this.schedule.id).subscribe((result: ScheduleTask[]) => {
            this.loading = false;
            this.scheduleTaskList = result;
        });
    }

    getScheduleInfo() {
        if (this.id) {
            let params: any = {};
            params.id = this.id;
            this.scheduleService.getScheduleById(params).subscribe((result: Schedule) => {
                this.schedule = result;
                this.isPush = result.status == 1 ? false : true;
                this.isDelete = true;
                // if (!this.isPush) {
                this.getTaskList();
                this.isSaved = true;
                // }
            });
        } else {
            //新增
            this.schedule.type = 1;
        }
    }

    save(isPulish = false) {
        for (const i in this.validateForm.controls) {
            this.validateForm.controls[i].markAsDirty();
        }
        if (this.validateForm.valid) {
            this.isConfirmLoading = true;
            if (this.schedule.type == 3) {
                if (this.schedule.beginTime)
                    this.schedule.beginTime = this.dateFormat(this.schedule.beginTime);
                else
                    this.schedule.beginTime = null;
                this.schedule.endTime = this.schedule.beginTime;
            } else {// type =1
                if (this.schedule.beginTime)
                    this.schedule.beginTime = this.dateFormat(this.schedule.beginTime);
                else
                    this.schedule.beginTime = null;
                if (this.schedule.endTime)
                    this.schedule.endTime = this.dateFormat(this.schedule.endTime);
                else
                    this.schedule.endTime = null;
            }
            this.successMsg = isPulish == false ? '保存成功！' : '发布成功！';
            if (!this.schedule.beginTime) {
                this.schedule.beginTime = null;
            }
            if (!this.schedule.endTime) {
                this.schedule.endTime = null;
            }
            this.saveScheduleInfo(isPulish);
        }
    }

    saveScheduleInfo(isPulish: boolean) {
        if (isPulish == false) {
            this.schedule.status = 0;
        }
        this.scheduleService.updateScheduleInfo(this.schedule).finally(() => { this.isConfirmLoading = false; })
            .subscribe((result: any) => {
                this.schedule = result;
                this.id = result.id;
                // if (this.schedule.id) {
                //     this.scheduleTask.scheduleId = this.schedule.id;
                //     this.scheduleService.updateScheduleTaskInfo(this.scheduleTask).subscribe((res: any) => {
                //         this.scheduleTask = res;
                //     })
                // }
                this.isDelete = true;
                this.isPush = result.status == 1 ? false : true;
                this.notify.info(this.l(this.successMsg));
                this.isSaved = true;
                this.getTaskList();
            });
    }
    push() {
        this.confirmModal = this.modal.confirm({
            nzContent: '发布后不可修改，是否确认发布?',
            nzOnOk: () => {
                this.schedule.status = 1;
                this.save(true);
            }
        });
    }

    delete(): void {
        this.confirmModal = this.modal.confirm({
            nzContent: '是否移除任务信息?',
            nzOnOk: () => {
                this.scheduleService.deleteSchedule(this.schedule).subscribe(() => {
                    this.notify.info(this.l('删除成功！'));
                    this.return();
                });
            }
        });
    }

    /**
     * 模态框返回
     */
    // getSelectData = (visitTask?: VisitTask[]) => {
    getSelectData = () => {
        // visitTask.forEach(v => {
        //     if (!this.existsTask(v.id)) {
        //         this.taskList.push(...visitTask);
        //     }
        // });
        this.getTaskList();
    }

    // existsTask(id: number): boolean {
    //     let bo = false;
    //     this.taskList.forEach(v => {
    //         if (v.id == id) {
    //             bo = true;
    //             return;
    //         }
    //     });
    //     return bo;
    // }

    showModal(): void {
        this.selectTaskModal.show(this.schedule.id);
    }

    showEmployeeModal(): void {
        this.selectEmployeeModal.show();
    }

    deleteTask(data: ScheduleTask) {
        // let i = 0;
        // this.taskList.forEach(v => {
        //     if (v.id == id) {
        //         this.taskList.splice(i, 1);
        //         return;
        //     }
        //     i++;
        // });
        this.confirmModal = this.modal.confirm({
            nzContent: '是否删除指派任务?',
            nzOnOk: () => {
                this.taskService.deleteScheduleTask(data).subscribe(() => {
                    this.notify.info(this.l('删除成功！'));
                    this.getTaskList();
                });
            }
        });
    }

    return() {
        this.router.navigate(['app/task/schedule']);
    }

    assignTask(id: number, taskId: string, visitNum: number) {
        let scheduleId: string = this.id;            //计划Id,状态
        this.router.navigate(['app/task/assign-task', id, taskId, visitNum, scheduleId, this.isPush]);
    }
    assignAll(id: number, taskId: string, visitNum: number): void {
        this.confirmModal = this.modal.confirm({
            nzContent: '即将指派所有烟农，是否继续?',
            nzOnOk: () => {
                let input: any = {};
                input.VisitNum = visitNum;
                input.ScheduleTaskId = id;
                input.ScheduleId = this.id;
                input.TaskId = taskId;
                this.taskService.createAllScheduleTask(input).subscribe(() => {
                    this.notify.info(this.l('任务指派成功！'));
                });
            }
        });
    }
}

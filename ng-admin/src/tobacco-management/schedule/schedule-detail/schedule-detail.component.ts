import { Component, Injector, OnInit, Output, ViewChild, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Schedule, VisitTask, ScheduleTask } from '@shared/entity/tobacco-management';
import { ScheduleServiceProxy } from '@shared/service-proxies/tobacco-management';
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

    id: number;
    validateForm: FormGroup;
    schedule: Schedule = new Schedule();
    scheduleTask: ScheduleTask = new ScheduleTask();
    types: any[] = [{ text: '每日', value: 1 }, { text: '每周', value: 2 }, { text: '每月', value: 3 }];
    isConfirmLoading = false;
    successMsg = '';
    confirmModal: NzModalRef;
    isDelete = false;
    isPush = true;

    constructor(injector: Injector, private scheduleService: ScheduleServiceProxy,
        private router: Router, private fb: FormBuilder, private actRouter: ActivatedRoute, private modal: NzModalService) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
    }
    ngOnInit(): void {
        this.validateForm = this.fb.group({
            desc: [null, Validators.compose([Validators.maxLength(500)])],
            type: null,
            beginTime: null,
            endTime: null,
            taskName: [null, Validators.compose([Validators.required, Validators.maxLength(200)])],
            visitNum: [null, Validators.compose([Validators.pattern("^\\d+$")])]
        });
        this.getScheduleInfo();
    }

    getScheduleInfo() {
        if (this.id) {
            let params: any = {};
            params.id = this.id;
            this.scheduleService.getScheduleById(params).subscribe((result: Schedule) => {
                this.schedule = result;
                this.isPush = result.status == 1 ? false : true;
                this.isDelete = true;
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
            this.schedule.beginTime = this.dateFormat(this.schedule.beginTime);
            this.schedule.endTime = this.dateFormat(this.schedule.endTime);
            this.successMsg = isPulish == false ? '保存成功！' : '发布成功！';
            if (!this.schedule.beginTime) {
                this.schedule.beginTime = null;
            }
            if (!this.schedule.endTime) {
                this.schedule.endTime = null;
            }
            this.saveScheduleInfo();
        }
    }

    saveScheduleInfo() {
        this.scheduleService.updateScheduleInfo(this.schedule).finally(() => { this.isConfirmLoading = false; })
            .subscribe((result: any) => {
                this.schedule = result;
                if (this.schedule.id) {
                    this.scheduleTask.scheduleId = this.schedule.id;
                    this.scheduleService.updateScheduleTaskInfo(this.scheduleTask).subscribe((res: any) => {
                        this.scheduleTask = res;
                    })
                }
                this.isDelete = true;
                this.isPush = result.status == 1 ? false : true;
                this.notify.info(this.l(this.successMsg));
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
            nzContent: '是否删除任务信息?',
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
    getSelectData = (visitTask?: VisitTask) => {
        if (visitTask) {
            this.scheduleTask.taskId = visitTask.id;
            this.scheduleTask.taskName = visitTask.name;
        }
    }

    showModal(): void {
        this.selectTaskModal.show();
    }

    showEmployeeModal(): void {
        this.selectEmployeeModal.show();
    }

    return() {
        this.router.navigate(['app/task/schedule']);
    }
}

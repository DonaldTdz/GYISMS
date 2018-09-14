import { Component, Injector, OnInit } from '@angular/core';
import { NzModalService, NzModalRef } from 'ng-zorro-antd';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VisitTaskServiceProxy } from '@shared/service-proxies/tobacco-management';
import { AppComponentBase } from '@shared/app-component-base';
import { VisitTask } from '@shared/entity/tobacco-management';

@Component({
    moduleId: module.id,
    selector: 'task-detail',
    templateUrl: 'task-detail.component.html',
    styleUrls: ['task-detail.component.scss']
})
export class TaskDetailComponent extends AppComponentBase implements OnInit {
    id: number;
    validateForm: FormGroup;
    task: VisitTask = new VisitTask();
    types: any[] = [{ text: '技术服务', value: 1 }, { text: '生产管理', value: 2 }, { text: '政策宣传', value: 3 }, { text: '临时任务', value: 4 }];
    isExamines: any[] = [{ text: '是', value: true }, { text: '否', value: false }];
    isConfirmLoading = false;
    successMsg = '';
    confirmModal: NzModalRef;
    isDelete = false;

    constructor(injector: Injector, private fb: FormBuilder
        , private taskService: VisitTaskServiceProxy
        , private actRouter: ActivatedRoute, private router: Router
        , private modal: NzModalService) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
    }

    ngOnInit(): void {
        this.validateForm = this.fb.group({
            name: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            type: [null, Validators.compose([Validators.required])],
            desc: [null, Validators.compose([Validators.maxLength(500)])],
            isExamine: null,
        });
        this.getTaskInfo();
    }

    getTaskInfo() {
        if (this.id) {
            let params: any = {};
            params.id = this.id;
            this.taskService.getVisitTaskById(params).subscribe((result: VisitTask) => {
                this.task = result;
                this.isDelete = true;
            });
        } else {
            //新增
            this.task.isExamine = true;
            this.task.type = 1;
        }
    }

    save() {
        for (const i in this.validateForm.controls) {
            this.validateForm.controls[i].markAsDirty();
        }
        if (this.validateForm.valid) {
            this.isConfirmLoading = true;
            this.successMsg = '保存成功';
            this.saveTaskInfo();
        }
    }

    saveTaskInfo() {
        this.taskService.updateTaskInfo(this.task).finally(() => { this.isConfirmLoading = false; })
            .subscribe((result: VisitTask) => {
                this.task = result;
                this.isDelete = true;
                this.notify.info(this.l(this.successMsg));
            });
    }

    delete(): void {
        this.confirmModal = this.modal.confirm({
            nzContent: '是否删除任务信息?',
            nzOnOk: () => {
                this.taskService.deleteVisitTask(this.task).subscribe(() => {
                    this.notify.info(this.l('删除成功！'));
                    this.return();
                });
            }
        });
    }

    return() {
        this.router.navigate(['app/task/visit-task']);
    }
}

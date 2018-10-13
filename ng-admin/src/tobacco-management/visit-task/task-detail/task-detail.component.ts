import { Component, Injector, OnInit, Output, EventEmitter, ViewChild } from '@angular/core';
import { NzModalService, NzModalRef } from 'ng-zorro-antd';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { VisitTaskServiceProxy, PagedResultDtoOfTaskExamine } from '@shared/service-proxies/tobacco-management';
import { AppComponentBase } from '@shared/app-component-base';
import { VisitTask, TaskExamine } from '@shared/entity/tobacco-management';
import { ExamineDetailModalComponent } from './examine-detail-modal/examine-detail-modal.component';

@Component({
    moduleId: module.id,
    selector: 'task-detail',
    templateUrl: 'task-detail.component.html',
    styleUrls: ['task-detail.component.scss']
})
export class TaskDetailComponent extends AppComponentBase implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('selectExamineModal') selectExamineModal: ExamineDetailModalComponent;

    id: number;
    validateForm: FormGroup;
    task: VisitTask = new VisitTask();
    taskExamine: TaskExamine = new TaskExamine();
    taskExamineList: TaskExamine[] = [];
    types: any[] = [{ text: '技术服务', value: 1 }, { text: '生产管理', value: 2 }, { text: '政策宣传', value: 3 }, { text: '临时任务', value: 4 }];
    isExamines: any[] = [{ text: '是', value: true }, { text: '否', value: false }];
    isConfirmLoading = false;
    successMsg = '';
    confirmModal: NzModalRef;
    isDelete = false;

    search: any = {};
    loading = false;

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
            desc: [null, Validators.compose([Validators.maxLength(500)])],
            type: null,
            isExamine: null,
            // teName: [null, Validators.compose([Validators.maxLength(50)])],
            // teName: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            // teDesc: [null, Validators.compose([Validators.maxLength(500)])],
            // teSeq: [null, Validators.compose([Validators.pattern(/^\+?[1-9][0-9]*$/)])]
        });
        // this.validateForm2 = this.fb.group({});
        this.getTaskInfo();
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
        this.taskService.getTaskExamineList(params).subscribe((result: PagedResultDtoOfTaskExamine) => {
            this.loading = false;
            this.taskExamineList = result.items;
            this.query.total = result.totalCount;
        })
    }
    getTaskInfo() {
        if (this.id) {
            let params: any = {};
            params.id = this.id;
            this.taskService.getVisitTaskById(params).subscribe((result: VisitTask) => {
                this.task = result;
                if (this.task.isExamine == true) {
                    this.loading = true;
                    this.getTaskExamineList();
                }
                this.isDelete = true;
            });
        } else {
            //新增
            this.task.isExamine = false;
            this.task.type = 1;
        }
    }
    getTaskExamineList() {
        let params: any = {};
        params.SkipCount = this.query.skipCount();
        params.MaxResultCount = this.query.pageSize;
        params.Name = this.search.name;
        params.TaskId = this.task.id;
        this.taskService.getTaskExamineList(params).subscribe((result: PagedResultDtoOfTaskExamine) => {
            this.loading = false;
            this.taskExamineList = result.items;
            this.query.total = result.totalCount;
        })
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
        this.task.taskExamineList = this.taskExamineList;
        this.taskService.updateTaskInfo(this.task).finally(() => { this.isConfirmLoading = false; })
            .subscribe((result: any) => {
                this.task = result;
                // if (this.task.isExamine == true) {
                //     this.taskExamine.taskId = this.task.id;
                //     this.taskService.updateTaskExamineInfo(this.taskExamine).subscribe((res: TaskExamine) => {
                //         this.taskExamine = res;
                //         this.isDelete = true;
                //         this.notify.info(this.l(this.successMsg), '');
                //     });
                // } else {
                this.isDelete = true;
                this.taskExamineList = this.task.taskExamineList;
                this.notify.info(this.l(this.successMsg), '');
                // }
            });
    }

    delete(): void {
        this.confirmModal = this.modal.confirm({
            nzContent: '是否删除任务信息?',
            nzOnOk: () => {
                this.taskService.deleteVisitTask(this.task).subscribe(() => {
                    this.notify.info(this.l('删除成功！'), '');
                    this.return();
                });
            }
        });
    }

    return() {
        this.router.navigate(['app/task/visit-task']);
    }

    goDetail(id: number) {
        this.router.navigate(['app/task/schedule-detail', id])
    }

    /**
 * 模态框返回
 */
    getSelectData = (examine?: TaskExamine) => {
        if (examine) {
            this.taskExamineList.push(examine);
        }
    }

    showModal(): void {
        this.selectExamineModal.show();
    }

    deleteExamine(examine: TaskExamine) {
        if (examine.id) {
            this.confirmModal = this.modal.confirm({
                nzContent: '是否删除考核项目?',
                nzOnOk: () => {
                    this.taskService.deleteTaskExamine(examine).subscribe(() => {
                        this.notify.info(this.l('删除成功！'), '');
                        this.getTaskExamineList();
                    });
                }
            });
        } else {
            let i = 0;
            this.taskExamineList.forEach(v => {
                if (v.name == examine.name && v.desc == examine.desc && v.seq == examine.seq) {
                    this.taskExamineList.splice(i, 1);
                    return;
                }
                i++;
            });
        }
    }
}

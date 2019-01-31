import { Component, Output, EventEmitter, OnInit } from '@angular/core';
import { TaskExamine } from '@shared/entity/tobacco-management';
import { ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';

@Component({
    moduleId: module.id,
    selector: 'examine-detail-modal',
    templateUrl: 'examine-detail-modal.component.html',
    styleUrls: ['examine-detail-modal.component.scss']
})
export class ExamineDetailModalComponent implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    id: number;
    eloading = false;
    isVisible = false;
    taskExamine: TaskExamine = new TaskExamine();
    validateForm: FormGroup;
    successMsg = '';
    notify: NotifyService;
    examineOptions = [{ id: 1, text: '优/合格/差' }, { id: 2, text: '到位/不到位' }, { id: 3, text: '了解/不了解' }]

    constructor(private actRouter: ActivatedRoute, private fb: FormBuilder
    ) {
        this.id = this.actRouter.snapshot.params['id'];
    }

    ngOnInit(): void {
        this.validateForm = this.fb.group({
            teName: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            teDesc: [null, Validators.compose([Validators.maxLength(500)])],
            teExamineOption: [1, Validators.compose([Validators.required])],
            teSeq: [null, Validators.compose([Validators.pattern(/^\+?[1-9][0-9]*$/)])]
        });
    }

    show(examine?: TaskExamine) {
        this.isVisible = true;
        this.getTaskInfo();
    }

    getTaskInfo() {
        //新增
        this.taskExamine.name = null;
        this.taskExamine.desc = null;
        this.taskExamine.seq = null;
        this.taskExamine.examineOption = 1;
    }

    /**
     * 取消按钮事件
     */
    handleCancel = (e) => {
        this.isVisible = false;
        this.eloading = false;
    }

    /**
     * 
     * @param organization 选择事件（对选择的数据进行回传）
     */
    SelectTaskExamine(examine: TaskExamine): void {
        for (const i in this.validateForm.controls) {
            this.validateForm.controls[i].markAsDirty();
        }
        //console.log(JSON.stringify(examine));
        if (examine.examineOption == 1) {
            examine.examineOptionDesc = '优/合格/差';
        } else if (examine.examineOption == 2) {
            examine.examineOptionDesc = '到位/不到位';
        } else {
            examine.examineOptionDesc = '了解/不了解';
        }
        if (this.validateForm.valid) {
            this.modalSelect.emit(examine.clone());
            this.isVisible = false;
        }
    }
}

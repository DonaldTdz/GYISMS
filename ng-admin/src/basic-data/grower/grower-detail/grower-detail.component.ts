import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzModalService, NzModalRef } from 'ng-zorro-antd';
import { ActivatedRoute, Router } from '@angular/router';
import { GrowerServiceProxy } from '@shared/service-proxies/basic-data';
import { Grower, Organization } from '@shared/entity/basic-data';
import { MessageOrganizationModalComponent } from './message-organization-modal/message-organization-modal.component';

@Component({
    moduleId: module.id,
    selector: 'grower-detail',
    templateUrl: 'grower-detail.component.html',
    styleUrls: ['grower-detail.component.scss']
})
export class GrowerDetailComponent extends AppComponentBase implements OnInit {
    @ViewChild('selectsOrganizationModal') selectsEmployeeModal: MessageOrganizationModalComponent;

    id: number;
    validateForm: FormGroup;
    grower: Grower = new Grower();
    countyCodes: any[] = [{ text: '剑阁县', value: 1 }, { text: '昭化区', value: 2 }, { text: '旺苍县', value: 3 }];
    isConfirmLoading = false;
    successMsg = '';
    confirmModal: NzModalRef;
    isDelete = false;

    constructor(injector: Injector, private fb: FormBuilder
        , private growerService: GrowerServiceProxy
        , private actRouter: ActivatedRoute, private router: Router
        , private modal: NzModalService) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
    }

    ngOnInit(): void {
        this.validateForm = this.fb.group({
            name: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            unitName: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            year: [null, Validators.compose([Validators.pattern(/^\+?[1-9][0-9]*$/)])],
            contractNo: [null, Validators.compose([Validators.maxLength(50)])],
            villageGroup: [null, Validators.compose([Validators.maxLength(50)])],
            address: [null, Validators.compose([Validators.maxLength(500)])],
            tel: [null, Validators.compose([Validators.maxLength(11)])],
            // latitude: [null, Validators.compose([Validators.pattern(/^[\-\+]?([0-8]?\d{1}\.\d{1,5}|90\.0{1,5})$/)])],
            // longitude: [null, Validators.compose([Validators.pattern(/^[\-\+]?(0?\d{1,2}\.\d{1,5}|1[0-7]?\d{1}\.\d{1,5}|180\.0{1,5})$/)])],
            longitude: null,
            latitude: null,
            plantingArea: [null, Validators.compose([Validators.pattern(/(?!0\.00)(\d+\.\d{2}$)/)])],
            countyCode: null,
            contractTime: null
        });
        this.getGrowerInfo();
    }

    getGrowerInfo() {
        if (this.id) {
            let params: any = {};
            params.id = this.id;
            this.growerService.getGrowerById(params).subscribe((result: Grower) => {
                this.grower = result;
                this.isDelete = true;
            });
        } else {
            //新增
        }
    }

    save() {
        for (const i in this.validateForm.controls) {
            this.validateForm.controls[i].markAsDirty();
        }
        if (this.validateForm.valid) {
            this.isConfirmLoading = true;

            this.grower.contractTime = this.dateFormat(this.grower.contractTime);
            this.successMsg = '保存成功';
            this.saveRoomInfo();
        }
    }

    saveRoomInfo() {
        this.growerService.updateGrowerInfo(this.grower).finally(() => { this.isConfirmLoading = false; })
            .subscribe((result: Grower) => {
                this.grower = result;
                this.isDelete = true;
                this.notify.info(this.l(this.successMsg));
            });
    }

    delete(): void {
        this.confirmModal = this.modal.confirm({
            nzContent: '是否删除烟农信息?',
            nzOnOk: () => {
                this.growerService.deleteGrower(this.grower).subscribe(() => {
                    this.notify.info(this.l('删除成功！'));
                    this.return();
                });
            }
        });
    }

    /**
     * 模态框返回
     */
    getSelectData = (organization?: Organization) => {
        if (organization) {
            this.grower.unitCode = organization.id.toString();
            this.grower.unitName = organization.departmentName;
            // this.grower.id = grower.id;
        }
    }
    showModal(): void {
        this.selectsEmployeeModal.show();
    }
    return() {
        this.router.navigate(['app/basic/grower']);
    }
}

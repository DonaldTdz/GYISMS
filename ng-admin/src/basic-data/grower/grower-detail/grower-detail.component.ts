import { Component, Injector, OnInit, ViewChild, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { NzModalService, NzModalRef } from 'ng-zorro-antd';
import { ActivatedRoute, Router } from '@angular/router';
import { GrowerServiceProxy } from '@shared/service-proxies/basic-data';
import { Grower, Employee, SelectGroup } from '@shared/entity/basic-data';
import { GrowerEmployeeModalComponent } from './grower-employee-modal/grower-employee-modal.component';
import * as moment from 'moment';
import { VisitTaskServiceProxy, PagedResultDtoOfVisitRecord } from '@shared/service-proxies/tobacco-management';
import { VisitRecord } from '@shared/entity/tobacco-management';

@Component({
    moduleId: module.id,
    selector: 'grower-detail',
    templateUrl: 'grower-detail.component.html',
    styleUrls: ['grower-detail.component.scss']
})
export class GrowerDetailComponent extends AppComponentBase implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('selectsEmployeeModal') selectsEmployeeModal: GrowerEmployeeModalComponent;

    id: number;
    validateForm: FormGroup;
    grower: Grower = new Grower();
    visitRecordList: VisitRecord[] = [];
    // countyCodes: RadioGroup[] = [
    //     // { text: '昭化区', value: 1 }, { text: '剑阁县', value: 2 }, { text: '旺苍县', value: 3 }
    // ];
    countyCodes: any = [
        { text: '昭化区', value: 1 }, { text: '剑阁县', value: 2 }, { text: '旺苍县', value: 3 }
    ];
    types: any[] = [{ text: '普通烟农', value: 1 }];
    unitTypes: SelectGroup[] = [];
    isConfirmLoading = false;
    successMsg = '';
    confirmModal: NzModalRef;
    isDelete = false;
    loading = false;
    previewImage = ''
    defalutImg = '/visit/defaultRecord.png';
    previewVisible = false;

    constructor(injector: Injector, private fb: FormBuilder
        , private growerService: GrowerServiceProxy
        , private actRouter: ActivatedRoute, private router: Router
        , private modal: NzModalService
        , private taskService: VisitTaskServiceProxy) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
    }

    ngOnInit(): void {
        this.validateForm = this.fb.group({
            name: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            unitCode: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            employeeName: [null, Validators.compose([Validators.maxLength(200)])],
            year: [null, Validators.compose([Validators.pattern(/^\+?[1-9][0-9]*$/)])],
            contractNo: [null, Validators.compose([Validators.maxLength(50)])],
            villageGroup: [null, Validators.compose([Validators.maxLength(50)])],
            address: [null, Validators.compose([Validators.maxLength(500)])],
            tel: [null, Validators.compose([Validators.maxLength(11), Validators.pattern(/^[0-9]*$/)])],
            // latitude: [null, Validators.compose([Validators.pattern(/^[\-\+]?([0-8]?\d{1}\.\d{1,5}|90\.0{1,5})$/)])],
            // longitude: [null, Validators.compose([Validators.pattern(/^[\-\+]?(0?\d{1,2}\.\d{1,5}|1[0-7]?\d{1}\.\d{1,5}|180\.0{1,5})$/)])],
            longitude: null,
            latitude: null,
            plantingArea: [null, Validators.compose([Validators.pattern(/^(?:[1-9]\d*|0)(?:\.\d{1,2})?$/)])],
            areaCode: null,
            contractTime: null,
            type: null
        });
        this.getUnitType();
        // this.getCountyCode();
    }

    getGrowerInfo() {
        if (this.id) {
            let params: any = {};
            params.id = this.id;
            this.growerService.getGrowerById(params).subscribe((result: Grower) => {
                this.grower = result;
                this.grower.plantingArea = Number(this.grower.plantingArea).toFixed(2);
                this.isDelete = true;
            });
            this.getVisitRecord();
        } else {
            //新增
            this.grower.areaCode = 1;
            this.grower.type = 1;
            this.grower.plantingArea = Number(0).toFixed(2);
            var currentdate = new Date();
            var y = currentdate.getFullYear();
            var m = currentdate.getMonth() + 1;
            var d = currentdate.getDate();
            var endTime = y + '-' + (m > 9 ? m : '0' + m) + '-' + (d > 9 ? d : '0' + d);
            this.grower.contractTime = moment(endTime).toString();
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
            if (!this.grower.contractTime) {
                this.grower.contractTime = null;
            }
            this.saveRoomInfo();
        }
    }
    getUnitType() {
        this.growerService.getUnitTypeAsync().subscribe((result: SelectGroup[]) => {
            this.unitTypes = result;
            this.getGrowerInfo();
        });
    }

    // getCountyCode() {
    //     this.growerService.getCountyCodeAsync().subscribe((result: RadioGroup[]) => {
    //         this.countyCodes = result;
    //     });
    // }

    getVisitRecord() {
        this.loading = true;
        let params: any = {};
        params.SkipCount = this.query.skipCount();
        params.MaxResultCount = this.query.pageSize;
        params.GrowerId = this.id;
        this.taskService.getVisitVisitRecordListByGrowerId(params).subscribe((result: PagedResultDtoOfVisitRecord) => {
            this.loading = false;
            this.visitRecordList = result.items;
            this.query.total = result.totalCount;
        });
    }

    getText(e: any) {
        if (e) {
            let status: any = this.unitTypes.find(s => s.value == e);
            this.grower.unitName = status.text;
        }
    }

    saveRoomInfo() {
        this.growerService.updateGrowerInfo(this.grower).finally(() => { this.isConfirmLoading = false; })
            .subscribe((result: Grower) => {
                this.grower = result;
                this.grower.plantingArea = Number(this.grower.plantingArea).toFixed(2);
                this.isDelete = true;
                this.notify.info(this.l(this.successMsg), '');
            });
    }

    delete(): void {
        this.confirmModal = this.modal.confirm({
            nzContent: '是否删除烟农信息?',
            nzOnOk: () => {
                this.growerService.deleteGrower(this.grower).subscribe(() => {
                    this.notify.info(this.l('删除成功！'), '');
                    this.return();
                });
            }
        });
    }

    /**
     * 模态框返回
     */
    getSelectData = (employee?: Employee) => {
        if (employee) {
            this.grower.employeeId = employee.id;
            this.grower.employeeName = employee.name;
        }
    }
    showModal(): void {
        this.selectsEmployeeModal.show(this.grower.areaCode);
    }
    return() {
        this.router.navigate(['app/basic/grower']);
    }

    handlePreview = (url: string) => {
        if (!url) {
            this.previewImage = this.defalutImg;
        }
        else {
            this.previewImage = url;
        }
        this.previewVisible = true;
    }

    areaCodeChange() {
        this.grower.employeeName = null;
        this.grower.employeeId = null;
    }
}

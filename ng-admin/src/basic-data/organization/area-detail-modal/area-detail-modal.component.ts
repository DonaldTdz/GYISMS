import { Component, OnInit, Output, EventEmitter, Injector } from '@angular/core';
import { NzModalRef } from 'ng-zorro-antd';
import { Employee } from '@shared/entity/basic-data';
import { FormGroup, FormBuilder } from '@angular/forms';
import { GrowerServiceProxy, EmployeeServiceProxy } from '@shared/service-proxies/basic-data';
import { NotifyService } from 'abp-ng2-module/dist/src/notify/notify.service';
import { AppComponentBase } from '@shared/app-component-base';
@Component({
    moduleId: module.id,
    selector: 'area-detail-modal',
    templateUrl: 'area-detail-modal.component.html',
    styleUrls: ['area-detail-modal.component.scss']
})

export class AreaDetailModalComponent extends AppComponentBase implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    eloading = false;
    isVisible = false;
    employee: Employee = new Employee();
    employeeId: string;
    successMsg = '';
    confirmModal: NzModalRef;
    validateForm: FormGroup;
    // countyCodes: SelectGroup[] = []
    countyCodes: any = [
        { text: '昭化区', value: 1 }, { text: '剑阁县', value: 2 }, { text: '旺苍县', value: 3 }
    ];
    notify: NotifyService;

    constructor(injector: Injector, private employeeService: EmployeeServiceProxy, private fb: FormBuilder
        , private growerService: GrowerServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
        this.validateForm = this.fb.group({
            unitCode: null
        });
    }

    /**
     * 获取
     */
    show(id?: string) {
        if (id) {
            // this.growerService.getCountyCodeSelectGroupAsync().subscribe((result: SelectGroup[]) => {
            //     this.countyCodes = result;
            //     this.getEmployee(id);
            // });
            this.getEmployee(id);
        }
        this.isVisible = true;
    }

    getEmployee(id: string) {
        this.employeeService.getEmployeeById(id).subscribe((result: Employee) => {
            this.employee = result;
        });
    }

    getText(e: any) {
        if (e) {
            let status: any = this.countyCodes.find(s => s.value == e);
            this.employee.area = status.text;
        }
    }

    /**
     * 取消按钮事件
     */
    handleCancel = (e) => {
        this.isVisible = false;
        this.eloading = false;
    }

    save(data: Employee) {
        if (this.validateForm.valid) {
            this.eloading = true;
            this.employee.area = data.area;
            this.employee.areaCode = data.areaCode;
            this.successMsg = '保存成功';
            this.saveAreaInfo(data);
        }
    }
    saveAreaInfo(data: Employee) {
        let params: any = {};
        params.Id = this.employee.id;
        params.Area = data.area;
        params.AreaCode = data.areaCode;
        this.employeeService.updateEmployeeArea(params).finally(() => { this.eloading = false; })
            .subscribe((result: Employee) => {
                this.employee = result;
                this.notify.info(this.successMsg, '');
                this.isVisible = false;
            });
    }
}

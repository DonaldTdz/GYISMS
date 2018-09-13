import { Component, OnInit, Output, EventEmitter } from '@angular/core';
import { FormBuilder } from '@angular/forms';
import { Organization } from '@shared/entity/basic-data';
import { OrganizationServiceProxy, PagedResultDtoOfOrganization } from '@shared/service-proxies/basic-data';

@Component({
    moduleId: module.id,
    selector: 'message-organization-modal',
    templateUrl: 'message-organization-modal.component.html',
})
export class MessageOrganizationModalComponent implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();

    q: any = {
        pi: 0,
        ps: 10,
        name: ''
    };
    eloading = false;
    isVisible = false;
    organizationList: Organization[] = [];
    status = [
        { text: '启用', value: false, type: 'success' },
        { text: '禁用', value: false, type: 'default' }
    ];
    constructor(private OrganizationService: OrganizationServiceProxy) {
    }

    ngOnInit(): void {
    }
    //isManger用判断模态框是否只显示经理级的员工
    show() {
        this.organizationList = new Array<Organization>();
        this.isVisible = true;
        // this.refreshData();
    }

    /**
     * 获取
     */
    refreshData() {
        if (this.q.name != null && this.q.name.trim().length !== 0) {
            this.eloading = true;
            let params: any = {};
            params.SkipCount = this.q.pi;
            params.MaxResultCount = this.q.ps;
            params.Name = this.q.name;
            params.DepartId = '1';
            this.OrganizationService.getAll(params).subscribe((result: PagedResultDtoOfOrganization) => {
                this.eloading = false;
                this.organizationList = result.items;
                this.q.total = result.totalCount;
            });
        }
    }

    /**
     * 取消按钮事件
     */
    handleCancel = (e) => {
        this.isVisible = false;
        this.eloading = false;
        this.q.name = '';
    }
    /**
     * 
     * @param organization 选择事件（对选择的数据进行回传）
     */
    SelectOrganization(organization: Organization): void {
        this.q.name = '';
        this.modalSelect.emit(organization);
        this.isVisible = false;
    }
}

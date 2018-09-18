import { Component, OnInit, Output, EventEmitter, Injector } from '@angular/core';
import { VisitTaskServiceProxy, PagedResultDtoOfVisitTask } from '@shared/service-proxies/tobacco-management';
import { VisitTask } from '@shared/entity/tobacco-management';
import { Employee } from '@shared/entity/basic-data';
import { NzTreeNode, NzDropdownContextComponent, NzFormatEmitEvent } from 'ng-zorro-antd';
import { OrganizationServiceProxy, EmployeeServiceProxy, PagedResultDtoOfEmployee } from '@shared/service-proxies/basic-data';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
    moduleId: module.id,
    selector: 'choose-employee-modal',
    templateUrl: 'choose-employee-modal.component.html',
    styleUrls: ['choose-employee-modal.component.scss']
})
export class ChooseEmployeeModalComponent extends AppComponentBase implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    search: any = {};
    syncDataLoading = false;
    exportLoading = false;
    searchValue;
    loading = false;
    dropdown: NzDropdownContextComponent;
    // can active only one node
    activedNode: NzTreeNode;
    employeeList: Employee[] = [];
    dragNodeElement;
    tempNode: string;
    nodes = [];
    eloading = false;
    isVisible = false;
    taskList: VisitTask[] = [];

    allChecked = false;
    disabledButton = true;
    checkedNumber = 0;
    indeterminate = false;

    checked = true;

    constructor(injector: Injector, private taskService: VisitTaskServiceProxy
        , private organizationService: OrganizationServiceProxy
        , private employeeService: EmployeeServiceProxy) {
        super(injector);
    }

    ngOnInit(): void {
    }

    show() {
        this.taskList = new Array<VisitTask>();
        this.isVisible = true;
        this.getTrees();
        this.refreshData(null);
    }

    getTrees() {
        this.organizationService.GetTreesAsync().subscribe((data) => {
            this.nodes = data;
        });
    }
    /**
     * 获取
     */
    refreshData(departId: string) {
        this.eloading = true;
        this.loading = true;
        let params: any = {};
        params.SkipCount = this.query.skipCount();
        params.MaxResultCount = this.query.pageSize;
        params.departId = departId;
        this.employeeService.getAll(params).subscribe((result: PagedResultDtoOfEmployee) => {
            this.loading = false;
            this.employeeList = result.items;
            this.query.total = result.totalCount;
        })
    }
    // 选中节点
    activeNode(data: NzFormatEmitEvent): void {
        if (this.activedNode) {
            this.activedNode = null;
        }
        data.node.isSelected = true;

        this.activedNode = data.node;
        this.query.pageIndex = 1;
        this.query.pageSize = 10;
        this.search = {};
        this.tempNode = data.node.key;

        this.refreshData(data.node.key);
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
    SelectOrganization(visitTask: VisitTask): void {
        this.modalSelect.emit(visitTask);
        this.isVisible = false;
    }

    // checkAll(value: boolean): void {
    //     this.employeeList.forEach(data => data.checked = value);
    // }
}

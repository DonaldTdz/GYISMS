import { Component, Injector, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ScheduleServiceProxy, VisitTaskServiceProxy } from '@shared/service-proxies/tobacco-management';
import { AppComponentBase } from '@shared/app-component-base';
import { NzFormatEmitEvent, NzTreeNode, NzDropdownContextComponent } from 'ng-zorro-antd';
import { PagedResultDtoOfEmployee, EmployeeServiceProxy, OrganizationServiceProxy } from '@shared/service-proxies/basic-data';
import { VisitTask } from '@shared/entity/tobacco-management';
import { Employee } from '@shared/entity/basic-data';

@Component({
    moduleId: module.id,
    selector: 'assign-task',
    templateUrl: 'assign-task.component.html',
    styleUrls: ['assign-task.component.scss']
})
export class AssignTaskComponent extends AppComponentBase implements OnInit {
    search: any = {};
    syncDataLoading = false;
    exportLoading = false;
    searchValue;
    loading = false;
    dropdown: NzDropdownContextComponent;

    activedNode: NzTreeNode;
    employeeList: Employee[] = [];
    dragNodeElement;
    tempNode: string;
    nodes = [];
    eloading = false;
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
}

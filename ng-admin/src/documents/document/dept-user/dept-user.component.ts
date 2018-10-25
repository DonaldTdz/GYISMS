import { Component, OnInit, Injector, Output, EventEmitter, Input } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { NzTreeNode, NzFormatEmitEvent } from 'ng-zorro-antd';
import { Employee } from '@shared/entity/basic-data';
import { EmployeeServiceProxy, OrganizationServiceProxy, PagedResultDtoOfEmployee } from '@shared/service-proxies/basic-data';

@Component({
    selector: 'dept-user',
    templateUrl: './dept-user.component.html',
    styleUrls: ['./dept-user.component.scss']
})
export class DeptUserComponent extends ModalComponentBase implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();

    selectedDepts = [];
    selectedUsers = [];

    checkedDeptKeys = [];
    activedNode: NzTreeNode;
    employeeList: Employee[] = [];
    dragNodeElement;
    tempNode: string;
    nodes = [];
    userName = '';
    loading = false;
    allChecked = false;
    indeterminate = false;

    constructor(
        injector: Injector, private organizationService: OrganizationServiceProxy, private employeeService: EmployeeServiceProxy
    ) {
        super(injector);
        this.query.pageSize = 5;
    }

    ngOnInit() {
        this.refreshData(null);
        this.getTrees();
    }

    handleDeptClose(tag: any) {
        var i = 0;
        for (const item of this.selectedDepts) {
            //console.log('item:' + item + ' tag:' + tag)
            if (item.id == tag.id) {
                //console.log('llll');
                this.selectedDepts.splice(i, 1);
                break;
            }
            i++;
        }
    }

    handleUserClose(tag: any) {
        var i = 0;
        for (const item of this.selectedUsers) {
            if (item.id == tag.id) {
                this.selectedUsers.splice(i, 1);
                break;
            }
            i++;
        }
    }

    openFolder(data: NzTreeNode | NzFormatEmitEvent): void {
        // do something if u want
        if (data instanceof NzTreeNode) {
            // change node's expand status
            if (!data.isExpanded) {
                // close to open
                data.origin.isLoading = true;
                setTimeout(() => {
                    data.isExpanded = !data.isExpanded;
                    data.origin.isLoading = false;
                }, 500);
            } else {
                data.isExpanded = !data.isExpanded;
            }
        } else {
            // change node's expand status
            if (!data.node.isExpanded) {
                // close to open
                data.node.origin.isLoading = true;
                setTimeout(() => {
                    data.node.isExpanded = !data.node.isExpanded;
                    data.node.origin.isLoading = false;
                }, 500);
            } else {
                data.node.isExpanded = !data.node.isExpanded;
            }
        }
    }

    // 选中节点
    activeNode(data: NzFormatEmitEvent): void {
        if (this.activedNode) {
            this.activedNode = null;
            this.userName = '';
        }
        data.node.isSelected = true;

        this.activedNode = data.node;
        this.query.pageIndex = 1;
        this.query.pageSize = 5;
        this.tempNode = data.node.key;

        this.refreshData(data.node.key);
    }

    refreshData(departId: string, reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.userName = '';
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        let params: any = {};
        params.SkipCount = this.query.skipCount();
        params.MaxResultCount = this.query.pageSize;
        params.departId = departId;
        params.Name = this.userName;
        this.employeeService.getAll(params).subscribe((result: PagedResultDtoOfEmployee) => {
            this.loading = false;
            this.employeeList = result.items;
            this.query.total = result.totalCount;
        })
    }

    getTrees() {
        this.organizationService.GetTreesAsync().subscribe((data) => {
            this.nodes = data;
        });
    }

    checkAll(value: boolean): void {
        this.employeeList.forEach(data => {
            data.checked = value;
        });
        this.refreshStatus(null, null);
    }

    refreshStatus(eve: any, item: any): void {
        const allChecked = this.employeeList.every(value => value.checked === true);
        const allUnChecked = this.employeeList.every(value => !value.checked);
        this.allChecked = allChecked;
        this.indeterminate = (!allChecked) && (!allUnChecked);
        if (item) {
            if (item.checked) {

            }
        }
    }


}

import { Component, OnInit, Injector, Output, EventEmitter, ViewChild, Input } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { NzTreeNode, NzFormatEmitEvent } from 'ng-zorro-antd';
import { Employee } from '@shared/entity/basic-data';
import { EmployeeServiceProxy, OrganizationServiceProxy, PagedResultDtoOfEmployee } from '@shared/service-proxies/basic-data';

@Component({
    selector: 'dept-user',
    templateUrl: './dept-user.component.html',
    styleUrls: [/*'./dept-user.component.scss'*/]
})
export class DeptUserComponent extends ModalComponentBase implements OnInit {
    @Output() modalCancel: EventEmitter<boolean> = new EventEmitter<boolean>();
    @ViewChild('treeCom') treeCom;

    @Input() selectedDepts = [];
    @Input() selectedUsers = [];

    checkedDeptKeys = [];
    orgCheckedKeys = [];
    activedNode: NzTreeNode;
    employeeList: Employee[] = [];
    dragNodeElement;
    tempNode: string;
    nodes = [];
    //orgNodes = [];
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
        this.initOrgCheckedKeys();
        this.refreshData(null);
        this.getTrees();
    }

    initOrgCheckedKeys() {
        this.orgCheckedKeys = this.selectedDepts.map(dept => { return dept.id; });
    }

    handleDeptClose(tag: any) {
        var i = 0;
        for (const item of this.selectedDepts) {
            if (item.id == tag.id) {
                let keys = this.orgCheckedKeys.filter(o => o == item.id);
                if (keys.length > 0) {
                    this.orgCheckedKeys.splice(this.orgCheckedKeys.indexOf(keys[0]), 1);
                }
                this.selectedDepts.splice(i, 1);
                console.log(this.orgCheckedKeys);
                //this.getTrees();
                //this.nodes = this.nodes.copyWithin(this.nodes.length, 0);
                const tempKeys = this.orgCheckedKeys.concat();
                console.log(tempKeys);
                if (tempKeys.length > 0) {
                    this.checkedDeptKeys = tempKeys;
                } else {
                    //this.checkedDeptKeys = [];
                    this.getTrees();
                }
                /*this.nodes = this.nodes.map(node => {
                    if (node.key == item.id) {
                        node.isChecked = false;
                    }
                    return node;
                });
                this.checkedDeptKeys = [];*/
                break;
            }
            i++;
        }
    }

    handleUserClose(tag: any) {
        var i = 0;
        for (const item of this.selectedUsers) {
            if (item.id == tag.id) {
                let users = this.employeeList.filter(e => e.id == item.id);
                if (users.length > 0) {
                    users[0].checked = false;
                }
                this.selectedUsers.splice(i, 1);
                this.refreshStatus(null, null);
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

    //checkbox
    checkBoxChange(data: NzFormatEmitEvent) {
        console.log(data);
        //console.log(this.treeCom.getCheckedNodeList());
        //let items = data.checkedKeys.map(checked => {
        //    return { id: checked.key, name: checked.title };
        //});
        //console.log(items);
        this.orgCheckedKeys = data.keys;
        this.refreshDeptTags({ id: data.node.key, name: data.node.origin.deptName, isChecked: data.node.isChecked });
    }

    refreshDeptTags(item: any) {
        let depts = this.selectedDepts.filter(s => s.id == item.id);
        if (item.isChecked) {
            if (depts.length == 0) {
                this.selectedDepts.push({ id: item.id, name: item.name });
            }
        } else {
            if (depts.length > 0) {
                this.selectedDepts.splice(this.selectedUsers.indexOf(depts[0]), 1);
            }
        }
        /*
        //增加
        let adds = items.filter(i => this.selectedDepts.indexOf({ id: i.id, name: i.name }) < 0);
        for (let item of adds) {
            this.selectedDepts.push({ id: item.id, name: item.name });
        }
        //删除
        let deletes = this.selectedDepts.filter(s => items.indexOf({ id: s.id, name: s.name }) < 0);
        for (let item of deletes) {
            this.selectedDepts.splice(this.selectedUsers.indexOf(item), 1);
        }*/
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
            result.items.map(item => {
                const users = this.selectedUsers.filter(e => e.id == item.id);
                if (users.length > 0) {
                    item.checked = true;
                } else {
                    item.checked = false;
                }
                return item;
            });
            this.employeeList = result.items;
            this.query.total = result.totalCount;
            this.refreshStatus(null, null);
        })
    }

    getTrees() {
        this.organizationService.GetTreesAsync().subscribe((data) => {
            this.nodes = data;
            //this.orgNodes = data;
            this.checkedDeptKeys = this.orgCheckedKeys;
            //console.log(this.checkedDeptKeys);
        });
    }

    checkAll(value: boolean): void {
        this.employeeList.forEach(data => {
            data.checked = value;
            this.refreshUserTags(data);
        });

        this.refreshStatus(null, null);
    }

    refreshStatus(eve: any, item: any): void {
        const allChecked = this.employeeList.every(value => value.checked === true);
        const allUnChecked = this.employeeList.every(value => !value.checked);
        this.allChecked = allChecked;
        this.indeterminate = (!allChecked) && (!allUnChecked);
        if (item) {
            this.refreshUserTags(item);
        }
    }

    refreshUserTags(item: any) {
        let users = this.selectedUsers.filter(s => s.id == item.id);
        if (item.checked) {
            if (users.length == 0) {
                this.selectedUsers.push({ id: item.id, name: item.name });
            }
        } else {
            if (users.length > 0) {
                this.selectedUsers.splice(this.selectedUsers.indexOf(users[0]), 1);
            }
        }
    }

    cancel() {
        this.close();
    }

    ok() {
        this.success(true);
    }
}

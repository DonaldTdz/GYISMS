import { Component, Injector, OnInit, ViewChild, HostListener } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { _HttpClient } from '@delon/theme';
import { Organization, TreeNode, Employee } from '@shared/entity/basic-data';
import { Router } from '@angular/router';
import { PagedResultDtoOfOrganization, OrganizationServiceProxy } from '@shared/service-proxies/basic-data';
import { Parameter } from '@shared/service-proxies/entity/parameter';
import { NzFormatEmitEvent, NzTreeNode, NzDropdownContextComponent } from 'ng-zorro-antd';
import { PagedResultDtoOfEmployee, EmployeeServiceProxy } from '@shared/service-proxies/basic-data/employee-service';

@Component({
    moduleId: module.id,
    selector: 'organization',
    templateUrl: './organization.component.html',
    styleUrls: ['./organization.component.scss']
})

export class OrganizationComponent extends AppComponentBase implements OnInit {

    syncDataLoading = false;
    exportLoading = false;
    search: any = {};
    searchValue;
    loading = false;
    dropdown: NzDropdownContextComponent;
    // can active only one node
    activedNode: NzTreeNode;
    employeeList: Employee[] = [];
    dragNodeElement;
    tempNode: string;
    nodes = [
        // new NzTreeNode({
        //     title: '成都和创科技有限公司',
        //     key: '1',
        //     expanded: true,
        //     children: [
        //         {
        //             title: '技术开发（4人）',
        //             key: '67209026',
        //             isLeaf: true
        //         },
        //         {
        //             title: '运营部（3人）',
        //             key: '67209027',
        //             isLeaf: true
        //         },
        //         {
        //             title: '销售（2人）',
        //             key: '67209029',
        //             isLeaf: true
        //         }
        //     ]
        // })
    ];

    constructor(injector: Injector, private organizationService: OrganizationServiceProxy,
        private employeeService: EmployeeServiceProxy, private router: Router) {
        super(injector);
    }

    ngOnInit(): void {
        this.refreshData(null);
        this.getTrees();
    }

    /**
     * important:
     * if u want to custom event/node properties, u need to maintain the selectedNodesList/checkedNodesList yourself
     * @param {} data
     */
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
        }
        data.node.isSelected = true;

        this.activedNode = data.node;
        this.query.pageIndex = 1;
        this.query.pageSize = 10;
        this.tempNode = data.node.key;

        this.refreshData(data.node.key);
    }

    selectDropdown(): void {
        this.dropdown.close();
        // do something
        console.log('dropdown clicked');
    }



    refreshData(departId: string, reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.search = {};
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        this.employeeService.getAll(this.query.skipCount(), this.query.pageSize, this.getParameter(departId)).subscribe((result: PagedResultDtoOfEmployee) => {
            this.loading = false;
            this.employeeList = result.items;
            this.query.total = result.totalCount;
        })
    }

    getParameter(departId: string): Parameter[] {
        var arry = [];
        arry.push(Parameter.fromJS({ key: 'DepartId', value: departId }));
        arry.push(Parameter.fromJS({ key: 'Name', value: this.search.name }));
        arry.push(Parameter.fromJS({ key: 'Mobile', value: this.search.mobile }));
        return arry;
    }

    syncData() {
        this.syncDataLoading = true;
        this.organizationService.synchronousOrganizationAsync().subscribe(() => {
            this.notify.info(this.l('同步成功！'));
            this.syncDataLoading = false;
        });
        setTimeout(() => {
            this.getTrees();
        }, '1000');
    }

    getTrees() {
        this.organizationService.GetTreesAsync().subscribe((data) => {
            this.nodes = data;
        });
    }

    // getEmoloyee(departId: string) {
    //     this.loading = true;
    //     this.employeeService.getAll(this.query.skipCount(), this.query.pageSize, this.getParameter(departId)).subscribe((result: PagedResultDtoOfEmployee) => {
    //         this.loading = false;
    //         this.employeeList = result.items;
    //         this.query.total = result.totalCount;
    //     })
    // }
}
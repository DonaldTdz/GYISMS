import { Component, Injector, OnInit, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { _HttpClient } from '@delon/theme';
import { Organization, TreeNode } from '@shared/entity/basic-data';
import { Router } from '@angular/router';
import { PagedResultDtoOfOrganization, OrganizationServiceProxy } from '@shared/service-proxies/basic-data';
import { Parameter } from '@shared/service-proxies/entity/parameter';
import { NzFormatEmitEvent, NzTreeNode } from 'ng-zorro-antd';

@Component({
    selector: 'organization',
    templateUrl: './organization.component.html',
    styleUrls: ['./organization.component.scss']
})

export class OrganizationComponent extends AppComponentBase implements OnInit {
    loading = false;
    exportLoading = false;
    syncDataLoading = false;
    search: any = {};
    searchValue;
    organizationList: Organization[] = [];
    x: NzTreeNode[] = [];
    y: TreeNode[] = [];
    treeNode: TreeNode = new TreeNode();
    rootNode = [
        new NzTreeNode({
            title: '',
            key: ''
        })
    ];
    constructor(injector: Injector, private organizationService: OrganizationServiceProxy, private router: Router) {
        super(injector);
    }

    ngOnInit(): void {
        // this.refreshData();
        this.getTree();
    }

    refreshData(reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.search = {};
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        this.organizationService.getAll(this.query.skipCount(), this.query.pageSize).subscribe((result: PagedResultDtoOfOrganization) => {
            this.loading = false;
            this.organizationList = result.items;
            this.query.total = result.totalCount;
        })

    }

    getParameter(): Parameter[] {
        var arry = [];
        // arry.push(Parameter.fromJS({ key: 'Name', value: this.search.name }));
        return arry;

    }

    syncData() {
        this.syncDataLoading = true;
        this.organizationService.synchronousOrganizationAsync().subscribe(() => {
            this.notify.info(this.l('同步成功！'));
            this.syncDataLoading = false;
        });
        setTimeout(() => {
            this.refreshData();
        }, '1000');
    }

    getTree() {
        this.organizationService.getRootOrganization(1).subscribe((result: TreeNode) => {
            this.treeNode = result;
            this.rootNode[0].title = this.treeNode.title;
            this.rootNode[0].key = this.treeNode.key;
        });
    }

    mouseAction(name: string, e: NzFormatEmitEvent): void {
        if (name === 'expand') {
            this.organizationService.getChildOrganization(e.node.key).subscribe((result: TreeNode[]) => {
                this.y = result;
                console.log(this.y);

                // this.x.map(v => v.title = result.filter(v => v.title));
                // this.x.title = this.treeNode.title;
                // this.x.key = this.treeNode.key;
            });
            // if (e.node.getChildren().length === 0 && e.node.isExpanded) {
            //     e.node.addChildren([
            //         {
            //             title: 'childAdd-1',
            //             key: '10031-' + (new Date()).getTime()
            //         },
            //         {
            //             title: 'childAdd-2',
            //             key: '10032-' + (new Date()).getTime(),
            //             isLeaf: true
            //         }]);
            // }
        }
    }
}
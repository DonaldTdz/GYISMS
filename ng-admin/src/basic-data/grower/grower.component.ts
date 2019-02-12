import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Router } from '@angular/router';
import { GrowerServiceProxy, PagedResultDtoOfGrower, OrganizationServiceProxy } from '@shared/service-proxies/basic-data';
import { Grower } from '@shared/entity/basic-data';
import { NzFormatEmitEvent } from 'ng-zorro-antd';

@Component({
    moduleId: module.id,
    selector: 'grower',
    templateUrl: 'grower.component.html',
    styleUrls: ['grower.component.scss']
})
export class GrowerComponent extends AppComponentBase implements OnInit {
    search: any = { isEnable: 1 };
    loading = false;
    growerList: Grower[] = [];
    areaTypes = [{ text: '昭化区', value: 1 },
    { text: '剑阁县', value: 2 },
    { text: '旺苍县', value: 3 }];
    enableTypes = [
        { text: '全部', value: 0 },
        { text: '启用', value: 1 },
        { text: '禁用', value: 2 }
    ]

    nodes = [];

    constructor(injector: Injector, private growerService: GrowerServiceProxy,
        private organizationService: OrganizationServiceProxy,
        private router: Router) {
        super(injector);
    }

    ngOnInit(): void {
        this.getTrees();
        this.refreshData();
    }
    refreshData(reset = false, search?: boolean) {
        if (reset) {
            this.query.pageIndex = 1;
            this.search = { isEnable: 1 };
        }
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        let params: any = {};
        params.SkipCount = this.query.skipCount();
        params.MaxResultCount = this.query.pageSize;
        params.Name = this.search.name;
        params.Employee = this.search.employee;
        params.AreaName = this.search.area;
        params.IsEnableValue = this.search.isEnable === 0 ? null : this.search.isEnable;
        //console.log(params);
        this.growerService.getGrowerListAsync(params).subscribe((result: PagedResultDtoOfGrower) => {
            this.loading = false;
            this.growerList = result.items;
            this.query.total = result.totalCount;
        })
    }

    createRoom() {
        this.router.navigate(['app/basic/grower-detail'])
    }

    goDetail(id: number) {
        this.router.navigate(['app/basic/grower-detail', id])
    }

    getTrees() {
        this.organizationService.GetGrowerTreesAsync().subscribe((data) => {
            this.nodes = data;
        });
    }

    // 选中节点
    activeNode(data: NzFormatEmitEvent): void {
        //data.node.isSelected = true;
        if (this.search.area == data.node.key) {
            return;
        }
        this.search.area = data.node.key;
        this.refreshData();
    }
}

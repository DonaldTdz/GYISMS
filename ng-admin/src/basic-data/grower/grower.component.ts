import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Router } from '@angular/router';
import { GrowerServiceProxy, PagedResultDtoOfGrower } from '@shared/service-proxies/basic-data';
import { Grower } from '@shared/entity/basic-data';

@Component({
    moduleId: module.id,
    selector: 'grower',
    templateUrl: 'grower.component.html',
    styleUrls: ['grower.component.scss']
})
export class GrowerComponent extends AppComponentBase implements OnInit {
    search: any = {};
    loading = false;
    growerList: Grower[] = [];
    areaTypes = [{ text: '昭化区', value: 1 },
    { text: '剑阁县', value: 2 },
    { text: '旺苍县', value: 3 }];
    constructor(injector: Injector, private growerService: GrowerServiceProxy,
        private router: Router) {
        super(injector);
    }

    ngOnInit(): void {
        this.refreshData();
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
        let params: any = {};
        params.SkipCount = this.query.skipCount();
        params.MaxResultCount = this.query.pageSize;
        params.Name = this.search.name;
        params.Employee = this.search.employee;
        params.Area = this.search.area;
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
}

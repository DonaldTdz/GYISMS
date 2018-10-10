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

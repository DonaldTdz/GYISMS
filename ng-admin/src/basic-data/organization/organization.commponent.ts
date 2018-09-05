import { Component, Injector, OnInit } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { _HttpClient } from '@delon/theme';
import { Organization } from '@shared/entity/basic-data';
import { Router } from '@angular/router';
import { PagedResultDtoOfOrganization, OrganizationServiceProxy } from '@shared/service-proxies/basic-data';
import { Parameter } from '@shared/service-proxies/entity/parameter';

@Component({
    selector: 'organization',
    templateUrl: './organization.component.html',
    styleUrls: ['./organization.component.scss'],
    // animations: [appModuleAnimation()],
    //providers: [OrganizationService],
})

export class OrganizationComponent extends AppComponentBase implements OnInit {
    loading = false;
    exportLoading = false;
    syncDataLoading = false;
    search: any = {};
    organizationList: Organization[] = [];
    constructor(injector: Injector, private organizationService: OrganizationServiceProxy, private router: Router) {
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
        this.refreshData();
    }

    goDetail(id: number) {
        this.router.navigate(['organization/organizationDetail/organization-detail', id]);
    }
}
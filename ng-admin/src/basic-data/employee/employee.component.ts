import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { EmployeeServiceProxy, PagedResultDtoOfEmployee } from '@shared/service-proxies/basic-data';
import { Router } from '@angular/router';
import { Employee } from '@shared/entity/basic-data';
import { Parameter } from '@shared/service-proxies/entity/parameter';

@Component({
    moduleId: module.id,
    selector: 'employee',
    templateUrl: 'employee.component.html',
    styleUrls: ['employee.component.scss']
})
export class EmployeeComponent extends AppComponentBase implements OnInit {

    search: any = {};
    loading = false;
    employeeList: Employee[] = [];

    constructor(injector: Injector, private employeeService: EmployeeServiceProxy, private router: Router) {
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
        this.employeeService.getAll(params).subscribe((result: PagedResultDtoOfEmployee) => {
            this.loading = false;
            this.employeeList = result.items;
            this.query.total = result.totalCount;
            console.log(result);

        })
    }
}

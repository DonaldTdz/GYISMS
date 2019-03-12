import { Component, OnInit, Input, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { DocumentDto, PagedResultDtoOfDocument, PagedResultDtoOfAdvise } from '@shared/entity/documents';
import { Router } from '@angular/router';
import { DocumentService } from '@shared/service-proxies/documents';
import { Advise } from '@shared/entity/documents/advise';

@Component({
    selector: 'advise-list',
    templateUrl: 'advise-list.component.html',
    styleUrls: ['advise-list.component.scss']
})
export class AdviseListComponent extends AppComponentBase implements OnInit {

    @Input() deptId: any;
    keyWord: string;
    loading = false;
    adviseList: Advise[] = [];

    // selectedCategory = { id: '', name: '' };

    constructor(injector: Injector, private router: Router, private documentService: DocumentService) {
        super(injector);
    }

    ngOnInit(): void {
        // this.refreshData(true);
    }

    refreshData(search?: boolean) {
        if (search) {
            this.query.pageIndex = 1;
        }
        this.loading = true;
        let params: any = {};
        params.SkipCount = this.query.skipCount();
        params.MaxResultCount = this.query.pageSize;
        params.KeyWord = this.keyWord;
        // params.CategoryId = this.selectedCategory ? this.selectedCategory.id : null;
        params.DeptId = this.deptId;
        this.documentService.getAdvisePaged(params).subscribe((result: PagedResultDtoOfAdvise) => {
            this.loading = false;
            this.adviseList = result.items;
            this.query.total = result.totalCount;
        })
    }
}

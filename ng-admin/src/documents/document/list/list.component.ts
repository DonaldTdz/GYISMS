import { Component, OnInit, Injector, Input } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Router } from '@angular/router';
import { DocumentService } from '@shared/service-proxies/documents';
import { PagedResultDtoOfDocument, DocumentDto } from '@shared/entity/documents';
import { ApiResult } from '@shared/service-proxies/entity/parameter';

@Component({
    selector: 'doc-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.scss']
})
export class DocListComponent extends AppComponentBase implements OnInit {

    @Input() categoryId: any;
    keyWord: string;
    loading = false;
    downloading = false;
    docs: DocumentDto[] = [];

    selectedCategory = { id: '', name: '' };

    constructor(injector: Injector, private router: Router, private documentService: DocumentService) {
        super(injector);
    }

    ngOnInit(): void {
        this.refreshData(true);
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
        //alert(this.selectedCategory ? this.selectedCategory.name : '');
        params.CategoryId = this.selectedCategory ? this.selectedCategory.id : null;
        this.documentService.getPaged(params).subscribe((result: PagedResultDtoOfDocument) => {
            this.loading = false;
            this.docs = result.items;
            this.query.total = result.totalCount;
        })
    }

    create() {
        if (!this.selectedCategory || !this.selectedCategory.id) {
            this.notify.info('请先选择分类');
            return;
        }
        this.router.navigate(['app/doc/doc-detail', { cid: this.selectedCategory.id, cname: this.selectedCategory.name }]);
    }

    edit(item) {
        this.router.navigate(['app/doc/doc-detail', item.id]);
    }

    download() {
        this.downloading = true;
        let params: any = {};
        params.KeyWord = this.keyWord;
        params.CategoryId = this.selectedCategory ? this.selectedCategory.id : null;
        this.documentService.download(params).subscribe((result: ApiResult) => {
            this.downloading = false;
            if (result.code == 0) {
                var url = this.documentService.baseUrl + result.data;
                document.getElementById('aDocZipUrl').setAttribute('href', url);
                document.getElementById('btnDocZipHref').click();
            } else {
                this.notify.error(result.msg);
            }
        })
    }

}

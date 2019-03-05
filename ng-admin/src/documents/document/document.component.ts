import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { DocListComponent } from './list/list.component';
import { NzFormatEmitEvent } from 'ng-zorro-antd';
import { DocumentService } from '@shared/service-proxies/documents';


@Component({
    moduleId: module.id,
    selector: 'document',
    templateUrl: 'document.component.html',
    styleUrls: ['document.component.scss']
})
export class DocumentComponent extends AppComponentBase implements OnInit {

    @ViewChild('docList') docList: DocListComponent;
    categoryName: string;
    //selectedCategory: { id: '', name: '' };
    nodes: any[];

    constructor(injector: Injector, private documentService: DocumentService) {
        super(injector);
    }

    ngOnInit(): void {
        this.getTrees();
    }

    onSelectedCategory(selected: any) {
        //this.selectedCategory = selected;
        //alert(this.selectedCategory.name);
        this.docList.selectedCategory = selected;
        this.docList.refreshData(true);
    }

    getTrees() {
        this.documentService.getDeptDocNzTreeNodes().subscribe((data) => {
            this.nodes = data;
        });
    }

    // 选中节点
    activeNode(data: NzFormatEmitEvent): void {

    }
}

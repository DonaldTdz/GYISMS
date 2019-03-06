import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { DocListComponent } from './list/list.component';
import { NzFormatEmitEvent, NzTreeComponent } from 'ng-zorro-antd';
import { DocumentService } from '@shared/service-proxies/documents';
import { CategoryComponent } from './category/category.component';


@Component({
    moduleId: module.id,
    selector: 'document',
    templateUrl: 'document.component.html',
    styleUrls: ['document.component.scss']
})
export class DocumentComponent extends AppComponentBase implements OnInit {

    @ViewChild('docList') docList: DocListComponent;
    @ViewChild('docCategory') docCategory: CategoryComponent;
    @ViewChild('detpTree') detpTree: NzTreeComponent;
    categoryName: string;
    //selectedCategory: { id: '', name: '' };
    nodes: any[];
    selectedDept: any = { id: '', name: '' };

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
        this.docList.dept = this.selectedDept;
        this.docList.refreshData(true);
    }

    getTrees() {
        this.documentService.getDeptDocNzTreeNodes().subscribe((data) => {
            this.nodes = data;
            if (data.length > 0) {
                var selectedNode = data[0].children[0];
                if (selectedNode && selectedNode.isSelected) {
                    this.selectedDept = { id: selectedNode.key, name: selectedNode.title };
                    this.docCategory.getTreeAsync(selectedNode.key);
                    this.docList.dept = this.selectedDept;
                    this.docList.refreshData(true);
                }
            }
        });
    }

    // 选中节点
    activeNode(data: NzFormatEmitEvent): void {
        if (data.node.key == '0' || data.node.key == '-1') {
            this.selectedDept = { id: '', name: '' };
        } else {
            this.selectedDept = { id: data.node.key, name: data.node.title };
        }
        this.docCategory.getTreeAsync(data.node.key);
        this.docList.dept = this.selectedDept;
        this.docList.refreshData(true);
    }
}

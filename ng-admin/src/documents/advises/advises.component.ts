import { Component, OnInit, ViewChild, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { NzTreeComponent, NzFormatEmitEvent } from 'ng-zorro-antd';
import { DocumentService } from '@shared/service-proxies/documents';
import { AdviseListComponent } from './advise-list/advise-list.component';

@Component({
    selector: 'advises',
    templateUrl: 'advises.component.html',
    styleUrls: ['advises.component.scss']
})
export class AdvisesComponent extends AppComponentBase implements OnInit {
    @ViewChild('adviseList') adviseList: AdviseListComponent;
    @ViewChild('detpTree') detpTree: NzTreeComponent;
    categoryName: string;
    selectedCategory: { id: '', name: '' };
    nodes: any[];
    selectedDept: any = { id: '', name: '' };

    constructor(injector: Injector, private documentService: DocumentService) {
        super(injector);
    }

    ngOnInit(): void {
        this.getTrees();
    }

    getTrees() {
        this.documentService.getDeptDocNzTreeNodes().subscribe((data) => {
            this.nodes = data;
        });
    }

    // 选中节点
    activeNode(data: NzFormatEmitEvent): void {
        if (data.node.key == '0' || data.node.key == '-1') {
            this.selectedDept = { id: '', name: '' };
        } else {
            this.selectedDept = { id: data.node.key, name: data.node.title };
        }
        this.adviseList.deptId = this.selectedDept.id;
        // console.log(this.adviseList.deptId);
        if (this.adviseList.deptId.replace(/(^s*)|(s*$)/g, "").length != 0) {
            this.adviseList.refreshData(true);
        } else {
            this.adviseList.adviseList = [];
            this.adviseList.query.total = 0;
        }
    }
}

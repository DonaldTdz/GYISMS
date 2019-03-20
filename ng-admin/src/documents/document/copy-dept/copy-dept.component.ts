import { Component, OnInit, Injector, Input, ViewChild } from '@angular/core';
import { NzFormatEmitEvent, NzModalService, NzTreeComponent } from 'ng-zorro-antd';
import { DocumentService, CategoryService } from '@shared/service-proxies/documents';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';

@Component({
    selector: 'copy-dept',
    templateUrl: 'copy-dept.component.html',
    styleUrls: ['copy-dept.component.scss']
})
export class CopyDeptComponent extends ModalComponentBase implements OnInit {
    @ViewChild('treeCom') treeCom: NzTreeComponent;
    @Input() rkey: string;
    @Input() rtitle: string;
    nodes: any[];
    nodesCate: any[];
    deptId: string;
    selectedDept: any = { id: '', name: '' };
    selectedCate: any = { id: '', name: '' };

    constructor(injector: Injector
        , private documentService: DocumentService
        , private categoryService: CategoryService
        , private modal: NzModalService) {
        super(injector);
    }

    ngOnInit(): void {
        this.getTrees();
    }
    getTrees() {
        this.documentService.getDeptDocNzTreeNodes().subscribe((data) => {
            this.nodes = data;
            if (data.length > 0) {
                var selectedNode = data[0].children[0];
                if (selectedNode && selectedNode.isSelected) {
                    this.selectedDept = { id: selectedNode.key, name: selectedNode.title };
                    this.getCateTreeAsync(selectedNode.key);
                }
            }
        });
    }

    getCateTreeAsync(deptId?: any) {
        if (!deptId) {
            deptId = this.deptId;
        }
        this.categoryService.getCopyTreeWithRootAsync(deptId).subscribe(res => {
            this.nodesCate = res;
        });
    }

    // 选中节点
    activeNode(data: NzFormatEmitEvent): void {
        if (data.node.key == '0' || data.node.key == '-1') {
            this.selectedDept = { id: '', name: '' };
        } else {
            this.selectedDept = { id: data.node.key, name: data.node.title };
            this.getCateTreeAsync(data.node.key);
        }
    }

    closeOut() {
        this.modal.closeAll();
    }

    // // 选中节点
    activeCateNode(data: NzFormatEmitEvent): void {
        if (data.node.key == '-1') {
            this.selectedCate = { id: '', name: '' };
        } else {
            this.selectedCate = { id: data.node.key, name: data.node.title };
        }
        if (this.selectedCate.id != '') {
            this.modalRef = this.modal.confirm({
                nzContent: `是否将分类复制到[${data.node.title}]层级下?`,
                nzOnOk: () => {
                    let input: any = {};
                    input.DeptId = this.selectedDept.id;
                    input.ParentId = data.node.key;
                    input.CategoryId = this.rkey;
                    this.categoryService.copyCategoryByDeptId(input).subscribe(res => {
                        if (res.code == 0) {
                            this.notify.info('操作成功！', '');
                            this.getCateTreeAsync(this.selectedDept.id);
                        } else {
                            this.notify.info('操作失败！', '');
                        }
                    });
                }
            });
        }
    }
}

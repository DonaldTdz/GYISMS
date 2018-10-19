import { Component, OnInit, Injector, Input, TemplateRef, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
    NzDropdownContextComponent,
    NzDropdownService,
    NzFormatEmitEvent,
    NzTreeComponent,
    NzTreeNode
} from 'ng-zorro-antd';
import { CreateCategoryComponent } from './create-category/create-category.component';
import { EditCategoryComponent } from './edit-category/edit-category.component';
import { Category } from '@shared/entity/documents';

@Component({
    selector: 'doc-category',
    templateUrl: 'category.component.html',
    styleUrls: ['category.component.scss']
})
export class CategoryComponent extends AppComponentBase implements OnInit {

    @Input() name: string;

    @ViewChild('treeCom') treeCom: NzTreeComponent;
    dropdown: NzDropdownContextComponent;
    // actived node
    activedNode: NzTreeNode;
    rkeyNode: { key: '', title: '' };
    nodes = [{
        title: '信息类',
        key: '100',
        //expanded: true,
        desc: '（5）',
        children: [
            { title: '信息A类', key: '1000', isLeaf: true, desc: '（3）' },
            { title: '信息B类', key: '1001', isLeaf: true, desc: '（5）' }
        ]
    }, {
        title: '机械类',
        key: '101',
        desc: '（20）',
        children: [
            { title: '机械A类', key: '1010', isLeaf: true, desc: '（10）' },
            { title: '机械B类', key: '1011', isLeaf: true, desc: '（12）' }
        ]
    }];


    constructor(injector: Injector, private nzDropdownService: NzDropdownService) {
        super(injector);
    }

    ngOnInit(): void {

    }

    openFolder(data: NzTreeNode | NzFormatEmitEvent): void {
        // do something if u want
        if (data instanceof NzTreeNode) {
            data.isExpanded = !data.isExpanded;
        } else {
            data.node.isExpanded = !data.node.isExpanded;
        }
    }

    activeNode(data: NzFormatEmitEvent): void {
        if (this.activedNode) {
            // delete selectedNodeList(u can do anything u want)
            this.treeCom.nzTreeService.setSelectedNodeList(this.activedNode, false);
        }
        data.node.isSelected = true;
        this.activedNode = data.node;
        // add selectedNodeList
        this.treeCom.nzTreeService.setSelectedNodeList(this.activedNode, false);
    }

    contextMenu($event: MouseEvent, template: TemplateRef<void>, node): void {
        this.dropdown = this.nzDropdownService.create($event, template);
        this.rkeyNode = node;
    }

    edit(): void {
        if (this.dropdown) {
            this.dropdown.close();
        }
        var category = new Category();
        category.id = parseInt(this.rkeyNode.key);
        category.name = this.rkeyNode.title;

        this.modalHelper
            .open(EditCategoryComponent, { category: category }, 'md', {
                nzMask: true,
                nzClosable: false,
            })
            .subscribe(isSave => {
                if (isSave) {
                    //this.refresh();
                }
            });
    }

    create(key: 'click' | 'r-key'): void {
        console.table(key);
        if (this.dropdown) {
            this.dropdown.close();
        }
        var pid;
        var pname;
        if (key === 'r-key') {
            pid = this.rkeyNode.key;
            pname = this.rkeyNode.title;
        }
        this.modalHelper
            .open(CreateCategoryComponent, { pid: pid, pname: pname }, 'md', {
                nzMask: true,
                nzClosable: false,
            })
            .subscribe(isSave => {
                if (isSave) {
                    //this.refresh();
                }
            });
    }

}

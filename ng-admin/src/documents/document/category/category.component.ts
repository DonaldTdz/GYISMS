import { Component, OnInit, Injector, Input, TemplateRef, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
    NzDropdownContextComponent,
    NzDropdownService,
    NzFormatEmitEvent,
    NzTreeComponent,
    NzTreeNode
} from 'ng-zorro-antd';

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
    nodes = [{
        title: '信息类（5）',
        key: '100',
        //expanded: true,
        children: [
            { title: '信息A类（3）', key: '1000', isLeaf: true },
            { title: '信息B类（5）', key: '1001', isLeaf: true }
        ]
    }, {
        title: '机械类（20）',
        key: '101',
        children: [
            { title: '机械A类（10）', key: '1010', isLeaf: true },
            { title: '机械B类（12）', key: '1011', isLeaf: true }
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

    contextMenu($event: MouseEvent, template: TemplateRef<void>): void {
        this.dropdown = this.nzDropdownService.create($event, template);
    }

    edit(): void {
        this.dropdown.close();
    }

    create(): void {
        this.dropdown.close();
    }

}

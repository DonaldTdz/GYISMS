import { Component, OnInit, Injector, Input, Output, TemplateRef, ViewChild, EventEmitter } from '@angular/core';
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
import { CategoryService } from '@shared/service-proxies/documents';


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
    rkeyNode = { key: '', title: '', origin: { parentId: null } };
    nodes = [];
    searchName;
    @Output() selectedCategory = new EventEmitter<any>();


    constructor(injector: Injector, private nzDropdownService: NzDropdownService, private categoryService: CategoryService) {
        super(injector);
    }

    ngOnInit(): void {
        this.getTreeAsync();
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
        //alert(this.activedNode.title);
        if (this.selectedCategory) {
            var catg = { id: this.activedNode.key, name: this.activedNode.title };
            this.selectedCategory.emit(catg);
        }
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
        category.parentId = this.rkeyNode.origin.parentId;
        this.modalHelper
            .open(EditCategoryComponent, { category: category }, 'md', {
                nzMask: true,
                nzClosable: false,
            })
            .subscribe(isSave => {
                if (isSave) {
                    this.getTreeAsync();
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
                    this.getTreeAsync();
                }
            });
    }

    getTreeAsync() {
        this.categoryService.getTreeAsync().subscribe(res => {
            /*if (this.treeCom) {
                let expNodes = this.treeCom.getExpandedNodeList();
                //console.table(expNodes);
                if (expNodes.length > 0) {
                    for (let i in res) {
                        for (let en of expNodes) {
                            if (en.key == res[i].key) {
                                res[i].expanded = true;
                                return;
                            }
                        }
                    }
                }
            }*/
            this.nodes = res;
        });
    }

    nzEvent(event: NzFormatEmitEvent): void {
        //console.log(event, this.treeCom.getMatchedNodeList().map(v => v.title));
    }


}

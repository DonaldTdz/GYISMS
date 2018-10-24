import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { DocListComponent } from './list/list.component';


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

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {

    }

    onSelectedCategory(selected: any) {
        //this.selectedCategory = selected;
        //alert(this.selectedCategory.name);
        this.docList.selectedCategory = selected;
        this.docList.refreshData(true);
    }
}

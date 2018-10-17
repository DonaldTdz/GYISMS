import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import {
    NzDropdownContextComponent,
    NzDropdownService,
    NzFormatEmitEvent,
    NzTreeComponent,
    NzTreeNode
} from 'ng-zorro-antd';

@Component({
    moduleId: module.id,
    selector: 'document',
    templateUrl: 'document.component.html',
    styleUrls: ['document.component.scss']
})
export class DocumentComponent extends AppComponentBase implements OnInit {

    categoryName: string;

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {

    }
}

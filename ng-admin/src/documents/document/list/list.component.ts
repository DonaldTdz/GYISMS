import { Component, OnInit, Injector, Input } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';

@Component({
    selector: 'doc-list',
    templateUrl: 'list.component.html',
    styleUrls: ['list.component.scss']
})
export class DocListComponent extends AppComponentBase implements OnInit {

    @Input() categoryId: any;
    key: string;
    docs: [{ name: '信息白皮书', categoryDesc: '信息类/信息A类', deptDesc: '', summary: 'ffff', employeeDes: '', releaseDate: '' },
        { name: '信息红匹数', categoryDesc: '信息类/信息B类', deptDesc: '', summary: 'ffff', employeeDes: '', releaseDate: '' }];

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {

    }

}

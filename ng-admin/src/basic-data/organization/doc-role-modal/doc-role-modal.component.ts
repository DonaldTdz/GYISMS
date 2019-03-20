import { Component, OnInit, Injector } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { SelectGroup } from '@shared/entity/basic-data';
import { GrowerServiceProxy } from '@shared/service-proxies/basic-data';

@Component({
    selector: 'doc-role-modal',
    templateUrl: 'doc-role-modal.component.html',
    styleUrls: ['doc-role-modal.component.scss']
})
export class DocRoleModalComponent extends AppComponentBase implements OnInit {
    docRoles: SelectGroup[] = [];
    loadingRole = false;
    isVisible = false;
    curDocRole: string;
    roleCode: string = null;
    ids: string;
    constructor(injector: Injector
        , private growerService: GrowerServiceProxy
    ) {
        super(injector);
    }

    ngOnInit(): void {
    }

    show(checkedEmoloyeeIds: string) {
        if (checkedEmoloyeeIds) {
            this.ids = checkedEmoloyeeIds;
            this.growerService.GetDocRoleTypeAsync().subscribe((result: SelectGroup[]) => {
                this.docRoles = result;
            });
        }
        this.isVisible = true;
    }

    save() {
        let input: any = {};
        input.EmployeeIds = this.ids;
        input.RoleCode = this.roleCode;
        this.growerService.updateEmployeeDocRole(input)
            .subscribe(() => {
                this.notify.info('操作成功', '');
                this.isVisible = false;
            });
    }

    /**
     * 取消按钮事件
     */
    handleCancel = (e) => {
        this.isVisible = false;
        this.loadingRole = false;
    }
}

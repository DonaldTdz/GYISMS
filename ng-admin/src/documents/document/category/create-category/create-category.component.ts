import { Component, OnInit, Injector, Output, EventEmitter, Input } from '@angular/core';
import { ModalFormComponentBase } from '@shared/component-base/modal-form-component-base';
import { CategoryService } from '@shared/service-proxies/documents';
import { Validators, FormControl } from '@angular/forms';
import { Category } from '@shared/entity/documents';

@Component({
    selector: 'create-category',
    templateUrl: './create-category.component.html',
    styles: []
})
export class CreateCategoryComponent extends ModalFormComponentBase<Category> implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    category: Category = new Category();

    @Input() pid: number = 0;
    @Input() pname: string;
    @Input() names: any[];

    constructor(
        injector: Injector,
        private _categoryService: CategoryService
    ) {
        super(injector);
    }

    ngOnInit() {
        this.validateForm = this.formBuilder.group({
            name: ['', [Validators.required]]
        });
        this.category.parentId = this.pid;
    }


    duplicateValidator = (control: FormControl): { [s: string]: boolean } => {
        if (!control.value) {
            return { required: true };
        } else if (this.names.indexOf(control.value) > -1) {
            return { duplicate: true, error: true };
        }
    }


    protected submitExecute(finisheCallback: Function): void {
        this._categoryService.createOrUpdate(this.category)
            .finally(() => { this.saving = false; })
            .subscribe(res => {
                this.notify.info(this.l('SavedSuccessfully'), '');
                this.success(true);
            });
    }

    protected setFormValues(entity: Category): void {

    }

    protected getFormValues(): void {
        this.category.name = this.getControlVal('name');
    }
}

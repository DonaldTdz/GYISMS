import { Component, OnInit, Injector, Output, EventEmitter, Input } from '@angular/core';
import { ModalFormComponentBase } from '@shared/component-base/modal-form-component-base';
import { CategoryService } from '@shared/service-proxies/documents';
import { Validators, FormControl } from '@angular/forms';
import { Category } from '@shared/entity/documents';

@Component({
    selector: 'edit-category',
    templateUrl: './edit-category.component.html',
    styles: []
})
export class EditCategoryComponent extends ModalFormComponentBase<Category> implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    @Input() category: Category = new Category();
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
        this.setFormValues(this.category);
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
        this.setControlVal('name', entity.name);
    }

    protected getFormValues(): void {
        this.category.name = this.getControlVal('name');

    }
}

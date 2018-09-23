import { Component, OnInit, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { DataConfigServiceProxy } from '@shared/service-proxies/config/data-config-service';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SystemData } from '@shared/entity/config/system-data';
import { isThisMonth } from 'date-fns';

@Component({
    moduleId: module.id,
    selector: 'data-config-edit-modal',
    templateUrl: 'data-config-edit.component.html',
    styleUrls: ['data-config-edit.component.scss']
})
export class DataConfigEditComponent extends AppComponentBase implements OnInit {
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    systemData: SystemData = new SystemData();
    modelId = null;
    emodalVisible = false;
    iseConfirmLoading = false;
    forme: FormGroup;
    constructor(injector: Injector, private dataConfigService: DataConfigServiceProxy, private fb: FormBuilder) {
        super(injector);
    }
    ngOnInit(): void {
        this.forme = this.fb.group({
            type: [null, Validators.compose([Validators.required])],
            code: [null, Validators.compose([Validators.required])],
            desc: [null, Validators.compose([Validators.required, Validators.maxLength(500)])],
            seq: [null, Validators.compose([Validators.pattern('^[0-9]*$')])],
        });
    }
    show(modelId, id) {
        console.log("id:" + id)
        this.modelId = modelId;
        this.getSingleConfigById(id);
        this.emodalVisible = true;
    }
    getSingleConfigById(id) {
        console.log("aa");
        this.dataConfigService.getSingleConfigById(id).subscribe(data => {
            console.log("data:" + data);
            this.systemData = data;
        });
    }
    ehandleCancel = (e) => {
        this.emodalVisible = false;
        this.iseConfirmLoading = false;
        this.reset();
    }
    reset(e?): void {
        if (e) {
            e.preventDefault();
        }
        this.forme.reset();
        for (const key in this.forme.controls) {
            this.forme.controls[key].markAsPristine();
        }
    }
    saveEd() {
        for (const i in this.forme.controls) {
            this.forme.controls[i].markAsDirty();
        }
        if (this.forme.valid) {
            this.iseConfirmLoading = true;
            this.dataConfigService.update(this.systemData)
                .finally(() => { this.iseConfirmLoading = false; })
                .subscribe(() => {
                    this.emodalVisible = false;
                    this.notify.info(this.l("保存成功！"));
                    this.modalSave.emit(this.modelId);
                });
        }
    }
}

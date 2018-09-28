import { Component, OnInit, Injector, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { SystemData } from '@shared/entity/config/system-data';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { DataConfigServiceProxy } from '@shared/service-proxies/config/data-config-service';

@Component({
    moduleId: module.id,
    selector: 'data-config-create-modal',
    templateUrl: 'data-config-create.component.html',
    styleUrls: ['data-config-create.component.scss']
})
export class DataConfigCreateComponent extends AppComponentBase implements OnInit {
    @Output() modalSave: EventEmitter<any> = new EventEmitter<any>();
    dataconfig: SystemData = new SystemData();
    modeId = null;
    configLeaf = [
        { value: 3, text: '烟农单位' },
        { value: 4, text: '烟农村组' },
        { value: 5, text: '烟叶公共' },
    ];
    configMetting = [
        { value: 1, text: '设备配置' },
        { value: 2, text: '会议物资' },
    ]
    config: any = [];
    form: FormGroup;
    modalVisible = false;
    isConfirmLoading = false;
    loading = false;
    constructor(injector: Injector, private dataConfigServcie: DataConfigServiceProxy, private fb: FormBuilder) {
        super(injector);
    }
    ngOnInit(): void {
        this.form = this.fb.group({
            type: [null, Validators.compose([Validators.required])],
            code: [null, Validators.compose([Validators.required])],
            desc: [null, Validators.compose([Validators.required, Validators.maxLength(500)])],
            seq: [null, Validators.compose([Validators.pattern('^[0-9]*$')])],
        });
    }
    show(modeId: number, type: number) {
        this.reset();
        this.modeId = modeId;
        this.dataconfig.init({ modelId: modeId, type: type });
        if (modeId == 1) {
            this.config = this.configMetting;
        } else {
            this.config = this.configLeaf;
        }
        this.modalVisible = true;
    }

    chandleCancel = (e) => {
        this.modalVisible = false;
        this.isConfirmLoading = false;
        this.reset(e);
    }
    reset(e?): void {
        if (e) {
            e.preventDefault();
        }
        this.form.reset();
        for (const key in this.form.controls) {
            this.form.controls[key].markAsPristine();
        }
    }
    save() {
        for (const i in this.form.controls) {
            this.form.controls[i].markAsDirty();
        }
        if (this.form.valid) {
            this.isConfirmLoading = true;
            this.dataConfigServcie.update(this.dataconfig)
                .finally(() => { this.isConfirmLoading = false })
                .subscribe(() => {
                    this.modalVisible = false;
                    this.notify.info(this.l('保存成功！'));
                    this.modalSave.emit(this.modeId);

                });
        }
    }
}

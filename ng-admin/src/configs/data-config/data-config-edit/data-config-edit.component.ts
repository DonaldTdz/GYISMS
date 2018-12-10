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
    configLeaf = [
        { value: 3, text: '烟农单位' },
        { value: 4, text: '烟农村组' },
        { value: 5, text: '烟叶公共' },
    ];
    configMetting = [
        { value: 1, text: '设备配置' },
        { value: 2, text: '会议物资' },
    ];
    configDing = [
        { value: 6, text: '钉钉配置', selected: true },
        { value: 7, text: '任务拜访', selected: false },
        { value: 8, text: '智能报表', selected: false },
        { value: 9, text: '会议申请', selected: false },
        { value: 10, text: '资料库', selected: false },
    ]
    confige = [];
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
        this.modelId = modelId;
        if (modelId == 1) {
            this.confige = this.configMetting;
        } else if (modelId == 2) {
            this.confige = this.configLeaf;
        } else {
            this.confige = this.configDing;
        }
        this.getSingleConfigById(id);
        this.emodalVisible = true;
    }
    getSingleConfigById(id) {
        this.dataConfigService.getSingleConfigById({ id: id }).subscribe(data => {
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
        console.log('save')
        for (const i in this.forme.controls) {
            this.forme.controls[i].markAsDirty();
        }
        if (this.forme.valid) {
            this.iseConfirmLoading = true;
            this.dataConfigService.update(this.systemData)
                .finally(() => { this.iseConfirmLoading = false; })
                .subscribe(() => {
                    this.emodalVisible = false;
                    this.notify.info(this.l("保存成功！"), '');
                    this.modalSave.emit(this.modelId);
                });
        }
    }
}

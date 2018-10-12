import { Component, OnInit, Injector, ViewChild } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { SystemData } from '@shared/entity/config/system-data';
import { Parameter } from '@shared/service-proxies/entity/parameter';
import { NzModalService } from 'ng-zorro-antd';
import { CLEAN_PROMISE } from '@angular/core/src/render3/instructions';
import { DataConfigCreateComponent } from './data-config-create/data-config-create.component';
import { DataConfigEditComponent } from './data-config-edit/data-config-edit.component';
import { DataConfigServiceProxy } from '@shared/service-proxies/config/data-config-service';

@Component({
    moduleId: module.id,
    selector: 'data-config',
    templateUrl: 'data-config.component.html',
    styleUrls: ['data-config.component.scss']
})
export class DataConfigComponent extends AppComponentBase implements OnInit {
    @ViewChild('createDataConfigModal') createDataConfigModal: DataConfigCreateComponent;
    @ViewChild('editDataConfigModal') editDataConfigModal: DataConfigEditComponent;

    systemDataLeafs: SystemData[] = [];
    systemDataMettings: SystemData[] = [];
    systemDataDings: SystemData[] = [];
    paramLe: any = { Type: 3 };
    paramMe: any = { Type: 1 };
    paramDi: any = { Type: 6 };
    queryMe: any = {
        pageIndex: 1,
        pageSize: 10,
        skipCount: function () { return (this.pageIndex - 1) * this.pageSize; },
        total: 0,
    };
    queryDi: any = {
        pageIndex: 1,
        pageSize: 10,
        skipCount: function () { return (this.pageIndex - 1) * this.pageSize; },
        total: 0,
    };
    mettingName = '';
    leafName = '';
    configLeaf = [
        { value: 3, text: '烟农单位', selected: true },
        { value: 4, text: '烟农村组', selected: false },
        { value: 5, text: '烟叶公共', selected: false },
    ];
    configMetting = [
        { value: 1, text: '设备配置', selected: true },
        { value: 2, text: '会议物资', selected: false },
    ]
    configDing = [
        { value: 6, text: '钉钉配置', selected: true },
        { value: 7, text: '任务拜访', selected: false },
        { value: 8, text: '智能报表', selected: false },
        { value: 9, text: '会议申请', selected: false },
    ]
    loading = false;
    loadingMe = false;
    loadingDi = false;
    constructor(injector: Injector, private systemDataSerice: DataConfigServiceProxy, private modal: NzModalService) {
        super(injector);
    }

    ngOnInit(): void {
        this.getAllLeafs();
        this.getAllMetting();
        this.getAllDing();
    }

    refureshData(type) {
        if (type == 1) {
            this.getAllMetting();
        } else if (type == 2) {
            this.getAllLeafs();
        } else {
            this.getAllDing();
        }

    }
    //#region 烟叶
    /**
     * 烟叶模块
     */
    getAllLeafs() {
        this.paramLe.skipCount = this.query.skipCount();
        this.paramLe.maxResultCount = this.query.pageSize;
        this.paramLe.ModelId = 2;
        this.loading = true;
        this.systemDataSerice.getAll(this.paramLe).subscribe(data => {
            this.loading = false;
            this.systemDataLeafs = data.items;
            this.query.total = data.totalCount;
        });
    }
    editLeaf(leaf: SystemData) {
        this.editDataConfigModal.show(2, leaf.id);
    }

    deleteLeaf(leaf, tplContent) {
        this.leafName = leaf.code;
        this.modal.warning({
            nzContent: tplContent,
            nzOkText: '确定',
            nzCancelText: '取消',
            nzOnOk: () => {
                this.systemDataSerice.delete({ id: leaf.id }).subscribe(() => {
                    this.notify.info(this.l('删除成功！'), '');
                    this.getAllLeafs();
                });
            }
        });
    }

    checkChangeLeaf(item) {
        this.paramLe.Type = item.value;
        this.getAllLeafs();
    }
    addLeaf() {
        this.createDataConfigModal.show(2, this.paramLe.Type);//2表示烟叶模块
    }

    //#endregion

    //#region 会议室

    /**
    * 会议室模块
    */
    getAllMetting() {
        this.paramMe.skipCount = this.queryMe.skipCount();
        this.paramMe.maxResultCount = this.queryMe.pageSize;
        this.paramMe.ModelId = 1;
        this.loadingMe = true;
        this.systemDataSerice.getAll(this.paramMe).subscribe(data => {
            this.loadingMe = false;
            this.systemDataMettings = data.items.map(i => {
                return i;
            });
            this.queryMe.total = data.totalCount;

        });
    }

    editMetting(metting: SystemData) {
        this.editDataConfigModal.show(1, metting.id);
    }

    deleteMetting(metting, tplContent) {
        this.mettingName = metting.code;
        this.modal.warning({
            nzContent: tplContent,
            nzOkText: '确定',
            nzCancelText: '取消',
            nzOnOk: () => {
                this.systemDataSerice.delete({ id: metting.id }).subscribe(() => {
                    this.notify.info(this.l('删除成功！'), '');
                    this.getAllMetting();
                })
            }
        })
    }

    checkChangeMetting(item) {
        this.paramMe.Type = item.value;
        this.getAllMetting();
    }

    addConfigMetting() {
        this.createDataConfigModal.show(1, this.paramMe.Type);//1表示会议室模块
    }
    //#endregion 

    //#region 钉钉配置
    getAllDing() {
        this.paramDi.skipCount = this.queryDi.skipCount();
        this.paramDi.maxResultCount = this.queryDi.pageSize;
        this.paramDi.ModelId = 3;
        this.loadingDi = true;
        this.systemDataSerice.getAll(this.paramDi).subscribe(data => {
            this.loadingDi = false;
            this.systemDataDings = data.items.map(i => {
                return i;
            });
            this.queryDi.total = data.totalCount;

        });
    }
    editDing(ding: SystemData) {
        this.editDataConfigModal.show(3, ding.id);
    }

    deleteDing(ding, tplContent) {
        this.mettingName = ding.code;
        this.modal.warning({
            nzContent: tplContent,
            nzOkText: '确定',
            nzCancelText: '取消',
            nzOnOk: () => {
                this.systemDataSerice.delete({ id: ding.id }).subscribe(() => {
                    this.notify.info(this.l('删除成功！'), '');
                    this.getAllDing();
                })
            }
        })
    }
    checkChangeconfigDing(item) {
        this.paramDi.Type = item.value;
        this.getAllDing();
    }
    addConfigDing() {
        this.createDataConfigModal.show(3, this.paramDi.Type);//1表示会议室模块2表示烟服务3表示钉钉配置
    }
    //#endregion

}

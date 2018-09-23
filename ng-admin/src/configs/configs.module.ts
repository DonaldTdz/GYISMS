import { CommonModule } from '@angular/common';
import { HttpClientModule } from '@angular/common/http';
import { LayoutModule } from 'layout/layout.module';
import { SharedModule } from '@shared/shared.module';
import { AbpModule } from 'abp-ng2-module/dist/src/abp.module';
import { DataConfigComponent } from './data-config/data-config.component';
import { ConfigsRoutingModule } from './configs-routing.module';
import { NgModule } from '@angular/core';
import { DataConfigCreateComponent } from './data-config/data-config-create/data-config-create.component';
import { DataConfigEditComponent } from './data-config/data-config-edit/data-config-edit.component';

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        ConfigsRoutingModule,
        LayoutModule,
        SharedModule,
        AbpModule,
    ],
    declarations: [
        DataConfigComponent,
        DataConfigCreateComponent,
        DataConfigEditComponent,
    ],
    entryComponents: [
    ],
    providers: [],
})
export class ConfigModule { }
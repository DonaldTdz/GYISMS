import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbpModule } from 'abp-ng2-module/dist/src/abp.module';
import { LocalizationService } from 'abp-ng2-module/dist/src/localization/localization.service';
import { LayoutModule } from '../layout/layout.module';
import { SharedModule } from '@shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { DocumentsRoutingModule } from './documents-routing.module';
import { DocumentComponent } from './document/document.component';
import { CategoryComponent } from './document/category/category.component';
import { DocListComponent } from './document/list/list.component';

@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        DocumentsRoutingModule,
        LayoutModule,
        SharedModule,
        AbpModule,
    ],
    declarations: [
        DocumentComponent,
        CategoryComponent,
        DocListComponent
    ],
    entryComponents: [
    ],
    providers: [LocalizationService],
})
export class DocumentsModule { }

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
import { CreateCategoryComponent } from './document/category/create-category/create-category.component';
import { EditCategoryComponent } from './document/category/edit-category/edit-category.component';
import { DocumentDetailComponent } from './document/detail/document-detail.component';
import { UploadFileComponent } from './document/upload-file/upload-file.component';
import { DeptUserComponent } from './document/dept-user/dept-user.component';

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
        DocListComponent,
        CreateCategoryComponent,
        EditCategoryComponent,
        DocumentDetailComponent,
        UploadFileComponent,
        DeptUserComponent
    ],
    entryComponents: [
        CreateCategoryComponent,
        EditCategoryComponent,
        DocumentDetailComponent,
        UploadFileComponent,
        DeptUserComponent
    ],
    providers: [LocalizationService],
})
export class DocumentsModule { }

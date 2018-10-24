import { Component, OnInit, Injector, Input } from '@angular/core';
import { FormComponentBase } from '@shared/component-base/form-component-base';
import { DocumentDto } from '@shared/entity/documents';
import { Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentService } from '@shared/service-proxies/documents';
import { UploadFileComponent } from '../upload-file/upload-file.component';

@Component({
    selector: 'doc-detail',
    templateUrl: 'document-detail.component.html',
    styleUrls: ['document-detail.component.scss']
})
export class DocumentDetailComponent extends FormComponentBase<DocumentDto> implements OnInit {

    qrCode = {
        value: '',
        background: 'white',
        backgroundAlpha: 1.0,
        foreground: 'black',
        foregroundAlpha: 1.0,
        level: 'M',
        mime: 'image/png',
        padding: 10,
        size: 230
    }

    document: DocumentDto = new DocumentDto();
    category = { id: '', name: '' };
    isDelete = false;
    id: '';
    codeStyle = 'none';

    constructor(injector: Injector, private actRouter: ActivatedRoute, private router: Router, private documentService: DocumentService) {
        super(injector);
        var cid = this.actRouter.snapshot.params['cid'];
        if (cid) {
            this.category.id = cid;
            this.category.name = this.actRouter.snapshot.params['cname'];
        }
        this.id = this.actRouter.snapshot.params['id'];
        //alert(this.id);
        //console.table(this.actRouter.snapshot.params);
    }

    ngOnInit(): void {
        this.validateForm = this.formBuilder.group({
            name: ['', [Validators.required, Validators.maxLength(200)]],
            summary: ['', [Validators.required, Validators.maxLength(1000)]],
            content: [null],
            releaseDate: [this.getDateFormat(), [Validators.required]]
        });
        this.getById();
    }

    protected submitExecute(finisheCallback: Function): void {
        this.documentService.createOrUpdate(this.document)
            .finally(() => { this.saving = false; })
            .subscribe(res => {
                this.notify.info(this.l('SavedSuccessfully'), '');
                this.document.id = res.data;
                this.qrCode.value = this.document.id;
                this.isDelete = true;
                this.codeStyle = 'block';
                //console.table(res);
            });
    }

    getById() {
        if (this.id) {
            this.documentService.getById(this.id).subscribe(res => {
                this.document = res;
                this.setFormValues(this.document);
            });
        }
    }

    protected setFormValues(entity: DocumentDto): void {
        this.setControlVal('name', entity.name);
        this.setControlVal('summary', entity.summary);
        this.setControlVal('content', entity.content);
        this.setControlVal('releaseDate', entity.releaseDate);
        this.category.id = entity.categoryId.toString();
        this.category.name = entity.categoryDesc;
        this.qrCode.value = entity.id;
        this.isDelete = true;
        this.codeStyle = 'block';
    }

    protected getFormValues(): void {
        this.document.name = this.getControlVal('name');
        this.document.summary = this.getControlVal('summary');
        this.document.content = this.getControlVal('content');
        this.document.isAllUser = true;
        this.document.releaseDate = this.getControlVal("releaseDate");
        this.document.categoryId = this.category.id ? parseInt(this.category.id) : 0;
        this.document.categoryDesc = this.category.name;
    }

    protected return() {
        this.router.navigate(['app/doc/document']);
    }

    uploadFile() {
        let att = { docId: this.document.id };
        this.modalHelper
            .open(UploadFileComponent, { attachment: att }, 'md', {
                nzMask: true,
                nzClosable: false,
            })
            .subscribe(isSave => {
                if (isSave) {

                }
            });
    }

}

import { Component, OnInit, Injector, Input } from '@angular/core';
import { FormComponentBase } from '@shared/component-base/form-component-base';
import { DocumentDto, Attachment } from '@shared/entity/documents';
import { Validators, FormControl } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { DocumentService, AttachmentService } from '@shared/service-proxies/documents';
import { UploadFileComponent } from '../upload-file/upload-file.component';
import { NzModalRef, NzModalService } from 'ng-zorro-antd';
import { log } from 'util';
import { DeptUserComponent } from '../dept-user/dept-user.component';

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
    attachments = [];
    confirmModal: NzModalRef;
    isAllUser = '1';
    userDesc = '';
    deptTags = ['信息中心', '技术部'];
    userTags = ['唐德舟', '杨帆', '王晓雪'];

    constructor(injector: Injector, private actRouter: ActivatedRoute, private router: Router
        , private documentService: DocumentService
        , private attachmentService: AttachmentService
        , private modal: NzModalService) {
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
            releaseDate: [this.getDateFormat(), [Validators.required]],
            isAllUser: ['1']
        });
        this.getById();
        this.getAttachments();
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
                //alert(this.isAllUser)
            });
        }
    }

    getAttachments() {
        if (this.id) {
            var param = { docId: this.id };
            this.attachmentService.getListByDocIdAsync(param).subscribe(res => {
                this.attachments = res;
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
        this.isAllUser = entity.isAllUser === true ? '1' : '0';
        this.setControlVal('isAllUser', this.isAllUser);
        this.setUserDesc(entity);
    }

    setUserDesc(entity: DocumentDto) {
        if (entity.deptDesc) {
            this.userDesc = entity.deptDesc;
        }
        if (entity.employeeDes) {
            this.userDesc = (this.userDesc ? (this.userDesc + ',' + entity.employeeDes) : entity.employeeDes);
        }
    }

    protected getFormValues(): void {
        this.document.name = this.getControlVal('name');
        this.document.summary = this.getControlVal('summary');
        this.document.content = this.getControlVal('content');
        this.isAllUser = this.getControlVal('isAllUser');
        this.document.isAllUser = this.isAllUser == '1' ? true : false;
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

    deleteAttachment(itemid) {
        this.confirmModal = this.modal.confirm({
            nzContent: '是否删除资料文档?',
            nzOnOk: () => {
                this.attachmentService.delete(itemid).subscribe(() => {
                    this.notify.info(this.l('删除成功！'), '');
                    this.getAttachments();
                });
            }
        });
    }

    allUserRadioChange(ngmodel: string) {
        //alert(ngmodel);
        this.isAllUser = ngmodel;
    }

    handleDeptClose(tag: string) {
        var i = 0;
        for (const item of this.deptTags) {
            //console.log('item:' + item + ' tag:' + tag)
            if (item == tag) {
                //console.log('llll');
                this.deptTags.splice(i, 1);
                break;
            }
            i++;
        }

        //console.table(this.deptTags);
        /*for (const key in this.deptTags) {
            if (this.deptTags.hasOwnProperty(key)) {
                //const element = this.deptTags[key];
                
            }
        }*/
    }

    handleUserClose(tag: string) {
        var i = 0;
        for (const item of this.userTags) {
            if (item == tag) {
                this.userTags.splice(i, 1);
                break;
            }
            i++;
        }
        //console.table(this.userTags);
    }

    showDeptUserModel() {
        this.modalHelper
            .open(DeptUserComponent, {}, 'lg', {
                nzMask: true,
                nzClosable: false,
            })
            .subscribe(res => {
                if (res) {

                }
            });
    }

}

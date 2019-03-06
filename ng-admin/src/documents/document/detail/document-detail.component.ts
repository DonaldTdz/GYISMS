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
    dept = { id: '', name: '' };
    isUpdate = false;
    id: any = '';
    codeStyle = 'none';
    attachments = [];
    confirmModal: NzModalRef;
    isAllUser = '1';
    deptTags = [];
    userTags = [];

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
        this.dept.id = this.actRouter.snapshot.params['deptId'];
        this.dept.name = this.actRouter.snapshot.params['deptName'];
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
                if (res.data) {
                    this.document.id = res.data;
                    this.id = this.document.id;
                    this.qrCode.value = this.document.id;
                }
                this.isUpdate = true;
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
        this.isUpdate = true;
        this.codeStyle = 'block';
        this.isAllUser = entity.isAllUser === true ? '1' : '0';
        this.setControlVal('isAllUser', this.isAllUser);
        this.setUserDepts(entity);
    }

    setUserDepts(entity: DocumentDto) {
        this.deptTags = entity.getDepts();
        this.userTags = entity.getUsers();
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
        this.getUserDepts();
    }

    getUserDepts() {
        if (this.document.isAllUser) {
            this.document.employeeIds = '';
            this.document.employeeDes = '';
            this.document.deptIds = '';
            this.document.deptDesc = '';
        }
        else {
            let userIds = '';
            let userNames = '';
            for (let u of this.userTags) {
                userIds += u.id + ',';
                userNames += u.name + ',';
            }
            this.document.employeeIds = (userIds == '' ? '' : userIds.substr(0, userIds.length - 1));
            this.document.employeeDes = (userNames == '' ? '' : userNames.substr(0, userNames.length - 1));
            let deptIds = '';
            let deptNames = '';
            for (let u of this.deptTags) {
                deptIds += u.id + ',';
                deptNames += u.name + ',';
            }
            this.document.deptIds = (deptIds == '' ? '' : deptIds.substr(0, deptIds.length - 1));
            this.document.deptDesc = (deptNames == '' ? '' : deptNames.substr(0, deptNames.length - 1));
        }
    }

    return() {
        this.router.navigate(['app/doc/document']);
    }

    delete() {
        this.confirmModal = this.modal.confirm({
            nzContent: '确定是否删除?',
            nzOnOk: () => {
                this.documentService.delete(this.document.id)
                    .finally(() => { this.saving = false; })
                    .subscribe(res => {
                        this.notify.info('删除成功');
                        this.router.navigate(['app/doc/document']);
                    });
            }
        });
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
                    this.getAttachments();
                }
            });
    }

    deleteAttachment(itemid) {
        this.confirmModal = this.modal.confirm({
            nzContent: '确定是否删除资料文档?',
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

    handleDeptClose(tag: any) {
        var i = 0;
        for (const item of this.deptTags) {
            //console.log('item:' + item + ' tag:' + tag)
            if (item.id == tag.id) {
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

    handleUserClose(tag: any) {
        var i = 0;
        for (const item of this.userTags) {
            if (item.id == tag.id) {
                this.userTags.splice(i, 1);
                break;
            }
            i++;
        }
        //console.table(this.userTags);
    }

    showDeptUserModel() {
        this.modalHelper
            .open(DeptUserComponent, { selectedDepts: this.deptTags, selectedUsers: this.userTags }, 'lg', {
                nzMask: true,
                nzClosable: false,
            })
            .subscribe(isconfirm => {
                if (!isconfirm) {
                    this.setUserDepts(this.document);
                }
            });
    }



}

import { Component, OnInit, Injector, Output, EventEmitter, Input } from '@angular/core';
import { ModalFormComponentBase } from '@shared/component-base/modal-form-component-base';
import { AttachmentService } from '@shared/service-proxies/documents';
import { Validators, FormControl } from '@angular/forms';
import { Attachment } from '@shared/entity/documents';
import { UploadFile } from 'ng-zorro-antd';

@Component({
    selector: 'upload-file',
    templateUrl: './upload-file.component.html',
    styles: []
})
export class UploadFileComponent extends ModalFormComponentBase<Attachment> implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    @Input() attachment: Attachment = new Attachment();
    @Input() names: any[];
    postUrl: string = '/GYISMSFile/DocFilesPostsAsync';
    uploadLoading = false;

    constructor(
        injector: Injector,
        private attachmentService: AttachmentService
    ) {
        super(injector);
    }

    ngOnInit() {
        this.validateForm = this.formBuilder.group({
            name: ['', [Validators.required]],
            fileType: ['', [Validators.required]]
        });
        //this.setFormValues(this.attachment);
    }


    duplicateValidator = (control: FormControl): { [s: string]: boolean } => {
        if (!control.value) {
            return { required: true };
        } else if (this.names.indexOf(control.value) > -1) {
            return { duplicate: true, error: true };
        }
    }

    protected submitExecute(finisheCallback: Function): void {
        this.attachmentService.createOrUpdate(this.attachment)
            .finally(() => { this.saving = false; })
            .subscribe(res => {
                this.notify.info(this.l('SavedSuccessfully'), '');
                this.success(true);
            });
    }

    protected setFormValues(entity: Attachment): void {
        this.setControlVal('name', entity.name);
        this.setControlVal('fileType', entity.fileType.toString());
    }

    protected getFormValues(): void {
        this.attachment.name = this.getControlVal('name');
        this.attachment.fileType = this.getControlVal('fileType');
    }

    beforeUpload = (file: UploadFile): boolean => {
        if (this.uploadLoading) {
            this.notify.info('正在上传中');
            return false;
        }
        this.uploadLoading = true;
        return true;
    }

    handleChange = (info: { file: UploadFile }): void => {
        if (info.file.status === 'error') {
            this.notify.error('上传文件异常，请重试');
            this.uploadLoading = false;
        }
        if (info.file.status === 'done') {
            this.uploadLoading = false;
            var res = info.file.response.result;
            //console.table(info.file.response.result);
            if (res.code == 0) {
                this.notify.success('上传文件成功');
                this.attachment.name = res.data.name;
                this.attachment.fileType = this.getFileType(res.data.ext);
                this.attachment.fileSize = res.data.size;
                this.attachment.path = res.data.url;
                this.setFormValues(this.attachment);
            } else {
                this.notify.error(res.msg);
            }
        }
    }

    getFileType(ext: string): number {
        if (ext == '.pdf') {
            return 1;
        }
        if (ext == '.doc' || ext == '.docx') {
            return 2;
        }
        if (ext == '.xls' || ext == '.xlsx') {
            return 3;
        }
        return 4;
    }
}

import { Component, OnInit, Injector, Input } from '@angular/core';
import { FormComponentBase } from '@shared/component-base/form-component-base';
import { DocumentDto } from '@shared/entity/documents';
import { Validators, FormControl } from '@angular/forms';

@Component({
    selector: 'doc-detail',
    templateUrl: 'document-detail.component.html',
    styleUrls: ['document-detail.component.scss']
})
export class DocumentDetailComponent extends FormComponentBase<DocumentDto> implements OnInit {

    qrCode = {
        value: 'https://ng-alain.com/',
        background: 'white',
        backgroundAlpha: 1.0,
        foreground: 'black',
        foregroundAlpha: 1.0,
        level: 'M',
        mime: 'image/png',
        padding: 10,
        size: 220
    }

    document: DocumentDto = new DocumentDto();

    constructor(injector: Injector) {
        super(injector);
    }

    ngOnInit(): void {
        this.validateForm = this.formBuilder.group({
            name: ['', [Validators.required, Validators.maxLength(200)]],
            summary: ['', [Validators.required, Validators.maxLength(1000)]],
            content: [null],
            releaseDate: ['2018-10-19', [Validators.required]]
        });
    }

    protected submitExecute(finisheCallback: Function): void {

    }

    protected setFormValues(entity: DocumentDto): void {

    }

    protected getFormValues(): void {

    }

}

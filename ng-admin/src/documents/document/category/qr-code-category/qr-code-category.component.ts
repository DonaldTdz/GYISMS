import { Component, OnInit, Injector, Output, EventEmitter, Input } from '@angular/core';
import { ModalComponentBase } from '@shared/component-base/modal-component-base';
import { CategoryService } from '@shared/service-proxies/documents';

@Component({
    selector: 'qr-code-category',
    templateUrl: 'qr-code-category.component.html',
    styleUrls: ['qr-code-category.component.scss']
})
export class QrCodeCategoryComponent extends ModalComponentBase implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    @Input() pname: string;
    @Input() pcode: string;
    text: string;
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
    constructor(
        injector: Injector,
        private _categoryService: CategoryService
    ) {
        super(injector);
    }

    ngOnInit() {
        this.qrCode.value = ',' + this.pcode;
        this.getParentName(parseInt(this.qrCode.value));
    }

    getParentName(id: number) {
        this._categoryService.getParentName(id).subscribe((result: string) => {
            this.text = result;
        });
    }
    submit() {
        var base64 = document.getElementById('abc').getElementsByTagName('img')[0].src;
        var a = document.createElement('a');          // 创建一个a节点插入的document
        var event = new MouseEvent('click')           // 模拟鼠标click点击事件
        a.download = 'beautifulGirl'                  // 设置a节点的download属性值
        a.href = base64;                              // 将图片的src赋值给a节点的href
        a.dispatchEvent(event)
    }
}

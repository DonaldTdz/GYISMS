import { Component, OnInit, Injector, ViewChild, Output, EventEmitter } from '@angular/core';
import { AppComponentBase } from '@shared/app-component-base';
import { Router, ActivatedRoute } from '@angular/router';
import { MeetingRoomServiceProxy } from '@shared/service-proxies/meeting-management';
import { MeetingRoom, CheckBoxList } from '@shared/entity/meeting-management';
import { FormGroup, Validators, FormBuilder, AbstractControl } from '@angular/forms';
import { AppConsts } from '@shared/AppConsts';
import { UploadFile, NzModalService, NzModalRef } from 'ng-zorro-antd';
import { config } from 'rxjs';
import { Employee } from '@shared/entity/basic-data';
import { PagedResultDtoOfEmployee, EmployeeServiceProxy } from '@shared/service-proxies/basic-data';
import { MessageEmployeeModalComponent } from './message-employee-modal/message-employee-modal.component';

@Component({
    moduleId: module.id,
    selector: 'room-detail',
    templateUrl: 'room-detail.component.html',
    styleUrls: ['room-detail.component.scss']
})
export class RoomDetailComponent extends AppComponentBase implements OnInit {
    @Output() modalSelect: EventEmitter<any> = new EventEmitter<any>();
    @ViewChild('selectsEmployeeModal') selectsEmployeeModal: MessageEmployeeModalComponent;

    search: any = {};
    isConfirmLoading = false;
    successMsg = '';
    room: MeetingRoom = new MeetingRoom();
    deviceList: CheckBoxList[] = [];
    showDeviceList: CheckBoxList[] = [];
    deviceArry: string[] = [];
    validateForm: FormGroup;
    host = AppConsts.remoteServiceBaseUrl;
    id: number;
    actionUrl = this.host + '/GYISMSFile/MeetingRoomPost?fileName=room';
    roomTypes: any[] = [{ text: '固定会议室', value: 1 }, { text: '临时会议室', value: 2 }];
    layoutPatterns: any[] = [{ text: '中心模式', value: 1 }, { text: '矩阵模式', value: 2 }];
    isApproves: any[] = [{ text: '是', value: true }, { text: '否', value: false }];
    confirmModal: NzModalRef;
    isDelete = false;

    constructor(injector: Injector, private fb: FormBuilder
        , private meetingService: MeetingRoomServiceProxy
        , private actRouter: ActivatedRoute, private router: Router
        , private modal: NzModalService) {
        super(injector);
        this.id = this.actRouter.snapshot.params['id'];
    }

    ngOnInit(): void {
        this.validateForm = this.fb.group({
            name: [null, Validators.compose([Validators.required, Validators.maxLength(50)])],
            address: [null, Validators.compose([Validators.maxLength(500)])],
            buildDesc: [null, Validators.compose([Validators.maxLength(200)])],
            remark: [null, Validators.compose([Validators.maxLength(500)])],
            coverPhoto: null,
            roomType: [null, Validators.compose([Validators.required])],
            layoutPattern: [null, Validators.compose([Validators.required])],
            isApprove: [null, Validators.compose([Validators.required])],
            num: [null, Validators.compose([Validators.required, Validators.pattern(/^\+?[1-9][0-9]*$/)])],
            managerName: null,
            row: [null, [Validators.compose([Validators.pattern(/^\+?[1-9][0-9]*$/)])]],
            column: [null, [Validators.compose([Validators.pattern(/^\+?[1-9][0-9]*$/)])]],
            devices: null
        });
        this.getRoomDevices();
        this.getMeetingRoom();
        this.host = AppConsts.remoteServiceBaseUrl;
    }

    getRoomDevices() {
        this.meetingService.getRoomDevices(this.id).subscribe((result: CheckBoxList[]) => {
            this.deviceList = result;
        });
    }

    getMeetingRoom() {
        if (this.id) {
            let params: any = {};
            params.id = this.id;
            this.meetingService.getMeetingRoomById(params).subscribe((result: MeetingRoom) => {
                this.room = result;
                this.isDelete = true;
                if (result.photo) {
                    this.room.showPhoto = this.host + this.room.photo;
                }
                // if (result.devices) {
                //     this.deviceArry = this.room.devices.split(',');
                //     let i: number = 0;
                //     this.deviceList.map(v => {
                //         if (v.label == this.deviceArry[i]) {
                //             v.checked = true;
                //             if (i < this.deviceArry.length) {
                //                 i++;
                //             }
                //         }
                //     });
                // }
            });
        } else {
            //新增
            this.room.roomType = 1;
            this.room.layoutPattern = 1;
            this.room.isApprove = true;
        }
    }

    //图片上传返回
    handleChange(info: { file: UploadFile }): void {
        if (info.file.status === 'error') {
            this.notify.error('上传图片异常，请重试');
        }
        if (info.file.status === 'done') {
            this.getBase64(info.file.originFileObj, (img: any) => {
                this.room.showPhoto = img;
            });
            this.room.photo = info.file.response.result.imageName;
            // alert(this.room.photo);
            this.notify.success('上传图片完成');
        }
    }

    private getBase64(img: File, callback: (img: any) => void) {
        const reader = new FileReader();
        reader.addEventListener('load', () => callback(reader.result));
        reader.readAsDataURL(img);
    }

    save() {
        for (const i in this.validateForm.controls) {
            this.validateForm.controls[i].markAsDirty();
        }
        if (this.validateForm.valid) {
            this.isConfirmLoading = true;
            this.successMsg = '保存成功';
            this.saveRoomInfo();
        }
    }
    saveRoomInfo() {
        let selected: CheckBoxList[] = this.deviceList.filter(v => v.checked == true);
        this.room.devices = selected.map(v => {
            return v.value;
        }).join(',');
        this.meetingService.updateRoomInfo(this.room).finally(() => { this.isConfirmLoading = false; })
            .subscribe((result: MeetingRoom) => {
                this.room = result;
                if (result.photo) {
                    this.room.showPhoto = this.host + this.room.photo;
                }
                this.isDelete = true;
                this.notify.info(this.l(this.successMsg));
            });
    }
    delete(): void {
        this.confirmModal = this.modal.confirm({
            nzContent: '是否删除会议室信息?',
            nzOnOk: () => {
                this.meetingService.deleteMeetingRoom(this.room.id).subscribe(() => {
                    this.notify.info(this.l('删除成功！'));
                    this.return();
                });
            }
        });
    }

    return() {
        this.router.navigate(['app/meeting/meeting-room']);
    }

    showModal(): void {
        this.selectsEmployeeModal.show();
    }

    /**
     * 模态框返回
     */
    getSelectData = (employee?: Employee) => {
        if (employee) {
            this.room.managerName = employee.name;
            this.room.managerId = employee.id;
        }
    }
}

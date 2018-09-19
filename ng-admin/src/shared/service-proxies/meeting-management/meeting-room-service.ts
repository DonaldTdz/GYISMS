import 'rxjs/add/operator/finally';
import 'rxjs/add/operator/map';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, from as _observableFrom, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ApiResult } from '@shared/service-proxies/entity/parameter';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { NzTreeNode } from 'ng-zorro-antd';
import { GyismsHttpClient } from '@shared/service-proxies/gyisms-httpclient';
import { MeetingRoom, CheckBoxList } from '@shared/entity/meeting-management';

@Injectable()
export class MeetingRoomServiceProxy {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }
    getAll(params: any): Observable<PagedResultDtoOfMeetingRoom> {
        let url_ = "/api/services/app/MeetingRoom/GetPagedMeetingRoomsAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return PagedResultDtoOfMeetingRoom.fromJS(data);
            } else {
                return null;
            }
        });
    }
    getMeetingRoomById(params: any): Observable<MeetingRoom> {
        let url_ = "/api/services/app/MeetingRoom/GetMeetingRoomByIdAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return MeetingRoom.fromJS(data);
            } else {
                return null;
            }
        });
    }

    updateRoomInfo(room: any): Observable<MeetingRoom> {
        let url_ = "/api/services/app/MeetingRoom/CreateOrUpdateMeetingRoomAsycn";
        return this._gyhttp.post(url_, room).map(data => {
            return data;
        });
    }

    deleteMeetingRoom(id: number): Observable<any> {
        // let url_ = "/api/services/app/MeetingRoom/MeetingRoomDeleteByIdAsync";
        // return this._gyhttp.post(url_, input).map(data => {
        //     return data.result;
        // });
        let url_ = "/api/services/app/MeetingRoom/MeetingRoomDeleteByIdAsync?id=" + id;
        return this._gyhttp.delete(url_).map(data => {
            return data.result;
        });
    }

    synchronousMeetingRoomAsync(): Observable<ApiResult> {
        let url_ = "/api/services/app/MeetingRoom/SynchronousMeetingRoomAsync";
        return this._gyhttp.post(url_).map(data => {
            return data.result;
        });
    }

    GetTreesAsync(): Observable<NzTreeNode[]> {
        let url = "/api/services/app/MeetingRoom/GetTreesAsync";
        return this._gyhttp.get(url).map(data => {
            let arry = [];
            data.map(d => {
                let tree = new NzTreeNode(d);
                arry.push(tree);
            });
            return arry;
        });
    }

    /**
     * 获取会议配置
     */
    getRoomDevices(id: number): Observable<CheckBoxList[]> {
        let url_ = "/api/services/app/SystemData/GetRoomDevicesAsync?id=" + id;
        return this._gyhttp.get(url_).map(data => {
            if (data) {
                return CheckBoxList.fromJSArray(data);
            } else {
                return null;
            }
        });
    }
}

export class PagedResultDtoOfMeetingRoom implements IPagedResultDtoOfMeetingRoom {
    totalCount: number;
    items: MeetingRoom[];

    constructor(data?: IPagedResultDtoOfMeetingRoom) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalCount = data["totalCount"];
            if (data["items"] && data["items"].constructor === Array) {
                this.items = [];
                for (let item of data["items"])
                    this.items.push(MeetingRoom.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfMeetingRoom {
        let result = new PagedResultDtoOfMeetingRoom();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalCount"] = this.totalCount;
        if (this.items && this.items.constructor === Array) {
            data["items"] = [];
            for (let item of this.items)
                data["items"].push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfMeetingRoom();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfMeetingRoom {
    totalCount: number;
    items: MeetingRoom[];
}
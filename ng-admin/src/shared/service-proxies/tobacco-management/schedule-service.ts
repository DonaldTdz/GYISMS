import 'rxjs/add/operator/finally';
import 'rxjs/add/operator/map';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, from as _observableFrom, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ApiResult } from '@shared/service-proxies/entity/parameter';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { GyismsHttpClient } from '@shared/service-proxies/gyisms-httpclient';
import { Schedule, ScheduleTask, ScheduleDetail } from '@shared/entity/tobacco-management';
import { SelectGroup } from '@shared/entity/basic-data';

@Injectable()
export class ScheduleServiceProxy {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }
    getScheduleList(params: any): Observable<PagedResultDtoOfSchedule> {
        let url_ = "/api/services/app/Schedule/GetPagedSchedulesAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return PagedResultDtoOfSchedule.fromJS(data);
            } else {
                return null;
            }
        });
    }

    getPagedScheduleDetailRecordAsync(params: any): Observable<PagedResultDtoOfScheduleDetail> {
        let url_ = "/api/services/app/ScheduleDetail/GetPagedScheduleDetailRecordAsync"
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return PagedResultDtoOfScheduleDetail.fromJS(data);
            } else {
                return null;
            }
        });
    }

    getScheduleById(params: any): Observable<Schedule> {
        let url_ = "/api/services/app/Schedule/GetScheduleByIdAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return Schedule.fromJS(data);
            } else {
                return null;
            }
        });
    }

    getWeekOfMonth(id: string): Observable<SelectGroup[]> {
        let url_ = "/api/services/app/SystemData/GetWeekOfMonthAsync?id=" + id;
        return this._gyhttp.get(url_).map(data => {
            if (data) {
                return SelectGroup.fromJSArray(data);
            } else {
                return null;
            }
        });
    }

    updateScheduleInfo(input: Schedule): Observable<Schedule> {
        let url_ = "/api/services/app/Schedule/CreateOrUpdateScheduleAsycn";
        return this._gyhttp.post(url_, input).map(data => {
            return data;
        });
    }

    updateScheduleTaskInfo(input: ScheduleTask): Observable<ScheduleTask> {
        let url_ = "/api/services/app/ScheduleTask/CreateOrUpdateScheduleTaskAsycn";
        return this._gyhttp.post(url_, input).map(data => {
            return data;
        });
    }

    deleteSchedule(input: any): Observable<any> {
        let url_ = "/api/services/app/Schedule/ScheduleDeleteByIdAsync";
        return this._gyhttp.post(url_, input).map(data => {
            return data.result;
        });
    }

    sendMessageToEmployee(input: any): Observable<ApiResult> {
        let url_ = "/api/services/app/Schedule/SendMessageToEmployeeAsync";
        return this._gyhttp.post(url_, input).map(data => {
            return data.result;
        });
    }
}

export class PagedResultDtoOfSchedule implements IPagedResultDtoOfSchedule {
    totalCount: number;
    items: Schedule[];

    constructor(data?: IPagedResultDtoOfSchedule) {
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
                    this.items.push(Schedule.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfSchedule {
        let result = new PagedResultDtoOfSchedule();
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
        let result = new PagedResultDtoOfSchedule();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfSchedule {
    totalCount: number;
    items: Schedule[];
}

export class PagedResultDtoOfScheduleDetail implements IPagedResultDtoOfScheduleDetail {
    totalCount: number;
    items: ScheduleDetail[];

    constructor(data?: IPagedResultDtoOfSchedule) {
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
                    this.items.push(ScheduleDetail.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfScheduleDetail {
        let result = new PagedResultDtoOfScheduleDetail();
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
        let result = new PagedResultDtoOfScheduleDetail();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfScheduleDetail {
    totalCount: number;
    items: ScheduleDetail[];
}
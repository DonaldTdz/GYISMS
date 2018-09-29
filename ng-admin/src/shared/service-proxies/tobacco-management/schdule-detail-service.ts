import { HttpClient } from "@angular/common/http";
import { Injectable, Inject, Optional } from "@angular/core";
import { GyismsHttpClient } from "@shared/service-proxies/gyisms-httpclient";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { ScheduleSum, ScheduleDetailTask } from "@shared/entity/tobacco-management";
import { Observable } from "rxjs";

@Injectable()
export class ScheduleDetailServiceProxy {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }

    /**
     * 获取任务汇总信息（按区域、任务类型、任务名）
     * @param params 
     */
    getSheduleSum(params: any): Observable<PagedResultDtoOfSheduleSum> {
        var url = '/api/services/app/ScheduleDetail/GetSumShedule';
        return this._gyhttp.get(url, params).map(data => {
            if (data) {
                return PagedResultDtoOfSheduleSum.fromJS(data);
            } else {
                return null;
            }
        });
    }

    /**
     * 获取任务明细
     * @param params 
     */
    getSheDulDetailByTask(params: any): Observable<PagedResultDtoOfScheduleDetailTask> {
        var url = '/api/services/app/ScheduleDetail/GetPagedScheduleDetailsByOtherTable';
        return this._gyhttp.get(url, params).map(data => {
            if (data) {
                return PagedResultDtoOfScheduleDetailTask.fromJS(data);
            } else {
                return null;
            }
        });
    }


}

export class PagedResultDtoOfSheduleSum implements IPagedResultDtoOfSheduleSum {
    totalSum: number;
    completeSum: number;
    expiredSum: number
    items: ScheduleSum[];

    constructor(data?: IPagedResultDtoOfSheduleSum) {
        if (data) {
            for (var property in data) {
                if (data.hasOwnProperty(property))
                    (<any>this)[property] = (<any>data)[property];
            }
        }
    }

    init(data?: any) {
        if (data) {
            this.totalSum = data["totalSum"];
            this.completeSum = data["completeSum"];
            this.expiredSum = data["expiredSum"];
            if (data["sheduleSumDtos"] && data["sheduleSumDtos"].constructor === Array) {
                this.items = [];
                for (let item of data["sheduleSumDtos"])
                    this.items.push(ScheduleSum.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfSheduleSum {
        let result = new PagedResultDtoOfSheduleSum();
        result.init(data);
        return result;
    }

    toJSON(data?: any) {
        data = typeof data === 'object' ? data : {};
        data["totalSum"] = this.totalSum;
        data["completeSum"] = this.completeSum;
        data["expiredSum"] = this.expiredSum;
        if (this.items && this.items.constructor === Array) {
            data["sheduleSumDtos"] = [];
            for (let item of this.items)
                data["sheduleSumDtos"].push(item.toJSON());
        }
        return data;
    }

    clone() {
        const json = this.toJSON();
        let result = new PagedResultDtoOfSheduleSum();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfSheduleSum {
    totalSum: number;
    completeSum: number;
    expiredSum: number
    items: ScheduleSum[];
}

export class PagedResultDtoOfScheduleDetailTask implements IPagedResultDtoOfScheduleDetailTask {
    totalCount: number;
    items: ScheduleDetailTask[];

    constructor(data?: IPagedResultDtoOfScheduleDetailTask) {
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
                    this.items.push(ScheduleDetailTask.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfScheduleDetailTask {
        let result = new PagedResultDtoOfScheduleDetailTask();
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
        let result = new PagedResultDtoOfScheduleDetailTask();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfScheduleDetailTask {
    totalCount: number;
    items: ScheduleDetailTask[];
}
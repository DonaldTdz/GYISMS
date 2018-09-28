import { HttpClient } from "@angular/common/http";
import { GyismsHttpClient } from "@shared/service-proxies/gyisms-httpclient";
import { Inject, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { Observer, Observable } from "rxjs";
import { HomeInfo, SheduleStatis } from "@shared/entity/home";

export class HomeInfoServiceProxy {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : '';
        this._gyhttp = gyhttp;
    }

    getHomeInfo(): Observable<HomeInfo> {
        let url = '/api/services/app/ScheduleDetail/GetHomeInfo';
        return this._gyhttp.get(url).map(data => {
            if (data != null) {
                return HomeInfo.fromJS(data);
            } else {
                return null;
            }
        });
    }

    getSheduleStatisByArea(params: any): Observable<SheduleStatis[]> {
        let url = '/api/services/app/ScheduleDetail/GetSchedulByAreaTime';
        return this._gyhttp.get(url, params).map(data => {
            if (data != null) {
                return SheduleStatis.fromJSArray(data);
            } else {
                return null;
            }
        });
    }

    getSheduleStatisByMoth(params: any): Observable<SheduleStatis[]> {
        let url = '/api/services/app/ScheduleDetail/GetSchedulByMothTime';
        return this._gyhttp.get(url, params).map(data => {
            if (data != null) {
                return SheduleStatis.fromJSArray(data);
            } else {
                return null;
            }
        });
    }
}

export class PagedResultDtoOfSheduleStatis implements IPagedResultDtoOfSheduleStatis {
    totalCount: number;
    items: SheduleStatis[];

    constructor(data?: IPagedResultDtoOfSheduleStatis) {
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
                    this.items.push(SheduleStatis.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfSheduleStatis {
        let result = new PagedResultDtoOfSheduleStatis();
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
        let result = new PagedResultDtoOfSheduleStatis();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfSheduleStatis {
    totalCount: number;
    items: SheduleStatis[];
}
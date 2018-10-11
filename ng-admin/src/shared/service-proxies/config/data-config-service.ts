import { HttpClient } from "@angular/common/http";
import { GyismsHttpClient } from "@shared/service-proxies/gyisms-httpclient";
import { Inject, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { SystemData } from "@shared/entity/config/system-data";
import { Observable } from "rxjs";
import { Parameter } from "@shared/service-proxies/entity/parameter";

export class DataConfigServiceProxy {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }
    getAll(params: any): Observable<PagedResultDtoOfSystemData> {
        let url = '/api/services/app/SystemData/GetPagedSystemDatasByType';
        return this._gyhttp.get(url, params).map(data => {
            if (data) {
                return PagedResultDtoOfSystemData.fromJS(data);
            } else {
                return null;
            }
        });
    }
    getSingleConfigById(id: any): Observable<SystemData> {
        let url = '/api/services/app/SystemData/GetSystemDataById';
        return this._gyhttp.get(url, id).map(data => {
            if (data != null) {
                return SystemData.fromJS(data);
            } else {
                return null;
            }
        });
    }

    delete(params: any): Observable<any> {
        let url = '/api/services/app/SystemData/DeleteSystemDataById';
        return this._gyhttp.delete(url, params);
    }

    update(input: SystemData): Observable<SystemData> {
        let url = '/api/services/app/SystemData/CreateOrUpdateSystemDataNew';
        return this._gyhttp.post(url, input).map(data => {
            if (data != null) {
                return data;
            } else {
                return null;
            }
        })
    }
}
export class PagedResultDtoOfSystemData implements IPagedResultDtoOfSystemData {
    totalCount: number;
    items: SystemData[];

    constructor(data?: IPagedResultDtoOfSystemData) {
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
                    this.items.push(SystemData.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfSystemData {
        let result = new PagedResultDtoOfSystemData();
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
        let result = new PagedResultDtoOfSystemData();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfSystemData {
    totalCount: number;
    items: SystemData[];
}
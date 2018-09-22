import 'rxjs/add/operator/finally';
import 'rxjs/add/operator/map';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, from as _observableFrom, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ApiResult } from '@shared/service-proxies/entity/parameter';
import { Grower } from '@shared/entity/basic-data';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { NzTreeNode } from 'ng-zorro-antd';
import { GyismsHttpClient } from '@shared/service-proxies/gyisms-httpclient';

@Injectable()
export class GrowerServiceProxy {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }
    getGrowerListAsync(params: any): Observable<PagedResultDtoOfGrower> {
        let url_ = "/api/services/app/Grower/GetPagedGrowersAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return PagedResultDtoOfGrower.fromJS(data);
            } else {
                return null;
            }
        });
    }

    getGrowerListNoPageAsync(params: any): Observable<Grower[]> {
        let url_ = "/api/services/app/Grower/GetGrowersNoPageAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return Grower.fromJSArray(data);
            } else {
                return null;
            }
        });
    }

    updateGrowerInfo(room: any): Observable<Grower> {
        let url_ = "/api/services/app/Grower/CreateOrUpdateGrowerAsycn";
        return this._gyhttp.post(url_, room).map(data => {
            return data;
        });
    }

    getGrowerById(params: any): Observable<Grower> {
        let url_ = "/api/services/app/Grower/GetGrowerByIdAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return Grower.fromJS(data);
            } else {
                return null;
            }
        });
    }

    deleteGrower(input: any): Observable<any> {
        let url_ = "/api/services/app/Grower/GrowerDeleteByIdAsync";
        return this._gyhttp.post(url_, input).map(data => {
            return data.result;
        });
    }
}

export class PagedResultDtoOfGrower implements IPagedResultDtoOfGrower {
    totalCount: number;
    items: Grower[];

    constructor(data?: IPagedResultDtoOfGrower) {
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
                    this.items.push(Grower.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfGrower {
        let result = new PagedResultDtoOfGrower();
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
        let result = new PagedResultDtoOfGrower();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfGrower {
    totalCount: number;
    items: Grower[];
}
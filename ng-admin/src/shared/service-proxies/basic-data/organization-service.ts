import 'rxjs/add/operator/finally';
import 'rxjs/add/operator/map';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, from as _observableFrom, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { ApiResult } from '@shared/service-proxies/entity/parameter';
import { Organization } from '@shared/entity/basic-data';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { NzTreeNode } from 'ng-zorro-antd';
import { GyismsHttpClient } from '@shared/service-proxies/gyisms-httpclient';

@Injectable()
export class OrganizationServiceProxy {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }
    getAll(params: any): Observable<PagedResultDtoOfOrganization> {
        let url_ = "/api/services/app/Organization/GetPagedOrganizationsAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return PagedResultDtoOfOrganization.fromJS(data);
            } else {
                return null;
            }
        });
    }

    synchronousOrganizationAsync(): Observable<ApiResult> {
        let url_ = "/api/services/app/Organization/SynchronousOrganizationAsync";
        return this._gyhttp.post(url_).map(data => {
            return data.result;
        });
    }

    GetTreesAsync(): Observable<NzTreeNode[]> {
        let url = "/api/services/app/Organization/GetTreesAsync";
        return this._gyhttp.get(url).map(data => {
            let arry = [];
            data.map(d => {
                let tree = new NzTreeNode(d);
                arry.push(tree);
            });
            return arry;
        });
    }

    GetEmployeeTreesAsync(): Observable<NzTreeNode[]> {
        let url = "/api/services/app/Employee/GetTrees";
        return this._gyhttp.get(url).map(data => {
            let arry = [];
            data.map(d => {
                let tree = new NzTreeNode(d);
                arry.push(tree);
            });
            return arry;
        });
    }
}

export class PagedResultDtoOfOrganization implements IPagedResultDtoOfOrganization {
    totalCount: number;
    items: Organization[];

    constructor(data?: IPagedResultDtoOfOrganization) {
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
                    this.items.push(Organization.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfOrganization {
        let result = new PagedResultDtoOfOrganization();
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
        let result = new PagedResultDtoOfOrganization();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfOrganization {
    totalCount: number;
    items: Organization[];
}
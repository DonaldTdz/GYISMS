import 'rxjs/add/operator/finally';
import 'rxjs/add/operator/map';
import { mergeMap as _observableMergeMap, catchError as _observableCatch } from 'rxjs/operators';
import { Observable, from as _observableFrom, throwError as _observableThrow, of as _observableOf } from 'rxjs';
import { Injectable, Inject, Optional, InjectionToken } from '@angular/core';
import { HttpClient, HttpHeaders, HttpResponse, HttpResponseBase } from '@angular/common/http';

import * as moment from 'moment';
import { ApiResult, Parameter } from '@shared/service-proxies/entity/parameter';
import { Employee } from '@shared/entity/basic-data';
import { API_BASE_URL } from '@shared/service-proxies/service-proxies';
import { GyismsHttpClient } from '@shared/service-proxies/gyisms-httpclient';

@Injectable()
export class EmployeeServiceProxy {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }

    getAll(params: any): Observable<PagedResultDtoOfEmployee> {
        let url_ = "/api/services/app/Employee/GetEmployeeListByIdAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return PagedResultDtoOfEmployee.fromJS(data);
            } else {
                return null;
            }
        });
    }

    getEmployeeById(id: string): Observable<Employee> {
        let url_ = "/api/services/app/Employee/GetEmployeeByIdAsync?id=" + id;
        return this._gyhttp.get(url_).map(data => {
            if (data) {
                return Employee.fromJS(data);
            } else {
                return null;
            }
        });
    }
    // getEmployee(params: any): Observable<PagedResultDtoOfEmployee> {
    //     let url_ = "/api/services/app/Employee/GetPagedEmployeeAsync";
    //     return this._gyhttp.get(url_, params).map(data => {
    //         if (data) {
    //             return PagedResultDtoOfEmployee.fromJS(data);
    //         } else {
    //             return null;
    //         }
    //     });
    // }
    updateEmployeeArea(params: any): Observable<Employee> {
        let url_ = "/api/services/app/Employee/EditEmployeeAreaInfoAsync";
        return this._gyhttp.post(url_, params).map(data => {
            return data;
        });
    }
}

export class PagedResultDtoOfEmployee implements IPagedResultDtoOfEmployee {
    totalCount: number;
    items: Employee[];

    constructor(data?: IPagedResultDtoOfEmployee) {
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
                    this.items.push(Employee.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfEmployee {
        let result = new PagedResultDtoOfEmployee();
        result.init(data);
        return result;
    }
    static fromJSArray(dataArray: any[]): PagedResultDtoOfEmployee[] {
        let array = [];
        dataArray.forEach(result => {
            let item = new PagedResultDtoOfEmployee();
            item.init(result);
            array.push(item);
        });
        return array;
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
        let result = new PagedResultDtoOfEmployee();
        result.init(json);
        return result;
    }
}
function throwException(message: string, status: number, response: string, headers: { [key: string]: any; }, result?: any): Observable<any> {
    if (result !== null && result !== undefined)
        return _observableThrow(result);
    else
        return _observableThrow(new SwaggerException(message, status, response, headers, null));
}
export class SwaggerException extends Error {
    message: string;
    status: number;
    response: string;
    headers: { [key: string]: any; };
    result: any;

    constructor(message: string, status: number, response: string, headers: { [key: string]: any; }, result: any) {
        super();

        this.message = message;
        this.status = status;
        this.response = response;
        this.headers = headers;
        this.result = result;
    }

    protected isSwaggerException = true;

    static isSwaggerException(obj: any): obj is SwaggerException {
        return obj.isSwaggerException === true;
    }
}
function blobToText(blob: any): Observable<string> {
    return new Observable<string>((observer: any) => {
        if (!blob) {
            observer.next("");
            observer.complete();
        } else {
            let reader = new FileReader();
            reader.onload = function () {
                observer.next(this.result);
                observer.complete();
            }
            reader.readAsText(blob);
        }
    });
}
export interface IPagedResultDtoOfEmployee {
    totalCount: number;
    items: Employee[];
}
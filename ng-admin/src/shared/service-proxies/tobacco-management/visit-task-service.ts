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
import { VisitTask, TaskExamine } from '@shared/entity/tobacco-management';

@Injectable()
export class VisitTaskServiceProxy {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }
    getVisitTaskList(params: any): Observable<PagedResultDtoOfVisitTask> {
        let url_ = "/api/services/app/VisitTask/GetPagedVisitTasksAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return PagedResultDtoOfVisitTask.fromJS(data);
            } else {
                return null;
            }
        });
    }

    getTaskExamineList(params: any): Observable<PagedResultDtoOfTaskExamine> {
        let url_ = "/api/services/app/TaskExamine/GetPagedTaskExaminesAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return PagedResultDtoOfTaskExamine.fromJS(data);
            } else {
                return null;
            }
        });
    }

    getVisitTaskById(params: any): Observable<VisitTask> {
        let url_ = "/api/services/app/VisitTask/GetVisitTaskByIdAsync";
        return this._gyhttp.get(url_, params).map(data => {
            if (data) {
                return VisitTask.fromJS(data);
            } else {
                return null;
            }
        });
    }

    getTaskExamineById(id: number): Observable<TaskExamine> {
        let url_ = "/api/services/app/TaskExamine/GetTaskExamineByIdAsync?id=" + id;
        return this._gyhttp.get(url_).map(data => {
            if (data) {
                return TaskExamine.fromJS(data);
            } else {
                return null;
            }
        });
    }

    updateTaskInfo(input: any): Observable<any> {
        console.log(input);

        let url_ = "/api/services/app/VisitTask/CreateOrUpdateVisitTaskAsycn";
        return this._gyhttp.post(url_, input).map(data => {
            return data;
        });
    }

    updateTaskExamineInfo(input: any): Observable<TaskExamine> {
        let url_ = "/api/services/app/TaskExamine/CreateOrUpdateTaskExamineAsync";
        return this._gyhttp.post(url_, input).map(data => {
            return data;
        });
    }

    deleteVisitTask(input: any): Observable<any> {
        let url_ = "/api/services/app/VisitTask/VisitTaskDeleteByIdAsync";
        return this._gyhttp.post(url_, input).map(data => {
            return data.result;
        });
    }
}

export class PagedResultDtoOfVisitTask implements IPagedResultDtoOfVisitTask {
    totalCount: number;
    items: VisitTask[];

    constructor(data?: IPagedResultDtoOfVisitTask) {
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
                    this.items.push(VisitTask.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfVisitTask {
        let result = new PagedResultDtoOfVisitTask();
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
        let result = new PagedResultDtoOfVisitTask();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfVisitTask {
    totalCount: number;
    items: VisitTask[];
}

export class PagedResultDtoOfTaskExamine implements IPagedResultDtoOfTaskExamine {
    totalCount: number;
    items: TaskExamine[];

    constructor(data?: IPagedResultDtoOfTaskExamine) {
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
                    this.items.push(TaskExamine.fromJS(item));
            }
        }
    }

    static fromJS(data: any): PagedResultDtoOfTaskExamine {
        let result = new PagedResultDtoOfTaskExamine();
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
        let result = new PagedResultDtoOfTaskExamine();
        result.init(json);
        return result;
    }
}

export interface IPagedResultDtoOfTaskExamine {
    totalCount: number;
    items: TaskExamine[];
}
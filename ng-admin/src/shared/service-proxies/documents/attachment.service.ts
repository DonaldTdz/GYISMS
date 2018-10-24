import { HttpClient } from "@angular/common/http";
import { GyismsHttpClient } from "@shared/service-proxies/gyisms-httpclient";
import { Injectable, Inject, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { Observable } from "rxjs";
import { PagedResultDtoOfDocument, DocumentDto } from "@shared/entity/documents";

@Injectable()
export class AttachmentService {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }

    createOrUpdate(input: any): Observable<any> {
        let url_ = "/api/services/app/DocAttachment/CreateOrUpdate";
        let attachment = { docAttachment: input };
        return this._gyhttp.post(url_, attachment).map(data => {
            return data;
        });
    }

    getListByDocIdAsync(param: any): Observable<any> {
        let url_ = "/api/services/app/DocAttachment/getListByDocIdAsync";
        return this._gyhttp.get(url_, param).map(data => {
            return data;
        });
    }

    delete(id: any): Observable<any> {
        let url_ = "/api/services/app/DocAttachment/Delete";
        var param = { id: id };
        return this._gyhttp.delete(url_, param).map(data => {
            return data;
        });
    }

}
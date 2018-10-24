import { HttpClient } from "@angular/common/http";
import { GyismsHttpClient } from "@shared/service-proxies/gyisms-httpclient";
import { Injectable, Inject, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";
import { Observable } from "rxjs";
import { PagedResultDtoOfDocument, DocumentDto } from "@shared/entity/documents";

@Injectable()
export class DocumentService {
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
        let url_ = "/api/services/app/Document/CreateOrUpdate";
        let cat = { document: input };
        return this._gyhttp.post(url_, cat).map(data => {
            return data;
        });
    }

    getPaged(param: any): Observable<PagedResultDtoOfDocument> {
        let url_ = "/api/services/app/Document/getPaged";
        return this._gyhttp.get(url_, param).map(data => {
            return PagedResultDtoOfDocument.fromJS(data);
        });
    }

    getById(id: any): Observable<DocumentDto> {
        let url_ = "/api/services/app/Document/getById";
        return this._gyhttp.get(url_, { id: id }).map(data => {
            return DocumentDto.fromJS(data);
        });
    }
}
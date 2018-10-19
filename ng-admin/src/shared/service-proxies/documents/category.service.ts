import { HttpClient } from "@angular/common/http";
import { GyismsHttpClient } from "@shared/service-proxies/gyisms-httpclient";
import { Injectable, Inject, Optional } from "@angular/core";
import { API_BASE_URL } from "@shared/service-proxies/service-proxies";

@Injectable()
export class CategoryService {
    private http: HttpClient;
    private _gyhttp: GyismsHttpClient;
    private baseUrl: string;
    protected jsonParseReviver: ((key: string, value: any) => any) | undefined = undefined;

    constructor(@Inject(HttpClient) http: HttpClient, @Inject(GyismsHttpClient) gyhttp: GyismsHttpClient, @Optional() @Inject(API_BASE_URL) baseUrl?: string) {
        this.http = http;
        this.baseUrl = baseUrl ? baseUrl : "";
        this._gyhttp = gyhttp;
    }

}



import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module/dist/src/abpHttpInterceptor';

import * as ApiServiceProxies from '@shared/service-proxies/service-proxies';
import { OrganizationServiceProxy, EmployeeServiceProxy, GrowerServiceProxy } from '@shared/service-proxies/basic-data';
import { GyismsHttpClient } from '@shared/service-proxies/gyisms-httpclient';
import { MeetingRoomServiceProxy } from '@shared/service-proxies/meeting-management';
import { VisitTaskServiceProxy, ScheduleServiceProxy } from '@shared/service-proxies/tobacco-management';
import { DataConfigServiceProxy } from '@shared/service-proxies/config/data-config-service';
import { HomeInfoServiceProxy } from '@shared/service-proxies/home/home-service';
import { ScheduleDetailServiceProxy } from '@shared/service-proxies/tobacco-management/schdule-detail-service';
import { CategoryService, DocumentService } from '@shared/service-proxies/documents';


@NgModule({
  providers: [
    GyismsHttpClient,
    ApiServiceProxies.RoleServiceProxy,
    ApiServiceProxies.SessionServiceProxy,
    ApiServiceProxies.TenantServiceProxy,
    ApiServiceProxies.UserServiceProxy,
    ApiServiceProxies.TokenAuthServiceProxy,
    ApiServiceProxies.AccountServiceProxy,
    ApiServiceProxies.ConfigurationServiceProxy,
    OrganizationServiceProxy,
    EmployeeServiceProxy,
    MeetingRoomServiceProxy,
    GrowerServiceProxy,
    VisitTaskServiceProxy,
    ScheduleServiceProxy,
    DataConfigServiceProxy,
    HomeInfoServiceProxy,
    ScheduleDetailServiceProxy,
    CategoryService,
    DocumentService,
    { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
  ],
})
export class ServiceProxyModule { }

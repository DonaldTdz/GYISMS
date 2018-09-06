import { NgModule } from '@angular/core';
import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AbpHttpInterceptor } from 'abp-ng2-module/dist/src/abpHttpInterceptor';

import * as ApiServiceProxies from '@shared/service-proxies/service-proxies';
import { OrganizationServiceProxy } from '@shared/service-proxies/basic-data';
import { GyismsHttpClient } from '@shared/service-proxies/gyisms-httpclient';

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
    { provide: HTTP_INTERCEPTORS, useClass: AbpHttpInterceptor, multi: true },
  ],
})
export class ServiceProxyModule { }

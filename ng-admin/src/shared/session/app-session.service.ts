import { Injectable } from '@angular/core';
import {
  SessionServiceProxy,
  UserLoginInfoDto,
  TenantLoginInfoDto,
  ApplicationInfoDto,
  GetCurrentLoginInformationsOutput,
} from '@shared/service-proxies/service-proxies';
import { AbpMultiTenancyService } from 'abp-ng2-module/dist/src/multi-tenancy/abp-multi-tenancy.service';

//alain stting
import { SettingsService, MenuService } from '@delon/theme';
import { ACLService } from '@delon/acl';

@Injectable()
export class AppSessionService {
  private _user: UserLoginInfoDto;
  private _tenant: TenantLoginInfoDto;
  private _application: ApplicationInfoDto;
  private _roles: string[];

  constructor(
    private _sessionService: SessionServiceProxy,
    private _abpMultiTenancyService: AbpMultiTenancyService,
    private settingService: SettingsService,
    private aclService: ACLService,
    private menuSrv: MenuService
  ) { }

  get application(): ApplicationInfoDto {
    return this._application;
  }

  get user(): UserLoginInfoDto {
    return this._user;
  }

  get userId(): number {
    return this.user ? this.user.id : null;
  }

  get tenant(): TenantLoginInfoDto {
    return this._tenant;
  }

  get tenantId(): number {
    return this.tenant ? this.tenant.id : null;
  }

  get roles(): string[] {
    return this._roles;
  }
  getShownLoginName(): string {
    const userName = this._user.userName;
    if (!this._abpMultiTenancyService.isEnabled) {
      return userName;
    }

    return (this._tenant ? this._tenant.tenancyName : '.') + '\\' + userName;
  }

  private reMenu() {
    this.menuSrv.resume();
  }


  init(): Promise<boolean> {
    return new Promise<boolean>((resolve, reject) => {
      this._sessionService
        .getCurrentLoginInformations()
        .toPromise()
        .then(
          (result: GetCurrentLoginInformationsOutput) => {
            this._application = result.application;
            this._user = result.user;
            this._tenant = result.tenant;
            this._roles = result.roles;
            //添加Alain框架设置user
            if (this._user) {
              let user = { name: this._user.name, email: this._user.emailAddress };
              this.settingService.setUser(user);//目前abp setting和 alain setting还未整合到一起，先兼容 保留两个一起使用
            }

            //添加Alain框架根据角色设置菜单权限
            if (result.roles) {
              //alert(JSON.stringify(result.roles))
              //ACL：设置权限为全量 如果是后台管理员
              if (result.roles.includes('Admin')) {
                //alert(1);
                this.aclService.setFull(true);
              }
              else {
                //alert(3);
                //Admin MarketingCenter CustomerManager
                this.aclService.setFull(false);
                this.aclService.setRole(result.roles);
                //this.aclService.setRole(roles);
              }
              this.reMenu();
            }
            resolve(true);
          },
          err => {
            reject(err);
          },
        );
    });
  }

  changeTenantIfNeeded(tenantId?: number): boolean {
    if (this.isCurrentTenant(tenantId)) {
      return false;
    }

    abp.multiTenancy.setTenantIdCookie(tenantId);
    location.reload();
    return true;
  }

  private isCurrentTenant(tenantId?: number) {
    if (!tenantId && this.tenant) {
      return false;
    } else if (tenantId && (!this.tenant || this.tenant.id !== tenantId)) {
      return false;
    }

    return true;
  }
}

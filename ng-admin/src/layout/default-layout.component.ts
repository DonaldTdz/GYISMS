import { Component, OnInit } from '@angular/core';
import { SignalRAspNetCoreHelper } from '@shared/helpers/SignalRAspNetCoreHelper';
import { AppComponentBase } from '@shared/app-component-base';
import { Injector } from '@angular/core';
import { AfterViewInit } from '@angular/core';
import { SettingsService, TitleService, MenuService, Menu } from '@delon/theme';
import { Router } from '@angular/router';
import { NavigationEnd } from '@angular/router';
import { filter } from 'rxjs/operators';
import { HostBinding } from '@angular/core';
import { NzModalService, NzNotificationService } from 'ng-zorro-antd';
import { AppConsts } from '@shared/AppConsts';
import { MenuItem } from '@shared/layout/menu-item';
import { AppSessionService } from '@shared/session/app-session.service';

@Component({
    selector: 'default-layout',
    templateUrl: './default-layout.component.html',
    styleUrls: ['./default-layout.component.less'],
})
export class DefaultLayoutComponent extends AppComponentBase
    implements OnInit, AfterViewInit {
    @HostBinding('class.layout-fixed')
    get isFixed() {
        return this.settings.layout.fixed;
    }
    @HostBinding('class.layout-boxed')
    get isBoxed() {
        return this.settings.layout.boxed;
    }
    @HostBinding('class.aside-collapsed')
    get isCollapsed() {
        return this.settings.layout.collapsed;
    }

    // 全局的菜单
    Menums = [
        // 首页
        new MenuItem('主页', '', 'anticon anticon-home', '/app/home'),
        // 组织架构
        // new MenuItem(
        //     '组织架构管理',
        //     '',
        //     'anticon anticon-info-circle-o',
        //     '/basic/organization',
        // ),
        // 基础数据
        new MenuItem(
            '基础数据',
            '',
            'anticon anticon-dingding',
            '',
            [new MenuItem(
                '组织架构',
                '',
                '',
                '/app/basic/organization', null, null, 'CityAdmin'
            ),
            // new MenuItem(
            //     '内部员工',
            //     '',
            //     '',
            //     '/app/basic/employee',
            // ),
            new MenuItem(
                '烟农管理',
                '',
                '',
                '/app/basic/grower',
            )]
        ),

        // 烟叶管理
        new MenuItem(
            '烟叶服务',
            '',
            'anticon anticon-select',
            '',
            [new MenuItem(
                '任务管理',
                '',
                '',
                '/app/task/visit-task',
            ),
            new MenuItem(
                '走访计划',
                '',
                '',
                '/app/task/schedule',
            ),
            new MenuItem(
                '统计报表',
                '',
                '',
                '/app/task/report-form',
            ),
                // new MenuItem(
                //     '考核管理',
                //     '',
                //     '',
                //     '',
                // ),
            ]
        ),
        // 会议管理
        new MenuItem(
            '会议申请',
            '',
            'anticon anticon-laptop',
            '',
            [new MenuItem(
                '会议室管理',
                '',
                '',
                '/app/meeting/meeting-room',
            ),
            /*new MenuItem(
                '会议室预定',
                '',
                '',
                '',
            ),
            new MenuItem(
                '历史会议记录',
                '',
                '',
                '',
            ),*/
            new MenuItem(
                '会议室详情',
                '',
                '',
                '/app/meeting/room-detail',
                null,
                true
            ),
            new MenuItem(
                '会议室详情',
                '',
                '',
                '/app/meeting/room-detail/:id',
                null,
                true
            ),
            ], null, 'CityAdmin'
        ),
        new MenuItem(
            '企业标准库',
            '',
            'anticon anticon-book',
            '',
            [new MenuItem(
                '资料管理',
                '',
                '',
                '/app/doc/document',
            ),
            ], null, 'CityAdmin'
        ),
        // 配置管理
        new MenuItem(
            '配置管理',
            '',
            'anticon anticon-tool',
            '',
            [
                //     new MenuItem(
                //     '钉钉配置',
                //     '',
                //     '',
                //     '',
                // ),
                // new MenuItem(
                //     '微信配置',
                //     '',
                //     '',
                //     '',
                // ),
                new MenuItem(
                    '数据配置',
                    '',
                    '',
                    '/app/config/data-config',
                ),], null, 'CityAdmin'
        ),

        // 系统管理
        new MenuItem(
            '系统管理',
            '',
            'anticon anticon-setting',
            '',
            [
                //租户
                new MenuItem(
                    '租户管理',
                    'Pages.Tenants',
                    '',
                    '/app/tenants',
                ),
                // 角色
                new MenuItem(
                    '角色管理',
                    'Pages.Roles',
                    '',
                    '/app/roles',
                ),
                // 用户
                new MenuItem(
                    '用户管理',
                    'Pages.Users',
                    '',
                    '/app/users',
                )
            ], null, 'Admin'
        )
    ];

    constructor(
        injector: Injector,
        private settings: SettingsService,
        private router: Router,
        private titleSrv: TitleService,
        private menuService: MenuService,
        private appSessionService: AppSessionService,
    ) {
        super(injector);

        // 创建菜单
        const arrMenu = new Array<Menu>();
        this.processMenu(arrMenu, this.Menums);
        this.menuService.add(arrMenu);
        this.menuService.resume();
    }

    ngOnInit(): void {
        this.router.events
            .pipe(filter(evt => evt instanceof NavigationEnd))
            .subscribe(() => this.titleSrv.setTitle('广元烟草信息化平台'));

        // 注册通知信息
        // SignalRAspNetCoreHelper.initSignalR();
        // 触发通知事件
        // abp.event.on('abp.notifications.received', userNotification => {
        //   abp.notifications.showUiNotifyForUserNotification(userNotification);

        //   // Desktop notification
        //   Push.create('AbpZeroTemplate', {
        //     body: userNotification.notification.data.message,
        //     icon: abp.appPath + 'assets/app-logo-small.png',
        //     timeout: 6000,
        //     onClick: function () {
        //       window.focus();
        //       this.close();
        //     },
        //   });
        // });
    }

    ngAfterViewInit(): void {
        // ($ as any).AdminBSB.activateAll();
        // ($ as any).AdminBSB.activateDemo();
    }

    // 处理生成菜单
    processMenu(resMenu: Menu[], menus: MenuItem[], isChild?: boolean) {
        menus.forEach(item => {
            let subMenu: Menu;
            subMenu = {
                text: item.displayName,
                link: item.route,
                icon: `${item.icon}`,
                hide: item.hide,
                acl: item.acl,
                reuse: false
            };
            if (item.permission !== '' && !this.isGranted(item.permission)) {
                subMenu.hide = true;
            }

            if (!this.appSessionService.roles.includes('Admin') && item.acl && !this.appSessionService.roles.includes(item.acl)) {
                subMenu.hide = true;
            }

            if (item.childMenus && item.childMenus.length > 0) {
                subMenu.children = [];
                this.processMenu(subMenu.children, item.childMenus);
            }

            resMenu.push(subMenu);
        });
    }
}

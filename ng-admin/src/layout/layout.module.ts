import { HeaderLanguageswitch } from './default/header/components/languageswitch.component';
import { NgModule } from '@angular/core';

import { HeaderComponent } from './default/header/header.component';
import { SidebarComponent } from './default/sidebar/sidebar.component';
import { HeaderSearchComponent } from './default/header/components/search.component';
import { HeaderNotifyComponent } from './default/header/components/notify.component';
import { HeaderTaskComponent } from './default/header/components/task.component';
import { HeaderIconComponent } from './default/header/components/icon.component';
import { HeaderFullScreenComponent } from './default/header/components/fullscreen.component';
import { HeaderStorageComponent } from './default/header/components/storage.component';
import { HeaderUserComponent } from './default/header/components/user.component';

const COMPONENTS = [
  HeaderComponent,
  SidebarComponent,
  SideBarNavComponent,
  SideBarLogoComponent,
  SidebarUserComponent,
];

const HEADERCOMPONENTS = [
  HeaderSearchComponent,
  HeaderNotifyComponent,
  HeaderTaskComponent,
  HeaderIconComponent,
  HeaderFullScreenComponent,
  HeaderStorageComponent,
  HeaderUserComponent,
  HeaderLanguageswitch,
];

//
import { SharedModule } from '@shared/shared.module';
import { SideBarNavComponent } from './default/sidebar/components/sidebar-nav.component';
import { SideBarLogoComponent } from './default/sidebar/components/sidebar-logo.component';
import { SidebarUserComponent } from './default/sidebar/components/sidebar-user.component';
import { DefaultLayoutComponent } from './default-layout.component';

@NgModule({
  imports: [SharedModule],
  providers: [],
  declarations: [...COMPONENTS, ...HEADERCOMPONENTS, DefaultLayoutComponent],
  exports: [...COMPONENTS],
})
export class LayoutModule { }

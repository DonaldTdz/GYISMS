import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AppRoutingModule } from '@app/app-routing.module';
//import { AppComponent } from '@app/app.component';
import { AbpModule } from 'abp-ng2-module/dist/src/abp.module';
import { LocalizationService } from 'abp-ng2-module/dist/src/localization/localization.service';
//import { LayoutModule } from '@app/layout/layout.module';
import { LayoutModule } from '../layout/layout.module';
import { HomeComponent } from '@app/home/home.component';
import { SharedModule } from '@shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { BrowserModule } from '@angular/platform-browser';
import { G2GroupBarModule } from '../../packages/group-bar';
import { TenantsComponent } from '@app/tenants/tenants.component';
import { UsersComponent } from '@app/users/users.component';
import { RolesComponent } from '@app/roles/roles.component';
import { CreateTenantComponent } from '@app/tenants/create-tenant/create-tenant.component';
import { EditTenantComponent } from '@app/tenants/edit-tenant/edit-tenant.component';
import { CreateRoleComponent } from '@app/roles/create-role/create-role.component';
import { EditRoleComponent } from '@app/roles/edit-role/edit-role.component';
import { CreateUserComponent } from '@app/users/create-user/create-user.component';
import { EditUserComponent } from '@app/users/edit-user/edit-user.component';
import { ChooeseEmployeeModalComponent } from './users/chooese-employee-modal/chooese-employee-modal.component';

@NgModule({
  imports: [
    CommonModule,
    BrowserModule,
    BrowserAnimationsModule,
    HttpClientModule,
    AppRoutingModule,
    LayoutModule,
    SharedModule,
    AbpModule,
    G2GroupBarModule,
  ],
  declarations: [
    //AppComponent,
    HomeComponent,
    TenantsComponent,
    UsersComponent,
    RolesComponent,
    CreateTenantComponent,
    EditTenantComponent,
    CreateRoleComponent,
    EditRoleComponent,
    CreateUserComponent,
    EditUserComponent,
    ChooeseEmployeeModalComponent
  ],
  entryComponents: [
    CreateTenantComponent,
    EditTenantComponent,
    CreateRoleComponent,
    EditRoleComponent,
    CreateUserComponent,
    EditUserComponent,
  ],
  providers: [LocalizationService],
})
export class AppModule { }

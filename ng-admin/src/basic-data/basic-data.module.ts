import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbpModule } from 'abp-ng2-module/dist/src/abp.module';
import { LocalizationService } from 'abp-ng2-module/dist/src/localization/localization.service';
import { LayoutModule } from '../layout/layout.module';
import { SharedModule } from '@shared/shared.module';
import { HttpClientModule } from '@angular/common/http';
import { OrganizationComponent } from './organization/organization.commponent';
import { BasicRoutingModule } from './basic-data-routing.module';
import { RetailCustomerComponent } from './retail-customer/retail-customer.component';
import { EmployeeComponent } from './employee/employee.component';
import { GrowerComponent } from './grower/grower.component';
import { GrowerDetailComponent } from './grower/grower-detail/grower-detail.component';
import { MessageOrganizationModalComponent } from './grower/grower-detail/message-organization-modal/message-organization-modal.component';


@NgModule({
    imports: [
        CommonModule,
        HttpClientModule,
        BasicRoutingModule,
        LayoutModule,
        SharedModule,
        AbpModule,
    ],
    declarations: [
        OrganizationComponent,
        RetailCustomerComponent,
        EmployeeComponent,
        GrowerComponent,
        GrowerDetailComponent,
        MessageOrganizationModalComponent
    ],
    entryComponents: [
    ],
    providers: [LocalizationService],
})
export class BasicDataModule { }

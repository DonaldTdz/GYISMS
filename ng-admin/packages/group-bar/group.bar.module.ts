import { NgModule, ModuleWithProviders } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DelonUtilModule } from '@delon/util';
import { G2GroupBarComponent } from './group-bar.component';

const COMPONENTS = [G2GroupBarComponent];

@NgModule({
    imports: [CommonModule, DelonUtilModule],
    declarations: [...COMPONENTS],
    exports: [...COMPONENTS],
})
export class G2GroupBarModule {
    static forRoot(): ModuleWithProviders {
        return { ngModule: G2GroupBarModule, providers: [] };
    }
}

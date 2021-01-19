import { NgModule } from '@angular/core';
import { SharedModule } from '../shared/shared.module';
import { ModulesRoutingModule } from './modules-routing.module';

@NgModule({
    imports: [
        SharedModule,
        ModulesRoutingModule
    ]
})
export class ModulesModule { }

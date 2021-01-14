import { NgModule } from '@angular/core';
import { from } from 'rxjs';
import { SharedModule } from 'src/app/shared/shared.module';
import { AccountRoutingModule, ACCOUNT_ROUTED_COMCOMPONENTS } from './account-routing.module';
import { NzCardModule } from 'ng-zorro-antd/card';
import { NzFormModule } from 'ng-zorro-antd/form';

@NgModule({
  imports: [
    SharedModule,
    NzCardModule,
    NzFormModule,
    AccountRoutingModule
  ],
  declarations: [
    ...ACCOUNT_ROUTED_COMCOMPONENTS
  ]
})
export class AccountModule { }

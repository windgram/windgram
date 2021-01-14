import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { HomeRoutingModule, HOME_ROUTED_COMCOMPONENTS } from './home-routing.module';

@NgModule({
  imports: [
    SharedModule,
    HomeRoutingModule
  ],
  declarations: [
    ...HOME_ROUTED_COMCOMPONENTS
  ]
})
export class HomeModule { }

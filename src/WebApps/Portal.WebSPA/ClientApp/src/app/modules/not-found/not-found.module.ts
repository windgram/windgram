import { NgModule } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { NotFoundRoutingModule, NOT_FOUND_ROUTED_COMCOMPONENTS } from './not-found-routing.module';

@NgModule({
  imports: [
    SharedModule,
    NotFoundRoutingModule
  ],
  declarations: [
    NOT_FOUND_ROUTED_COMCOMPONENTS
  ]
})
export class NotFoundModule { }

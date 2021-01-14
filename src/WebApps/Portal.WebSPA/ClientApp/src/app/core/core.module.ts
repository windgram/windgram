import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthGuard } from './auth-guard';
import { StorageService } from './services/storage.service';
import { UserService } from './services/user.service';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [
    AuthGuard,
    StorageService,
    UserService
  ]
})
export class CoreModule { }

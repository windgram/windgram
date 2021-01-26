import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuthGuard } from './auth-guard';
import { StorageService } from './services/storage.service';
import { ProfileService } from './services/profile.service';
import { AuthService } from './services/auth.service';
import { JsLoaderService } from './services/js-loader.service';

@NgModule({
  imports: [
    CommonModule
  ],
  providers: [
    AuthGuard,
    AuthService,
    StorageService,
    ProfileService,
    JsLoaderService
  ]
})
export class CoreModule { }

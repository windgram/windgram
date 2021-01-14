import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { StorageService } from 'src/app/core/services/storage.service';
import { environment } from 'src/environments/environment';
import { LoginModel, RegisterModel } from './account.model';

const ACCOUNT_URL = `${environment.stsAuthority}/api/account`;
@Injectable({
  providedIn: 'root'
})
export class AccountService {
  constructor(
    private httpClient: HttpClient,
    private storageService: StorageService
  ) { }

  login(model: LoginModel) {
    return this.httpClient.post<void>(`${ACCOUNT_URL}/login`, model);
  }

  register(model: RegisterModel) {
    return this.httpClient.post<void>(`${ACCOUNT_URL}/register`, model);
  }

  logout() {
    this.storageService.clear();
    return this.httpClient.post<void>(`${ACCOUNT_URL}/logout`, null);
  }

}

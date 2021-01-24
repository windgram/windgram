import { Injectable } from '@angular/core';
import { User, UserManager, UserManagerSettings } from 'oidc-client';
import { environment } from 'src/environments/environment';

@Injectable()
export class AuthService {
  userStoreKeys = `oidc.user:${environment.stsAuthority}:${environment.clientId}`;
  userManager: UserManager;

  constructor() {
    const settings: UserManagerSettings = {
      authority: environment.stsAuthority,
      client_id: environment.clientId,
      redirect_uri: `${environment.clientRoot}/callback/signin`,
      silent_redirect_uri: `${environment.clientRoot}/callback/silent`,
      post_logout_redirect_uri: `${environment.clientRoot}/callback/logout`,
      response_type: 'id_token token',
      scope: environment.clientScope,
      automaticSilentRenew: true,
    };
    this.userManager = new UserManager(settings);
  }

  getUser(): Promise<User> {
    return this.userManager.getUser();
  }

  login(): Promise<void> {
    return this.userManager.signinRedirect();
  }

  loginCallBack(): Promise<User> {
    return this.userManager.signinRedirectCallback();
  }

  renewToken(): Promise<User> {
    return this.userManager.signinSilent();
  }

  renewTokenCallback(): Promise<User> {
    return this.userManager.signinSilentCallback();
  }

  logout(): Promise<void> {
    return this.userManager.signoutRedirect();
  }

  goProfile() {
    location.href = `${environment.stsAuthority}/manage/profile`;
  }
}

import { Injectable } from '@angular/core';
import { ActivatedRouteSnapshot, CanActivate, Router, RouterStateSnapshot } from '@angular/router';
import { AuthService } from './services/auth.service';

@Injectable()
export class AuthGuard implements CanActivate {
  static user: any;
  constructor(
    private router: Router,
    private authService: AuthService,
  ) { }

  canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot) {
    return this.authService.getUser()
      .then(() => {
        return true;
      }).catch(() => {
        this.router.navigate(['/account/login'], { queryParams: { returnUrl: state.url } });
        return false;
      });
  }
}

import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserProfileViewModel } from 'src/app/core/models/user-profile.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-layout-default-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class LayoutDefaultHeaderComponent implements OnInit {
  profile: UserProfileViewModel;
  constructor(
    private router: Router,
    private authService: AuthService,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.userService.userLoaded.subscribe(user => {
      this.profile = user;
    });
    this.userService.loadUser();
  }

  login() {
    this.authService.login()
      .then(() => {

      });
  }

  logout() {
    this.authService.logout()
      .then(() => {

      });
  }
}

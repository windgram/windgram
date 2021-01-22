import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserService } from 'src/app/core/services/user.service';

@Component({
  selector: 'app-layout-default-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class LayoutDefaultHeaderComponent implements OnInit {
  profile: any;
  constructor(
    private router: Router,
    private authService: AuthService,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.load();
  }

  load() {
    this.userService.getUser()
      .subscribe(res => {
        this.profile = res;
        console.log(res);
      });
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

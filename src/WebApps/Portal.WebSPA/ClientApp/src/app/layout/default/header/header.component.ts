import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { UserProfileViewModel } from 'src/app/core/models/user-profile.model';
import { UserService } from 'src/app/core/services/user.service';
import { AccountService } from 'src/app/modules/account/shared/account.service';

@Component({
  selector: 'app-layout-default-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class LayoutDefaultHeaderComponent implements OnInit {
  profile: UserProfileViewModel;
  constructor(
    private router: Router,
    private accountService: AccountService,
    private userService: UserService
  ) { }

  ngOnInit() {
    this.userService.userLoaded.subscribe(user => {
      this.profile = user;
    });
    this.userService.loadUser();
  }

  logout() {
    this.accountService.logout()
      .subscribe(() => {
        location.href = '/';
      });
  }
}

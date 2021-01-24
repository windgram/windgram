import { Component, OnInit } from '@angular/core';
import { UserProfileViewModel } from 'src/app/core/models/profile.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { ProfileService } from 'src/app/core/services/profile.service';

@Component({
  selector: 'app-layout-default-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class LayoutDefaultHeaderComponent implements OnInit {
  user: UserProfileViewModel;
  constructor(
    private authService: AuthService,
    private profileService: ProfileService
  ) { }

  ngOnInit() {
    this.load();
  }

  load() {
    this.profileService.get()
      .subscribe(res => {
        this.user = res;
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

  goProfile() {
    this.authService.goProfile();
  }
}

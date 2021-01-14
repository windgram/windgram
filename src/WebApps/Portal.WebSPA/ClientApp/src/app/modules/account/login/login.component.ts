import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { UserService } from 'src/app/core/services/user.service';
import { AccountService } from '../shared/account.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  posting = false;
  form: FormGroup;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private accountService: AccountService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    this.form = this.fb.group({
      username: this.fb.control('', [Validators.required, Validators.email, Validators.maxLength(32)]),
      password: this.fb.control('', [Validators.required, Validators.minLength(8), Validators.maxLength(16)])
    });
  }

  onSubmit() {
    if (this.form.invalid || this.posting) {
      return;
    }
    this.posting = true;
    this.accountService.login(this.form.value)
      .toPromise()
      .then(res => {
        this.authService.login()
          .then(user => {
            console.log(user);
          });
      })
      .finally(() => {
        this.posting = false;
      });
  }

}

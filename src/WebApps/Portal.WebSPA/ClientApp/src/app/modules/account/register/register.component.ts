import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { AccountService } from '../shared/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {
  posting = false;
  form: FormGroup;
  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private fb: FormBuilder,
    private accountService: AccountService
  ) { }

  ngOnInit() {
    this.buildForm();
  }

  private buildForm() {
    this.form = this.fb.group({
      email: this.fb.control('', [Validators.required, Validators.email, Validators.maxLength(32)]),
      password: this.fb.control('', [Validators.required, Validators.minLength(8), Validators.maxLength(16)]),
      confirmPassword: this.fb.control('', [Validators.required, Validators.minLength(8), Validators.maxLength(16)]),
    });
  }

  onSubmit() {
    if (this.form.invalid || this.posting) {
      return;
    }
    this.posting = true;
    this.accountService.register(this.form.value)
      .toPromise()
      .then(res => {

      })
      .finally(() => {
        this.posting = false;
      });
  }
}

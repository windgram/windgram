import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent implements OnInit {
  type: string;
  constructor(
    private authService: AuthService,
    private route: ActivatedRoute,
    private router: Router
  ) {
    this.route.paramMap.subscribe(p => {
      if (p.has('type')) {
        this.type = p.get('type');
      }
    });
  }

  ngOnInit() {
    this.handleCallback();
  }

  private handleCallback() {
    switch (this.type) {
      case 'signin': {
        this.authService.loginCallBack()
          .then(user => {
            console.log(user);
            this.router.navigate(['/']);
          });
        break;
      }
      case 'silent': {
        this.authService.renewTokenCallback()
          .then(user => {
            console.log('Silent renew successful!');
          });
        break;
      }
    }
  }
}

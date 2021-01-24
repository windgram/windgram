import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { UserProfileViewModel } from '../models/profile.model';

const USER_URL = `${environment.userUrl}/api/profile`;
@Injectable()
export class ProfileService {

  constructor(
    private httpClient: HttpClient
  ) { }

  get() {
    return this.httpClient.get<UserProfileViewModel>(USER_URL);
  }
}

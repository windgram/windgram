import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Subject } from 'rxjs';
import { environment } from 'src/environments/environment';
import { UserProfileViewModel, UserProfileDto } from '../models/user-profile.model';
import { StorageService } from './storage.service';

const USER_URL = `${environment.stsAuthority}/api/user`;
@Injectable()
export class UserService {
  private userLoadedSource = new Subject<UserProfileViewModel>();
  public userLoaded = this.userLoadedSource.asObservable();
  constructor(
    private httpClient: HttpClient,
    private storageService: StorageService
  ) { }

  loadUser() {
    this.getUser()
      .subscribe(user => {
        this.userLoadedSource.next(user);
      });
  }

  getUser() {
    return this.httpClient.get<UserProfileViewModel>(USER_URL);
  }

  uploadPicture(file: File) {
    const fd = new FormData();
    fd.append('picture', file);
    return this.httpClient.post<void>(`${USER_URL}/picture`, fd);
  }

  updateProfile(dto: UserProfileDto) {
    return this.httpClient.post<void>(`${USER_URL}/profile`, dto);
  }
}

export interface UserProfileViewModel {
  id: string;
  userName: string;
  email: string;
  phoneNumber: string;
  createdDateTime: string;
  profile: UserClaimsViewModel;
}

export interface UserClaimsViewModel {
  nickname: string;
  picture: string;
  birthdate: string;
  gender: string;
  zoneinfo: string;
  bio: string;
}

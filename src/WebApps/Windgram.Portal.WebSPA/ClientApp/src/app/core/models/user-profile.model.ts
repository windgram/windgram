export interface UserProfileViewModel {
  sub: string;
  email: string;
  name: string;
  nickname: string;
  picture: string;
  gender: string;
  birthDate: string;
  zoneinfo: string;
  updated_at: string;
}

export interface UserProfileDto {
  nickName: string;
  gender: string;
  birthDate: string;
  bio: string;
  location: string;
}

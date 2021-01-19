export interface UserProfileViewModel {
  userName: string;
  email: string;
  picture: string;
  nickName: string;
  gender: string;
  birthDate: string;
  bio: string;
  location: string;
}

export interface UserProfileDto {
  nickName: string;
  gender: string;
  birthDate: string;
  bio: string;
  location: string;
}

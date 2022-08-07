export interface UserRegisterModel {
  email: string;
  userName: string;
  password: string;
  confirmPassword: string;
}

export interface UserLoginModel {
  email: string;
  password: string;
}

export interface UserLoginResponseModel {
  token: string;
}

export interface UserForgotPasswordModel {
  email: string;
  clientUri: string;
}

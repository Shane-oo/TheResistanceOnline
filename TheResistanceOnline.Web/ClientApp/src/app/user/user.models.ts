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

export interface LoginResponseModel {
  token: string;
}

export interface UserForgotPasswordModel {
  email: string;
  clientUri: string;
}


export interface UserResetPasswordModel {
  password: string;
  confirmPassword: string;
  email: string;
  token: string;
}

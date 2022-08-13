export interface UserRegisterModel {
  email: string;
  userName: string;
  password: string;
  confirmPassword: string;
  clientUri: string;
}

export interface UserLoginModel {
  email: string;
  password: string;
  clientUri: string;
}

export interface LoginResponseModel {
  token: string;
  userId: string; //Guid???
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

export interface UserConfirmEmailModel {
  email: string;
  token: string;
}

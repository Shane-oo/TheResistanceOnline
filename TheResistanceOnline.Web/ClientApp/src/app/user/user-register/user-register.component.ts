import { Component, OnInit } from '@angular/core';
import { UserRegisterModel } from '../user.models';
import { AuthenticationService } from '../authentication.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
             selector: 'app-user-register',
             templateUrl: './user-register.component.html',
             styleUrls: ['./user-register.component.css']
           })
export class UserRegisterComponent implements OnInit {
  public registerForm: FormGroup = new FormGroup({});

  constructor(private authService: AuthenticationService) {
  }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
                                        userName: new FormControl('', [Validators.required]),
                                        email: new FormControl('', [Validators.required, Validators.email]),
                                        password: new FormControl('', [Validators.required]),
                                        confirmPassword: new FormControl('', [Validators.required])
                                      });
  }

  public validateControl = (controlName: string) => {
    return this.registerForm.get(controlName)?.invalid && this.registerForm.get(controlName)?.touched;
  };

  public hasError = (controlName: string, errorName: string) => {
    return this.registerForm.get(controlName)?.hasError(errorName);

  };
  //TODO what is type
  public registerUser = (registerFormValue: any) => {
    const formValues = {...registerFormValue};
    const user: UserRegisterModel = {
      username: formValues.userName,
      email: formValues.email,
      password: formValues.password,
      confirmPassword: formValues.confirmPassword

    };
    this.authService.registerUser(user).subscribe({
                                                    next: (_) => console.log('Successful registration'),
                                                    error: (err: HttpErrorResponse) => console.log(err)
                                                  });
  };

}

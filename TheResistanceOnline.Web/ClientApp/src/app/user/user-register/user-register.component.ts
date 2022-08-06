import { Component, OnInit } from '@angular/core';
import { UserRegisterModel } from '../user.models';
import { AuthenticationService } from '../authentication.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { PasswordValidatorService } from '../../shared/custom-validators/user/password-validator.service';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';

@Component({
             selector: 'app-user-register',
             templateUrl: './user-register.component.html',
             styleUrls: ['./user-register.component.css']
           })
export class UserRegisterComponent implements OnInit {
  public registerForm: FormGroup = new FormGroup({});

  constructor(private authService: AuthenticationService, private passwordValidator: PasswordValidatorService,
              private swalService: SwalContainerService) {
  }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
                                        userName: new FormControl('', [Validators.required]),
                                        email: new FormControl('', [Validators.required, Validators.email]),
                                        password: new FormControl('', [Validators.required]),
                                        confirmPassword: new FormControl('')
                                      });
    // Custom Validators
    this.registerForm.get('confirmPassword')?.setValidators([Validators.required,
                                                              this.passwordValidator.validateConfirmPassword(this.registerForm.get(
                                                                'password'))]);

  }

  public validateControl = (controlName: string) => {
    return this.registerForm.get(controlName)?.invalid && this.registerForm.get(controlName)?.touched;
  };

  public hasError = (controlName: string, errorName: string) => {
    return this.registerForm.get(controlName)?.hasError(errorName);

  };

  public registerUser = (registerFormValue: UserRegisterModel) => {
    console.log(typeof (registerFormValue));
    const formValues = {...registerFormValue};
    const user: UserRegisterModel = {
      userName: formValues.userName,
      email: formValues.email,
      password: formValues.password,
      confirmPassword: formValues.confirmPassword
    };
    this.authService.registerUser(user).subscribe({
                                                    next: (_) => {
                                                      this.swalService.showSwal(
                                                        'Successfully registered! An email has been sent to '
                                                        + user.email
                                                        + ' please follow the link provided to confirm email address.',
                                                        SwalTypesModel.Success);
                                                    },
                                                    error: (err: HttpErrorResponse) => {
                                                    }
                                                  });
  };

}

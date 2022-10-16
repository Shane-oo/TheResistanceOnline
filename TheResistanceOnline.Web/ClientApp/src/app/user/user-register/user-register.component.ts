import { Component, OnInit } from '@angular/core';
import { UserRegisterModel } from '../user.models';
import { AuthenticationService } from '../authentication.service';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { HttpErrorResponse } from '@angular/common/http';
import { PasswordValidatorService } from '../../shared/custom-validators/user/password-validator.service';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';
import { Router } from '@angular/router';
import { environment } from '../../../environments/environment';

@Component({
             selector: 'app-user-register',
             templateUrl: './user-register.component.html',
             styleUrls: ['./user-register.component.css']
           })
export class UserRegisterComponent implements OnInit {
  public registerForm: FormGroup = new FormGroup({});

  constructor(private authService: AuthenticationService, private passwordValidator: PasswordValidatorService,
              private swalService: SwalContainerService, private router: Router) {
  }

  ngOnInit(): void {
    this.registerForm = new FormGroup({
                                        userName: new FormControl('', [Validators.required, Validators.maxLength(30)]),
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
    const formValues = {...registerFormValue};
    const user: UserRegisterModel = {
      userName: formValues.userName,
      email: formValues.email,
      password: formValues.password,
      confirmPassword: formValues.confirmPassword,
      clientUri: `${environment.Base_URL}/user/email-confirmation`
    };

    this.authService.registerUser(user).subscribe({
                                                    next: (_) => {
                                                      this.router.navigate([`/user/login`]).then(r => {
                                                        this.swalService.showSwal(
                                                          'registered! an email confirmation has been sent to you address',
                                                          SwalTypesModel.Success);
                                                      });

                                                    },
                                                    error: (err: HttpErrorResponse) => {
                                                    }
                                                  });
  };

}

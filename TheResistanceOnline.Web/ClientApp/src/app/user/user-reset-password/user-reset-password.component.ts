import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { PasswordValidatorService } from '../../shared/custom-validators/user/password-validator.service';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserResetPasswordModel } from '../user.models';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
             selector: 'app-user-reset-password',
             templateUrl: './user-reset-password.component.html',
             styleUrls: ['./user-reset-password.component.css']
           })
export class UserResetPasswordComponent implements OnInit {
  public resetPasswordForm: FormGroup = new FormGroup({});
  private token: string = '';
  private email: string = '';

  constructor(private authService: AuthenticationService, private passwordValidator: PasswordValidatorService,
              private swalService: SwalContainerService, private route: ActivatedRoute, private router: Router) {
  }

  ngOnInit(): void {
    this.resetPasswordForm = new FormGroup({
                                             password: new FormControl('', [Validators.required]),
                                             confirmPassword: new FormControl('')
                                           });
    // Custom Validators
    this.resetPasswordForm.get('confirmPassword')?.setValidators([Validators.required,
                                                                   this.passwordValidator.validateConfirmPassword(this.resetPasswordForm.get(
                                                                     'password'))]);
    // extract the email and token that came from the route
    this.token = this.route.snapshot.queryParams['token'];
    this.email = this.route.snapshot.queryParams['email'];
  }

  public validateControl = (controlName: string) => {
    return this.resetPasswordForm.get(controlName)?.invalid && this.resetPasswordForm.get(controlName)?.touched;
  };

  public hasError = (controlName: string, errorName: string) => {
    return this.resetPasswordForm.get(controlName)?.hasError(errorName);
  };

  public resetPassword = (resetPasswordFormValue: UserResetPasswordModel) => {
    const formValues = {...resetPasswordFormValue};
    const userResetPassword: UserResetPasswordModel = {
      email: this.email,
      token: this.token,
      password: formValues.password,
      confirmPassword: formValues.confirmPassword
    };


    this.authService.resetPassword(userResetPassword).subscribe({
                                                                  next: (_) => {
                                                                    this.router.navigate([`/user/login`]).then(r => {
                                                                      this.swalService.showSwal(
                                                                        'Reset Password Successful!',
                                                                        SwalTypesModel.Success);
                                                                    });

                                                                  },
                                                                  error: (err: HttpErrorResponse) => {
                                                                  }
                                                                });
  };
}


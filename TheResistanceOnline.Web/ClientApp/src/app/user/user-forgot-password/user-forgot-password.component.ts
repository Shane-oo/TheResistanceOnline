import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';
import { UserForgotPasswordModel } from '../user.models';
import { environment } from '../../../environments/environment';
import { HttpErrorResponse } from '@angular/common/http';

@Component({
             selector: 'app-user-forgot-password',
             templateUrl: './user-forgot-password.component.html',
             styleUrls: ['./user-forgot-password.component.css']
           })
export class UserForgotPasswordComponent implements OnInit {
  public forgotPasswordForm: FormGroup = new FormGroup({});

  constructor(private authService: AuthenticationService, private swalService: SwalContainerService) {
  }

  ngOnInit(): void {
    this.forgotPasswordForm = new FormGroup({
                                              email: new FormControl('', [Validators.required, Validators.email])
                                            });
  }

  public validateControl = (controlName: string) => {
    return this.forgotPasswordForm.get(controlName)?.invalid && this.forgotPasswordForm.get(controlName)?.touched;
  };

  public hasError = (controlName: string, errorName: string) => {
    return this.forgotPasswordForm.get(controlName)?.hasError(errorName);
  };

  public sendForgotPassword = (forgotPasswordFormValue: UserForgotPasswordModel) => {
    const forgotPassword = {...forgotPasswordFormValue};

    const forgotPasswordDto: UserForgotPasswordModel = {
      email: forgotPassword.email,
      clientUri: `${environment.Base_URL}/user/reset-password`
    };

    this.authService.sendUserForgotPassword(forgotPasswordDto).subscribe({
                                                                           next: (_) => {

                                                                             this.swalService.showSwal(
                                                                               'Reset Password link has been sent',
                                                                               SwalTypesModel.Success);
                                                                           },
                                                                           error: (err: HttpErrorResponse) => {
                                                                           }
                                                                         });
  };

}

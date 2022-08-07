import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserLoginModel, UserLoginResponseModel } from '../user.models';
import { HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../environments/environment';

@Component({
             selector: 'app-user-login',
             templateUrl: './user-login.component.html',
             styleUrls: ['./user-login.component.css']
           })
export class UserLoginComponent implements OnInit {
  public loginForm: FormGroup = new FormGroup({});
  private returnUrl: string = '';

  constructor(private authService: AuthenticationService, private swalService: SwalContainerService, private router: Router, private route: ActivatedRoute) {
  }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
                                     email: new FormControl('', [Validators.required, Validators.email]),
                                     password: new FormControl('', [Validators.required])

                                   });
    // this.returnUrl = this.router.snapshot.queryParams['returnUrl'] || '/';
  }

  validateControl = (controlName: string) => {
    return this.loginForm.get(controlName)?.invalid && this.loginForm.get(controlName)?.touched;
  };

  hasError = (controlName: string, errorName: string) => {
    return this.loginForm.get(controlName)?.hasError(errorName);
  };

  loginUser = (loginFormValue: UserLoginModel) => {
    const login = {...loginFormValue};

    const user: UserLoginModel = {
      email: login.email,
      password: login.password
    };

    this.authService.loginUser(user).subscribe({
                                                 next: (response:UserLoginResponseModel) => {
                                                   console.log("In here");
                                                   localStorage.setItem('TheResistanceToken', response.token);
                                                   this.authService.sendAuthStateChange(true);
                                                   // Route to homepage
                                                   this.router.navigate([`/`]).then(r => {
                                                     this.swalService.showSwal(
                                                       'Successfully Login',
                                                       SwalTypesModel.Success);
                                                   });
                                                 },
                                                 error: (err: HttpErrorResponse) => {
                                                   console.log(err);
                                                 }
                                               });
  };

}

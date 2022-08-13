import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { AuthenticationService } from '../authentication.service';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';
import { ActivatedRoute, Router } from '@angular/router';
import { LoginResponseModel, UserLoginModel } from '../user.models';
import { HttpErrorResponse } from '@angular/common/http';
import { environment } from '../../../environments/environment';
import { SessionStorageService } from '../../shared/services/session-storage.service';

@Component({
             selector: 'app-user-login',
             templateUrl: './user-login.component.html',
             styleUrls: ['./user-login.component.css']
           })
export class UserLoginComponent implements OnInit {
  public loginForm: FormGroup = new FormGroup({});
  private returnUrl: string = '';

  constructor(private authService: AuthenticationService, private swalService: SwalContainerService, private router: Router, private route: ActivatedRoute,private sessionService:SessionStorageService) {
  }

  ngOnInit(): void {
    this.loginForm = new FormGroup({
                                     email: new FormControl('', [Validators.required, Validators.email]),
                                     password: new FormControl('', [Validators.required])

                                   });
    this.returnUrl = this.route.snapshot.queryParams['returnUrl'] || '/';
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
      password: login.password ,
      clientUri: `${environment.Frontend_URL}/user/forgot-password`
    };

    this.authService.loginUser(user).subscribe({
                                                 next: (response: LoginResponseModel) => {
                                                   localStorage.setItem('TheResistanceToken', response.token);
                                                   this.sessionService.saveUserId(response.userId);
                                                   this.authService.sendAuthStateChange(true);
                                                   // Route to redirect url or homepage
                                                   this.router.navigate([this.returnUrl]).then(r => {

                                                   });
                                                 },
                                                 error: (err: HttpErrorResponse) => {
                                                   console.log(err);
                                                 }
                                               });
  };

}

import { Component, OnInit } from '@angular/core';
import { HttpErrorResponse } from '@angular/common/http';
import { AuthenticationService } from '../authentication.service';
import { ActivatedRoute, Router } from '@angular/router';
import { UserConfirmEmailModel } from '../user.models';
import { SwalContainerService, SwalTypesModel } from '../../../ui/swal/swal-container.service';

@Component({
             selector: 'app-user-email-confirmation',
             templateUrl: './user-email-confirmation.component.html',
             styleUrls: ['./user-email-confirmation.component.css']
           })
export class UserEmailConfirmationComponent implements OnInit {

  constructor(private authService: AuthenticationService, private route: ActivatedRoute, private swalService: SwalContainerService, private router: Router) {
  }

  ngOnInit(): void {
    this.confirmEmail();
  }

  private confirmEmail = () => {
    const token = this.route.snapshot.queryParams['token'];
    const email = this.route.snapshot.queryParams['email'];
    const userConfirmEmail: UserConfirmEmailModel = {
      email: email,
      token: token
    };
    this.authService.confirmEmail(userConfirmEmail).subscribe({
                                                                next: (_) => {
                                                                  this.router.navigate([`/user/login`]).then(r => {
                                                                    this.swalService.showSwal(
                                                                      'Email Confirmed!',
                                                                      SwalTypesModel.Success);
                                                                  });

                                                                },
                                                                error: (err: HttpErrorResponse) => {
                                                                }
                                                              });
  }
  };

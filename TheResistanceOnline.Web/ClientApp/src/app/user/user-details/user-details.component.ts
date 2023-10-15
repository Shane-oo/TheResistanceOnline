import {Component, OnInit} from '@angular/core';
import {Subject, takeUntil} from "rxjs";

import {UpdateUserCommand, UserDetailsModel} from "../user.models";
import {UserService} from "../user.service";
import {SwalContainerService, SwalTypes} from "../../../ui/swal/swal-container.service";
import {StateService} from "../../shared/services/state/state.service";

@Component({
  selector: 'app-user-details',
  templateUrl: './user-details.component.html',
  styleUrls: ['./user-details.component.css']
})
export class UserDetailsComponent implements OnInit {
  public userDetails: UserDetailsModel = {
    createdOn: "",
    userName: "",
    modifiedOn: null,
  };
  private readonly destroyed = new Subject<void>();

  constructor(private userService: UserService, private stateService: StateService, private swalService: SwalContainerService) {

  }

  ngOnInit() {
    this.userService.getUserDetails()
      .pipe(takeUntil(this.destroyed))
      .subscribe({
        next: (response: UserDetailsModel) => {
          this.userDetails = response;
        }
      });
  }

  ngOnDestroy() {
    this.destroyed.next();
    this.destroyed.complete();
  }

  updateUser(command: UpdateUserCommand) {
    this.userService.updateUser(command)
      .pipe(takeUntil(this.destroyed))
      .subscribe({
        next: (response: UserDetailsModel) => {
          this.userDetails = response;
          this.stateService.userName = this.userDetails.userName;
          this.swalService.showSwal(SwalTypes.Success);
        }
      });
  }

}

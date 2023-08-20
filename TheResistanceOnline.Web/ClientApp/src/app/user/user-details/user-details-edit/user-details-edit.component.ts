import {Component, EventEmitter, Inject, Input, LOCALE_ID, Output} from '@angular/core';
import {FormControl, FormGroup, Validators} from "@angular/forms";
import {formatDate} from "@angular/common";

import {ConstantsModels} from "../../../shared/models/constants.models";
import {UpdateUserCommand, UserDetailsEditFormModel, UserDetailsModel} from "../../user.models";

@Component({
  selector: 'app-user-details-edit',
  templateUrl: './user-details-edit.component.html',
  styleUrls: ['./user-details-edit.component.css']
})
export class UserDetailsEditComponent {
  public userDetailsEditForm: FormGroup<UserDetailsEditFormModel>;
  @Output() updateUserEvent = new EventEmitter<UpdateUserCommand>();

  constructor(@Inject(LOCALE_ID) private locale: string) {
    this.userDetailsEditForm = new FormGroup<UserDetailsEditFormModel>({
      userName: new FormControl('', {
        nonNullable: true,
        validators: [Validators.required,
          Validators.maxLength(31),
          Validators.pattern('^[A-Za-z0-9ñÑáéíóúÁÉÍÓÚ/^\S*$/]+$'), // only allow text and numbers
        ]
      })
    })
  }

  public _userDetails!: UserDetailsModel;


  @Input() set userDetails(value: UserDetailsModel) {
    this._userDetails = value;
    this.setUserDetailsEditForm(this._userDetails);

    // format createdOn and modifiedOn
    this._userDetails.createdOn = this._userDetails.createdOn.length > 0 ?
      formatDate(this._userDetails.createdOn, ConstantsModels.dateTimeReadableLongFormat, this.locale)
      : "";
    this._userDetails.modifiedOn = this._userDetails.modifiedOn && this._userDetails.modifiedOn.length > 0 ?
      formatDate(this._userDetails.modifiedOn, ConstantsModels.dateTimeReadableLongFormat, this.locale)
      : null;
  };

  validateControl = (controlName: string) => {
    return this.userDetailsEditForm.get(controlName)?.invalid;
  }

  hasError = (controlName: string, errorName: string) => {
    return this.userDetailsEditForm.get(controlName)?.hasError(errorName);
  };

  updateUser = (userDetailsEditFormValue: any) => {
    const formValues = {...userDetailsEditFormValue};
    const updateUserCommand: UpdateUserCommand = {
      userName: formValues.userName
    };

    if (updateUserCommand.userName === this._userDetails.userName) {
      // dont update its the same
      return;
    }

    this.updateUserEvent.emit(updateUserCommand);
  }

  private setUserDetailsEditForm = (userDetails: UserDetailsModel) => {
    this.userDetailsEditForm.setValue({
      userName: userDetails.userName
    });

  }
}

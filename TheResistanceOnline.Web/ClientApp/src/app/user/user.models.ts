import {AbstractControl, FormControl, FormGroup} from "@angular/forms";

export interface UserDetailsModel {
  userName: string,
  createdOn: string,
  modifiedOn: string | null
}

export interface UpdateUserCommand {
  userName: string
}

export interface UserDetailsFormModel {
  userDetailsEditForm: FormGroup<UserDetailsEditFormModel>

}

export interface UserDetailsEditFormModel {
  userName: FormControl<string>,
  // createdOn: FormControl<string>,
  // modifiedOn: FormControl<string | null>
}

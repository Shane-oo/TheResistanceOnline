import { Injectable } from '@angular/core';
import { AbstractControl, ValidatorFn } from '@angular/forms';

@Injectable({
              providedIn: 'root'
            })
export class PasswordValidatorService {

  constructor() {
  }

  public validateConfirmPassword = (passwordControl: AbstractControl | null): ValidatorFn => {
    return (confirmationControl: AbstractControl): { [key: string]: boolean } | null => {
      const confirmValue = confirmationControl.value;
      const passwordValue = passwordControl?.value;
      if(confirmValue === '') {
        // confirmPassword not entered yet
        return {empty: true};
      }
      if(confirmValue !== passwordValue) {
        return {mustMatch: true};
      }
      return null;
    };
  };
}

import { Injectable } from '@angular/core';
import { AbstractControl, FormGroup, ValidatorFn } from '@angular/forms';

@Injectable({
              providedIn: 'root'
            })
export class GameValidatorService {

  constructor() {
  }

  //requireCheckboxesToBeCheckedValidator(minRequired = 1): ValidatorFn {
  public requireCheckboxesToBeCheckedValidator = (minRequired = 1) => {
    return function validate (formGroup: FormGroup) {
      let checked = 0;

      Object.keys(formGroup.controls).forEach(key => {
        const control = formGroup.controls[key];

        if (control.value === true) {
          checked ++;
        }
      });

      if (checked < minRequired) {
        return {
          requireOneCheckboxToBeChecked: true,
        };
      }

      return null;
    };
  };
}

import { ValidatorFn, AbstractControl } from '@angular/forms';

export function noEmptySpacesValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const invalid = (control.value || '').indexOf(' ') > -1;
    return invalid ? { noEmptySpaces: { value: control.value } } : null;
  };
}

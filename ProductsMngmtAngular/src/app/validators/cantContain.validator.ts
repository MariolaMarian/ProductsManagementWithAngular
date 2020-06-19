import { ValidatorFn, AbstractControl } from '@angular/forms';

export function cantContainValidator(forbidden: RegExp): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const invalid = forbidden.test(control.value);
    return invalid ? { cantContain: { value: control.value } } : null;
  };
}

import { ValidatorFn, AbstractControl } from '@angular/forms';

export function DateLaterThanTodayValidator(): ValidatorFn {
  return (control: AbstractControl): { [key: string]: any } | null => {
    const today = new Date();
    const diff = new Date(control.value).getTime() - today.getTime();
    return diff < 0 ? { tooLate: { value: control.value } } : null;
  };
}

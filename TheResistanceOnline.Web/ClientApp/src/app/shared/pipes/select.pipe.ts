import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'select'
})
export class SelectPipe implements PipeTransform {
  transform(value: any[], key: string): any {
    return value.map(value => value[key]);
  }
}


/*
It's not recommended to call functions on the component to calculate values for the template.
 This moves work from the component to the template, and if you need data to be mutated
  then do that work in ngOnInit() or in ngOnChanges() instead.
Pipes have purity which means they are executed only when the inbound data is mutated.
 When you call a function like {{doWork(object.array)}} then Angular does not know if the function doWork()
 is pure or not. So it assumes it is not pure and calls it for each change detection.
 */

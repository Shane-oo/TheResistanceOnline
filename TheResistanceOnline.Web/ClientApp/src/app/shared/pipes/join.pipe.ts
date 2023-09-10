import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  name: 'join'
})
export class JoinPipe implements PipeTransform {
  //To minimize the load on your application, you might also consider creating a pipe.

  transform(value: Array<any>, sep = ','): string {
    return value.join(sep);
  }

}

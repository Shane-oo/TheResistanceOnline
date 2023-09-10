import {NgModule} from '@angular/core';
import {CommonModule} from '@angular/common';
import {JoinPipe} from "./join.pipe";
import {SelectPipe} from "./select.pipe";


@NgModule({
  declarations: [JoinPipe,
    SelectPipe],
  exports: [
    SelectPipe
  ],
  imports: [
    CommonModule
  ]
})
export class PipesModule {
}

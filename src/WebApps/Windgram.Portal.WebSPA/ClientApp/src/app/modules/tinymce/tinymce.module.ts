import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { TinymceComponent } from './tinymce.component';

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [TinymceComponent],
  exports: [TinymceComponent]
})
export class TinymceModule { }

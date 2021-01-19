import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { NzFormModule } from 'ng-zorro-antd/form';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzButtonModule } from 'ng-zorro-antd/button';
import { NzInputModule } from 'ng-zorro-antd/input';

const SHARDD_MODULES = [
  CommonModule,
  RouterModule,
  FormsModule,
  ReactiveFormsModule
];
const SHARDD_ANTD_MODULES = [
  NzFormModule,
  NzGridModule,
  NzButtonModule,
  NzInputModule
];
@NgModule({
  imports: [
    ...SHARDD_MODULES,
    ...SHARDD_ANTD_MODULES
  ],
  exports: [
    ...SHARDD_MODULES,
    ...SHARDD_ANTD_MODULES
  ]
})
export class SharedModule { }

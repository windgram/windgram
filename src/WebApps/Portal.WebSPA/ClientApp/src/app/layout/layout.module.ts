import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { LayoutDefaultComponent } from './default/default.component';
import { LayoutDefaultHeaderComponent } from './default/header/header.component';
import { RouterModule } from '@angular/router';
import { NzLayoutModule } from 'ng-zorro-antd/layout';
import { NzGridModule } from 'ng-zorro-antd/grid';
import { NzMenuModule } from 'ng-zorro-antd/menu';
import { NzAvatarModule } from 'ng-zorro-antd/avatar';
import { NzIconModule } from 'ng-zorro-antd/icon';
import { NzInputModule } from 'ng-zorro-antd/input';

const COMPONENTS = [
  LayoutDefaultComponent,
  LayoutDefaultHeaderComponent
];
@NgModule({
  imports: [
    CommonModule,
    RouterModule,
    NzLayoutModule,
    NzGridModule,
    NzMenuModule,
    NzAvatarModule,
    NzIconModule,
    NzInputModule
  ],
  declarations: [
    ...COMPONENTS
  ]
})
export class LayoutModule { }

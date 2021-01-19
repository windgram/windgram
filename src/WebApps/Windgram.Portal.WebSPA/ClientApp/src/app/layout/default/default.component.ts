import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-layout-default',
  templateUrl: './default.component.html',
  styleUrls: ['./default.component.scss']
})
export class LayoutDefaultComponent implements OnInit {
  year = (new Date()).getFullYear();
  constructor() { }

  ngOnInit() {

  }

}

/* tslint:disable:no-unused-variable */
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { DebugElement } from '@angular/core';

import { LayoutDefaultHeaderComponent } from './header.component';

describe('HeaderComponent', () => {
  let component: LayoutDefaultHeaderComponent;
  let fixture: ComponentFixture<LayoutDefaultHeaderComponent>;

  beforeEach((() => {
    TestBed.configureTestingModule({
      declarations: [LayoutDefaultHeaderComponent]
    })
      .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(LayoutDefaultHeaderComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});

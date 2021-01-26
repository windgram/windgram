/* tslint:disable:no-unused-variable */

import { TestBed, async, inject } from '@angular/core/testing';
import { JsLoaderService } from './js-loader.service';

describe('Service: JsLoader', () => {
  beforeEach(() => {
    TestBed.configureTestingModule({
      providers: [JsLoaderService]
    });
  });

  it('should ...', inject([JsLoaderService], (service: JsLoaderService) => {
    expect(service).toBeTruthy();
  }));
});

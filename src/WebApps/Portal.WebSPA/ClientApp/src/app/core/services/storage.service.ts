import { Injectable } from '@angular/core';

@Injectable()
export class StorageService {
  private storage: any;
  constructor() {
    this.storage = sessionStorage;
  }

  getValue(key: string) {
    const item = this.storage.getItem(key);
    if (item) {
      return JSON.parse(item);
    }
    return;
  }

  setValue(key: string, value: any) {
    this.storage.setItem(key, JSON.stringify(value));
  }

  remove(key: string) {
    this.storage.removeItem(key);
  }
  clear() {
    this.storage.clear();
  }

}

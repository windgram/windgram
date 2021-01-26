import { Injectable, Inject } from '@angular/core';
import { Subject } from 'rxjs';
import { DOCUMENT } from '@angular/common';

@Injectable()
export class JsLoaderService {
  private static container: { path: string, loaded: boolean }[] = [];
  private loaderSubject = new Subject<string>();
  public onLoaded = this.loaderSubject.asObservable();
  constructor(
    @Inject(DOCUMENT) private document: any,
  ) { }

  load(path: string) {
    let item = JsLoaderService.container.find(p => p.path === path);
    if (!item) {
      item = {
        path,
        loaded: false
      };
      JsLoaderService.container.push(item);
      this.loadScript(item)
        .then(() => {
          this.loaderSubject.next(path);
        })
        .catch(() => {
          console.warn(`JS file load failed ${item.path}`);
        });
    } else if (item.loaded) {
      this.loaderSubject.next(path);
    }
  }

  private loadScript(item): Promise<void> {
    return new Promise((resolve, reject) => {
      const node: HTMLScriptElement = this.document.createElement('script');
      node.type = 'text/javascript';
      node.src = item.path;
      node.onload = () => {
        item.loaded = true;
        resolve();
      };
      node.onerror = () => {
        reject();
      };
      this.document.getElementsByTagName('head')[0].appendChild(node);
    });
  }

}

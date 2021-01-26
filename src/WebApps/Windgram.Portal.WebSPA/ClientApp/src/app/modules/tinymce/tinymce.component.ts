import { Component, Input, ChangeDetectorRef, forwardRef, ChangeDetectionStrategy, AfterViewInit, ViewEncapsulation } from '@angular/core';
import { ControlValueAccessor, NG_VALUE_ACCESSOR } from '@angular/forms';
import { JsLoaderService } from 'src/app/core/services/js-loader.service';
import { environment } from 'src/environments/environment';


@Component({
  // tslint:disable-next-line: component-selector
  selector: 'tinymce-editor',
  template: `<textarea [id]="id" [placeholder]="placeholder"></textarea>`,
  providers: [
    {
      provide: NG_VALUE_ACCESSOR,
      useExisting: forwardRef(() => TinymceComponent),
      multi: true
    }
  ],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class TinymceComponent implements AfterViewInit, ControlValueAccessor {
  @Input() config: any;
  @Input() placeholder = '';
  @Input() set disabled(value: boolean) {
    this.currentDisabled = value;
    this.setDisabled();
  }
  id = `tinymce-${Math.random().toString(36).substring(3)}`;
  private currentDisabled = false;
  private currentInstance: any;
  private innerValue: string;
  private onChange: (value: string) => void;
  private onTouched: () => void;

  get value() {
    return this.innerValue;
  }

  private setValue(content: string) {
    this.innerValue = content || '';
    if (this.currentInstance) {
      this.currentInstance.setContent(this.innerValue);
      this.cdr.markForCheck();
    }
  }

  get instance() {
    return this.currentInstance;
  }

  private setDisabled() {
    if (!this.currentInstance) {
      return;
    }
    this.currentInstance.setMode(this.currentDisabled ? 'readonly' : 'design');
  }
  constructor(
    private cdr: ChangeDetectorRef,
    private jsLoaderService: JsLoaderService
  ) { }

  private initEditor() {
    const that = this;
    if (!tinymce) {
      throw new Error('tinymce load failed');
    }
    if (this.config) {
      this.config.selector = `#${that.id}`;
    }
    const options = Object.assign(
      {
        selector: `#${that.id}`,
        language: 'zh_CN',
        language_url: `${environment.tinymcePath}/langs/zh_CN.js`,
        branding: false,
        setup: (editor: any) => {
          this.currentInstance = editor;
          editor.on('change keyup', () => {
            this.innerValue = editor.getContent();
            this.onChange(this.innerValue);
            this.cdr.markForCheck();
          });
        },
        init_instance_callback: (editor: any) => {
          if (editor && this.value) {
            editor.setContent(this.value);
          }
          this.setDisabled();
        }
      },
      that.config
    );
    tinymce.init(options);
    this.cdr.markForCheck();
  }

  ngAfterViewInit() {
    const tinymcePath = `${environment.tinymcePath}/tinymce.min.js`;
    this.jsLoaderService.onLoaded.subscribe(path => {
      if (path === tinymcePath) {
        setTimeout(() => {
          this.initEditor();
        });
      }
    });
    this.jsLoaderService.load(tinymcePath);
  }

  writeValue(value: any): void {
    this.setValue(value);
  }
  registerOnChange(fn: any): void {
    this.onChange = fn;
  }
  registerOnTouched(fn: any): void {
    this.onTouched = fn;
  }

  setDisabledState?(isDisabled: boolean): void {
    this.disabled = isDisabled;
    this.setDisabled();
  }
}

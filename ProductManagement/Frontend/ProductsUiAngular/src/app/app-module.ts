import { NgModule, provideBrowserGlobalErrorListeners, LOCALE_ID } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { CommonModule, registerLocaleData } from '@angular/common'; 
import localeDe from '@angular/common/locales/de';

import { AppRoutingModule } from './app-routing-module';
import { App } from './app';
import { ProductList } from './product-list/product-list';
import { CategoryList } from './category-list/category-list';

registerLocaleData(localeDe, 'de');

@NgModule({
  declarations: [
    App,
    ProductList,
    CategoryList
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    CommonModule
  ],
  providers: [
    provideBrowserGlobalErrorListeners(),
    { provide: LOCALE_ID, useValue: 'de-DE' } 
  ],
  bootstrap: [App]
})
export class AppModule { }

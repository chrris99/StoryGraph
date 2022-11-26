import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HeaderComponent } from './header/header.component';
import { LayoutComponent } from './layout/layout.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';

@NgModule({
  declarations: [
    HeaderComponent,
    LayoutComponent,
    NotFoundComponent
  ],
  imports: [
    CommonModule
  ],
  exports: [
    LayoutComponent,
    NotFoundComponent
  ]
})
export class SharedModule { }

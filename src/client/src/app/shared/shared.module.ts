import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterModule } from '@angular/router';

import { NavigationComponent } from './components/navigation/navigation.component';
import { LayoutComponent } from './layout/layout.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { FooterComponent } from './components/footer/footer.component';
import { TitleComponent } from './components/title/title.component';
import { UnauthorizedComponent } from './errors/unauthorized/unauthorized.component';
import { ErrorComponent } from './errors/error/error.component';

@NgModule({
  declarations: [
    NavigationComponent,
    LayoutComponent,
    NotFoundComponent,
    FooterComponent,
    TitleComponent,
    UnauthorizedComponent,
    ErrorComponent
  ],
  imports: [
    CommonModule,
    RouterModule
  ],
  exports: [
    LayoutComponent,
    NotFoundComponent,
    TitleComponent,
    UnauthorizedComponent
  ]
})
export class SharedModule { }

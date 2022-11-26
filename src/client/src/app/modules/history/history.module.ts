import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { HistoryRoutingModule } from './history-routing.module';
import { SharedModule } from 'src/app/shared/shared.module';
import { HistoryTableComponent } from './components/history-table/history-table.component';


@NgModule({
  declarations: [
    HistoryTableComponent
  ],
  imports: [
    CommonModule,
    HistoryRoutingModule,
    SharedModule
  ]
})
export class HistoryModule { }

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { AuthGuard } from 'src/app/shared/guards/auth.guard';
import { HistoryTableComponent } from './history-table/history-table.component';

const routes: Routes = [
  { path: '', canActivate: [AuthGuard], component: HistoryTableComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class HistoryRoutingModule { }

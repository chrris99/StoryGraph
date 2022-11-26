import { Component, OnInit } from '@angular/core';
import { Record } from '../../models/record';
import { HistoryService } from '../../services/history.service';

@Component({
  selector: 'app-history-table',
  templateUrl: './history-table.component.html',
  styleUrls: ['./history-table.component.css']
})
export class HistoryTableComponent implements OnInit {
  title: string = "History";
  description: string = 'Have a look at your previously visualized stories.'

  records: Record[] = [];

  constructor(private history: HistoryService) { }

  ngOnInit(): void { }
}

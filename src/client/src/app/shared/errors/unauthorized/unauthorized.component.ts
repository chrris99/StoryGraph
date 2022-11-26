import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-unauthorized',
  templateUrl: './unauthorized.component.html',
  styleUrls: ['./unauthorized.component.css']
})
export class UnauthorizedComponent implements OnInit {
  errorCode: number = 401;
  errorMessage: string = 'Looks like you don\'t have access to this page.';

  constructor() { }

  ngOnInit(): void { }
}

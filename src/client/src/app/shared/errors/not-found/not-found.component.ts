import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-not-found',
  templateUrl: './not-found.component.html',
  styleUrls: ['./not-found.component.css']
})
export class NotFoundComponent implements OnInit {
  errorCode: number = 404;
  errorMessage: string = 'We can\'t find that page.';

  constructor() { }

  ngOnInit(): void {}
}

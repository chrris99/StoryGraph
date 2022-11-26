import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  title: string = 'Get started today!';
  description: string = 'Turn any story into a spectacular graph-based visual representation of appearing characters and there relations.';

  constructor() { }

  ngOnInit(): void { }
}

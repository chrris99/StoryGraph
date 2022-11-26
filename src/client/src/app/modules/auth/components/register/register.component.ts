import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  title: string = 'Get started today!';
  description: string = 'Turn any story into a spectacular graph-based visual representation of appearing characters and there relations.';

  constructor() { }

  ngOnInit(): void { }
}

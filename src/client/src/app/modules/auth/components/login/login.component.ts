import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  title: string = 'Get started today!';
  description: string = 'Turn any story into a spectacular graph-based visual representation of appearing characters and there relations.';

  form: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private auth: AuthService
  ) {
    this.form = this.formBuilder.group({
      email: [ '', Validators.required ],
      password: [ '', Validators.required ]
    });
  }

  ngOnInit(): void { }

  public login(): void {
    const data = this.form.value;

    if (data.email && data.password) {
      this.auth.login(data.email, data.password);
    }
  }
}

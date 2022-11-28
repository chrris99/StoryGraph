import { Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/shared/services/auth.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  title: string = 'Get started today!';
  description: string = 'Turn any story into a spectacular graph-based visual representation of appearing characters and there relations.';

  form: FormGroup;

  constructor(
    private auth: AuthService,
    private formBuilder: FormBuilder
  ) {
    this.form = formBuilder.group({
      name: ['', [Validators.required]],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(8)]]
    });
  }

  ngOnInit(): void { }

  public register(): void {
    const data = this.form.value;

    if (data.email && data.password && data.name) {
      this.auth
        .register(data.email, data.name, data.password);
    }
  }
}

import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';

import { StoryService } from '../../services/story.service';

@Component({
  selector: 'app-story-input',
  templateUrl: './story-input.component.html',
  styleUrls: ['./story-input.component.css']
})
export class StoryInputComponent implements OnInit {
  form: FormGroup;

  constructor(
    private formBuilder: FormBuilder,
    private story: StoryService
  ) {
    this.form = formBuilder.group({
      title: [ '', Validators.required ],
      text: [ '', Validators.required ]
    });
  }

  ngOnInit(): void { }

  public submit(): void {
    if (this.form.valid) {
      const title: string = this.form.value.title;
      const text: string = this.form.value.text;

      this.story
      .validate({
        title: title,
        text: text
      })
      .subscribe(() => {
        console.log("Input validation completed");

        this.story
          .visualize({
            title: title,
            text: text
          })
          .subscribe(() => console.log("Story visualization completed"));
      });
    }
  }
}

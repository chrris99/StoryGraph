import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { resourceUsage } from 'process';
import { StoryService } from '../../services/story.service';

@Component({
  selector: 'app-story-input',
  templateUrl: './story-input.component.html',
  styleUrls: ['./story-input.component.css']
})
export class StoryInputComponent implements OnInit {
  @Output() inputSubmitted: EventEmitter<string> = new EventEmitter<string>(); // might not be needed

  constructor(private story: StoryService) { }

  ngOnInit(): void { }

  public submit(title: string, story: string): void {
    this.story
      .validate({
        title: title,
        text: story
      })
      .subscribe(result => {
        if (!result.ok) {

        }
      })
  }
}

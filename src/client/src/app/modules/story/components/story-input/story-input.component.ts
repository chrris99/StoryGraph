import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-story-input',
  templateUrl: './story-input.component.html',
  styleUrls: ['./story-input.component.css']
})
export class StoryInputComponent implements OnInit {
  @Output() inputSubmitted: EventEmitter<string> = new EventEmitter<string>();

  constructor() { }

  ngOnInit(): void { }
}

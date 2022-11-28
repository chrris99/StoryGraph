import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-story-visualizer',
  templateUrl: './story-visualizer.component.html',
  styleUrls: ['./story-visualizer.component.css']
})
export class StoryVisualizerComponent implements OnInit {
  title: string = 'Story';
  description: string = 'Have an overview of your story at a glance.'

  inputSubmitted: boolean | undefined;

  constructor() { }

  ngOnInit(): void { }

  public receiveInputSubmitEvent($event: boolean) {
    console.log("event received");
    console.log($event);

    this.inputSubmitted = $event;
  }
}

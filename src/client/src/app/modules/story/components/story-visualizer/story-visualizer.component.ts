import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-story-visualizer',
  templateUrl: './story-visualizer.component.html',
  styleUrls: ['./story-visualizer.component.css']
})
export class StoryVisualizerComponent implements OnInit {
  title: string = 'Story';
  description: string = 'Have an overview of your story at a glance.'

  constructor() { }

  ngOnInit(): void { }
}

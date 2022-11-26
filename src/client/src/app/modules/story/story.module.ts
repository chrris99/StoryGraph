import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';

import { StoryRoutingModule } from './story-routing.module';
import { StoryGraphComponent } from './components/story-graph/story-graph.component';
import { StoryInputComponent } from './components/story-input/story-input.component';
import { SharedModule } from 'src/app/shared/shared.module';
import { StoryVisualizerComponent } from './components/story-visualizer/story-visualizer.component';

@NgModule({
  declarations: [
    StoryGraphComponent,
    StoryInputComponent,
    StoryVisualizerComponent,
  ],
  imports: [
    CommonModule,
    SharedModule,
    StoryRoutingModule
  ]
})
export class StoryModule { }

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { StoryVisualizerComponent } from './components/story-visualizer/story-visualizer.component';

const routes: Routes = [
  { path: '', component: StoryVisualizerComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class StoryRoutingModule { }

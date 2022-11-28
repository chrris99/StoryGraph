import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { Edge, Node } from '@swimlane/ngx-graph';
import { Subject } from 'rxjs';

import { SignalrService } from 'src/app/shared/services/signalr.service';
import { EdgeDetected } from '../../events/edge-detected';
import { NodeDetected } from '../../events/node-detected';

@Component({
  selector: 'app-story-graph',
  templateUrl: './story-graph.component.html',
  styleUrls: ['./story-graph.component.css']
})
export class StoryGraphComponent implements OnInit, OnDestroy {
  private _inputSubmitted: boolean | undefined;

  @Input()
  get inputSubmitted(): boolean | undefined { return this._inputSubmitted; }
  set inputSubmitted(inputSubmitted: boolean | undefined) {
    this._inputSubmitted = inputSubmitted;

    if (this.inputSubmitted) {
      this.nodes = [];
      this.edges = [];
    }
  }

  nodes: Node[] = [];
  edges: Edge[] = [];

  constructor(private signalr: SignalrService) { }

  ngOnInit(): void {
    this.signalr
      .startConnection()
      .then(() => {
        this.signalr
          .subscribe(NodeDetected, event => this.addNode(event));

        this.signalr
          .subscribe(EdgeDetected, event => this.addEdge(event))
      });
  }

  ngOnDestroy(): void {
    this.signalr.unsubscribe(NodeDetected);
    this.signalr.unsubscribe(EdgeDetected)
  }

  private addNode(event: NodeDetected): void {
    if (!event.id) return;

    const node: Node | undefined = this.nodes
      .find(node => node.id == event.id);

    if (!node) {
      this.nodes.push({
        id: event.id,
        label: event.label
      });

      console.log(event.id);
      console.log(event.label);

      this.nodes = [...this.nodes];
    }
  }

  private addEdge(event: EdgeDetected): void {
    if (!event.target || !event.source) return;

    const edge: Edge | undefined = this.edges
      .find(edge => edge.source == event.source && edge.target == event.target);

    if (!edge) {
      this.edges.push({
        id: event.id,
        source: event.source,
        target: event.target,
        label: event.label
      });

      this.edges = [...this.edges];
    }
  }
}

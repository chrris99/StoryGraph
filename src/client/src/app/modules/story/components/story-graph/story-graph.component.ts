import { Component, OnDestroy, OnInit } from '@angular/core';
import { Edge, Node } from '@swimlane/ngx-graph';
import { SignalrService } from 'src/app/shared/services/signalr.service';
import { EdgeDetected } from '../../events/edge-detected';
import { NodeDetected } from '../../events/node-detected';
import { StoryService } from '../../services/story.service';

@Component({
  selector: 'app-story-graph',
  templateUrl: './story-graph.component.html',
  styleUrls: ['./story-graph.component.css']
})
export class StoryGraphComponent implements OnInit, OnDestroy {
  nodes: Node[] = [];
  edges: Edge[] = [];

  constructor(
    private graph: StoryService,
    private signalr: SignalrService
  ) { }

  ngOnInit(): void {
    this.signalr
      .startConnection()
      .then(() => {
        this.signalr
          .subscribe(NodeDetected, event => this.addNode(event));

        this.signalr
          .subscribe(EdgeDetected, event => this.addEdge(event))
      });

    this.nodes = this.graph.getNodes();
    this.edges = this.graph.getEdges();
  }

  ngOnDestroy(): void {
    this.signalr.unsubscribe(NodeDetected);
    this.signalr.unsubscribe(EdgeDetected)
  }

  private addNode(event: NodeDetected): void {
    if (!event.id) return;

    const node: Node = {
      id: event.id,
      label: event.label
    };

    this.nodes.push(node);
    this.nodes = [...this.nodes];
  }

  private addEdge(event: EdgeDetected): void {
    if (!event.target || !event.source) return;

    const edge: Edge = {
      id: event.id,
      source: event.source,
      target: event.target,
      label: event.label
    };

    this.edges.push(edge);
    this.edges = [...this.edges];
  }
}

import { Component, OnInit } from '@angular/core';
import { Edge, Node } from '@swimlane/ngx-graph';
import { Subject } from 'rxjs';
import { GraphService } from '../../services/graph.service';

@Component({
  selector: 'app-story-graph',
  templateUrl: './story-graph.component.html',
  styleUrls: ['./story-graph.component.css']
})
export class StoryGraphComponent implements OnInit {
  nodes: Node[] = [];
  edges: Edge[] = [];

  constructor(private graph: GraphService) { }

  ngOnInit(): void {
    this.nodes = this.graph.getNodes();
    this.edges = this.graph.getEdges();

    /*
    Update nodes or edges array like this, to rerender graph

    this.nodes.push(newNode);
    this.nodes = [...this.nodes];

    look into: center and fit-to-view observables
    */
  }
}

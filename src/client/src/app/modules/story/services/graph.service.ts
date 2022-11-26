import { Injectable } from '@angular/core';
import { Node, Edge } from '@swimlane/ngx-graph';

@Injectable({
  providedIn: 'root'
})
export class GraphService {
  constructor() { }


  public getEdges(): Edge[] {
    const edges: Edge[] = [
      {
        id: 'a',
        source: '1',
        target: '2'
      },
      {
        id: 'b',
        source: '1',
        target: '3'
      },
      {
        id: 'c',
        source: '3',
        target: '4'
      },
      {
        id: 'd',
        source: '3',
        target: '5'
      },
      {
        id: 'e',
        source: '4',
        target: '5'
      },
      {
        id: 'f',
        source: '2',
        target: '6'
      }
    ];

    return edges;
  }

  public getNodes(): Node[] {
    const nodes: Node[] = [
      {
        id: '1',
        label: 'Node A'
      },
      {
        id: '2',
        label: 'Node B'
      },
      {
        id: '3',
        label: 'Node C'
      },
      {
        id: '4',
        label: 'Node D'
      },
      {
        id: '5',
        label: 'Node E'
      },
      {
        id: '6',
        label: 'Node F'
      }
    ];

    return nodes;
  }
}

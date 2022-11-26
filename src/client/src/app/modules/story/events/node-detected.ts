import { BaseEvent } from "src/app/shared/events/base-event";

export class NodeDetected extends BaseEvent {
  id?: string;
  label?: string;

  constructor() {
    super(BaseEvent.NodeDetected);
  }
}

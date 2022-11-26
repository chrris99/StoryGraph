import { BaseEvent } from "src/app/shared/events/base-event";

export class EdgeDetected extends BaseEvent {
  id?: string;
  source?: string;
  target?: string;
  label?: string;

  constructor() {
    super(BaseEvent.EdgeDetected);
  }
}

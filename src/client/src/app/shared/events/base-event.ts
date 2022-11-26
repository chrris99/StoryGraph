export abstract class BaseEvent {
  public static readonly NodeDetected = 'NodeDetected';
  public static readonly EdgeDetected = 'EdgeDetected';

  constructor(public readonly className: string) { }
}

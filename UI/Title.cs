using Godot;
using System;

public class Title : Control
{
  public override void _UnhandledInput(InputEvent @event)
  {
    base._UnhandledInput(@event);
    
    if (!@event.IsActionPressed("5")) return;

    GameManager.NewGame();
  }
}

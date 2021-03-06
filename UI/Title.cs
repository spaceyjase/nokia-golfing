using Godot;
using System;

public class Title : Control
{
  private AudioStream buttonSfx;
  private Node audioManager;

  public override void _Ready()
  {
    base._Ready();
    buttonSfx = ResourceLoader.Load<AudioStream>("res://data/sfx/blip14.wav");
    audioManager = GetNode("/root/AudioManager");
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    base._UnhandledInput(@event);
    
    if (!@event.IsActionPressed("5")) return;

    audioManager.Call("play_sfx", buttonSfx);
    GameManager.NewGame();
  }
}

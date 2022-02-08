using Godot;
using System;

public class Arrow : Node2D
{
  private Sprite sprite;

  public override void _Ready()
  {
    base._Ready();
    sprite = GetNode<Sprite>(nameof(Sprite));
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    sprite.GlobalRotation = 0f;
  }
}

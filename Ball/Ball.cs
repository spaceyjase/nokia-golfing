using Godot;
using System;

public class Ball : RigidBody2D
{
  [Signal]
  public delegate void Stopped();
  
  [Export] private float stopVelocity = 0.25f;
  
  private bool moving;
  private Sprite sprite;

  public override void _Ready()
  {
    base._Ready();
    sprite = GetNode<Sprite>(nameof(Sprite));
  }

  public void Shoot(float angle, float power)
  {
    var force = Vector2.Right.Rotated(angle) * power;
    ApplyImpulse(Vector2.Zero, force * power / 5f);
    moving = true;
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    
    sprite.GlobalRotation = 0f;
  }

  public override void _IntegrateForces(Physics2DDirectBodyState state)
  {
    base._IntegrateForces(state);

    if (!moving) return;
    if (!(state.LinearVelocity.Length() < stopVelocity)) return;
    
    EmitSignal(nameof(Stopped));
    state.LinearVelocity = Vector2.Zero;
    moving = false;
  }
}

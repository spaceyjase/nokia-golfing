using Godot;
using System;

public class Hole : Area2D
{
  [Signal] public delegate void Holed(bool holed);

  [Export] private bool useTimer = false;

  private Timer timer;
  
  public override void _Ready()
  {
    base._Ready();
    
    timer = GetNode<Timer>("Timer");
  }

  private void On_Hole_body_entered(Node body)
  {
    if (useTimer)
    {
      timer.Start();
    }
    else
    {
      HoleEntered();
    }
  }

  private void HoleEntered()
  {
    EmitSignal(nameof(Holed), true);
  }

  private void On_Hole_body_exited(Node body)
  {
    timer.Stop();
  }

  private void On_Timer_timeout()
  {
    HoleEntered();
  }
}

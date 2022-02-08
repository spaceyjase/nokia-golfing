using Godot;

public class Holes : Node2D
{
  [Signal]
  public delegate void Victory();
  
  private void On_Hole_Holed(bool holed)
  {
    if (!holed) return;

    EmitSignal(nameof(Victory));
  }
}

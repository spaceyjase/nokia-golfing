using Godot;
using System;
using Godot.Collections;

public class GameManager : Node2D
{
  [Export] private Array<PackedScene> levels;

  private static GameManager Instance { get; set; } // Crap Singleton
  private int score;
  
  public GameManager()
  {
    Instance = this;
  }

  public override void _Ready()
  {
    base._Ready();
  }

  public static void NewGame()
  {
    Instance.CurrentLevel = 0;
    Instance.score = 0;
    Instance.LoadCurrentLevel();
  } 
  
  public static void LoadNextLevel()
  {
    Instance.CurrentLevel++;
    Instance.LoadCurrentLevel();
  }

  private void LoadCurrentLevel()
  {
    if (CurrentLevel == levels.Count)  // This includes the end game scene
    {
      CurrentLevel = 0;
      LoadTitle();
    }
    else
    {
      var level = levels[CurrentLevel].Instance();
      ReconfigureViewportWithLevel(level);
    }
  }

  private void ReconfigureViewportWithLevel(Node level)
  {
    var viewport = GetNode<Viewport>("/root/Main/ViewportContainer/Viewport");
    foreach (Node child in viewport.GetChildren())
    {
      viewport.RemoveChild(child);
      child.QueueFree();
    }

    viewport.AddChild(level);
  }

  private void LoadTitle()
  {
    Instance.GetTree().ChangeScene("res://data/scenes/Main.tscn");
  }

  private int CurrentLevel { get; set; }

  public static int Score => Instance.score;
  
  public static int UpdateScore()
  {
    Instance.score += 1;
    
    return Instance.score;
  }
}
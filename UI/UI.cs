using Godot;
using System;

public class UI : Control
{
  private Label shots;
  private TextureProgress powerbar;
  private Control victoryUi;
  
  public double MaxPower => powerbar.MaxValue;
  public double MinPower => powerbar.MinValue;

  public override void _Ready()
  {
    base._Ready();

    powerbar = GetNode<TextureProgress>("MarginContainer/VBoxContainer/PowerBar");
    shots = GetNode<Label>("MarginContainer/VBoxContainer/Shots");
    victoryUi = GetNode<Control>("VictoryUI");
  }

  public void UpdateScore(int score)
  {
    shots.Text = $"{score}";
  }

  public void UpdatePowerbar(float power)
  {
    powerbar.Value = power;
  }

  private void On_Holes_Victory()
  {
    victoryUi.Show();
  }
}

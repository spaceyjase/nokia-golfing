using Godot;
using System;

public class Level : Node2D
{
  private enum LevelState
  {
    SetAngle,
    SetPower,
    Shoot,
    Complete,
  }

  [Export] public int powerChange = 1;
  [Export] public int powerSpeed = 100;
  [Export] public int angleChange = 1;
  [Export] public float angleSpeed = 50f;
  [Export] public int angleMaxDegrees = 75;

  private Ball ball;
  private Arrow arrow;
  private Node2D hole;
  private UI ui;
  private LevelState state;
  private float power;
  private Node audioManager;

  private float holeDirection;
  private bool complete;
  private AudioStream readySfx;
  private AudioStream shotSfx;
  private AudioStream stopSfx;
  private AudioStream holeSfx;
  private AudioStream buttonSfx;

  public override void _Ready()
  {
    base._Ready();

    ball = GetNode<Ball>(nameof(Ball));
    arrow = GetNode<Arrow>(nameof(Arrow));
    hole = GetNode<Node2D>("Holes/Hole");
    ui = GetNode<UI>(nameof(UI));
    audioManager = GetNode("/root/AudioManager");
    
    readySfx = ResourceLoader.Load<AudioStream>("res://data/sfx/blip6.wav");
    shotSfx = ResourceLoader.Load<AudioStream>("res://data/sfx/blip11.wav");
    stopSfx = ResourceLoader.Load<AudioStream>("res://data/sfx/hit2.wav");
    holeSfx = ResourceLoader.Load<AudioStream>("res://data/sfx/good3.wav");
    buttonSfx = ResourceLoader.Load<AudioStream>("res://data/sfx/good1.wav");

    arrow.Hide();
    ChangeState(LevelState.SetAngle);

    ui.UpdateScore(GameManager.Score);
  }

  private void ChangeState(LevelState newState)
  {
    state = newState;
    switch (state)
    {
      case LevelState.SetAngle:
        var arrowPosition = arrow.Transform;
        arrowPosition.origin = ball.Transform.origin;
        arrow.Transform = arrowPosition;
        SetStartAngle();
        arrow.Show();
        break;
      case LevelState.SetPower:
        audioManager.Call("play_sfx", readySfx);
        break;
      case LevelState.Shoot:
        arrow.Hide();
        audioManager.Call("play_sfx", shotSfx);
        ball.Shoot(arrow.Rotation, power);
        break;
      case LevelState.Complete:
        audioManager.Call("play_sfx", holeSfx);
        complete = true;
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public override void _UnhandledInput(InputEvent @event)
  {
    base._UnhandledInput(@event);

    if (!@event.IsActionPressed("5")) return;

    switch (state)
    {
      case LevelState.SetAngle:
        ChangeState(LevelState.SetPower);
        break;
      case LevelState.SetPower:
        ChangeState(LevelState.Shoot);
        break;
      case LevelState.Shoot:
        break;
      case LevelState.Complete:
        audioManager.Call("play_sfx", buttonSfx);
        GameManager.LoadNextLevel();
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  public override void _Process(float delta)
  {
    base._Process(delta);
    switch (state)
    {
      case LevelState.SetAngle:
        AnimateAngle(delta);
        break;
      case LevelState.SetPower:
        AnimatePower(delta);
        break;
      case LevelState.Shoot:
        break;
      case LevelState.Complete:
        break;
      default:
        throw new ArgumentOutOfRangeException();
    }
  }

  // ReSharper disable once UnusedMember.Local
  private void OnBall_Stopped()
  {
    if (complete) return;
    
    ChangeState(LevelState.SetAngle);
    ui.UpdateScore(GameManager.UpdateScore());
    audioManager.Call("play_sfx", stopSfx);
  }

  private void AnimatePower(float delta)
  {
    power += powerSpeed * powerChange * delta;
    if (power > (int)ui.MaxPower)
    {
      powerChange = -1;
    }

    if (power <= (int)ui.MinPower)
    {
      powerChange = 1;
    }

    ui.UpdatePowerbar(power);
  }

  private void AnimateAngle(float delta)
  {
    arrow.Rotation += angleSpeed * angleChange * delta;
    if (arrow.Rotation > holeDirection + Mathf.Pi / 2)
    {
      angleChange = -1;
    }
    if (arrow.Rotation < holeDirection - Mathf.Pi / 2)
    {
      angleChange = 1;
    }
  }

  private void SetStartAngle()
  {
    var holePosition = new Vector2(hole.GlobalTransform.origin.x,
      hole.GlobalTransform.origin.y);
    var ballPosition = new Vector2(ball.GlobalTransform.origin.x,
      ball.GlobalTransform.origin.y);
    holeDirection = (holePosition - ballPosition).Angle();
    arrow.Rotation = holeDirection;
  }

  // ReSharper disable once UnusedMember.Local
  private void On_Holes_Victory() => ChangeState(LevelState.Complete); // Level is complete!
}
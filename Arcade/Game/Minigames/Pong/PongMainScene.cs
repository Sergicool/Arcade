using Godot;
using System;
using System.ComponentModel.Design;
using static Godot.SpringBoneSimulator3D;

public partial class PongMainScene : Node
{
    public int BestOf;
    public bool Singleplayer;

    private PackedScene _playerScene = GD.Load<PackedScene>("res://Minigames/Pong/Entities/Player/Player.tscn");
    private PackedScene _botScene = GD.Load<PackedScene>("res://Minigames/Pong/Entities/Bot/PongBot.tscn");
    private PackedScene _ballScene = GD.Load<PackedScene>("res://Minigames/Pong/Entities/Ball/Ball.tscn");
    
    private Texture2D _player1Texture = GD.Load<Texture2D>("res://Minigames/Pong/Entities/Player/Player_1.png");
    private Texture2D _player2Texture = GD.Load<Texture2D>("res://Minigames/Pong/Entities/Player/Player_2.png");
    private Texture2D _pongBotTexture = GD.Load<Texture2D>("res://Minigames/Pong/Entities/Bot/PongBot.png");

    private AudioStreamPlayer _scorePointSFX;

    private PongPlayer _player1, _player2;
    private PongBot _pongBot;
    private PongBall _ball;

    [Export] private Vector2 _player1StartPosition = new Vector2(-160, 0);
    [Export] private Vector2 _player2StartPosition = new Vector2(160, 0);
    [Export] private Vector2 _ballStartPosition = Vector2.Zero;

    private int _scorePlayer1, _scorePlayer2;
    
    private Timer _startTime, _goalTime;
    private PongUI _pongUi;
    private EndGameUI _endGameUI;

    public override void _Ready()
    {
        if (BestOf < 1) BestOf = 1;

        _player1 = _playerScene.Instantiate<PongPlayer>();
        _player1.PlayerNumber = 1;
        _player1.SpriteTexture = _player1Texture;
        if (Singleplayer)
        {
            _pongBot = _botScene.Instantiate<PongBot>();
            _pongBot.PlayerNumber = 2;
            _pongBot.SpriteTexture = _pongBotTexture;
        }
        else
        {
            _player2 = _playerScene.Instantiate<PongPlayer>(); 
            _player2.PlayerNumber = 2;
            _player2.SpriteTexture = _player2Texture;
        }
        
        GetNode("./Entities").CallDeferred("add_child", _player1);
        if (Singleplayer)
            GetNode("./Entities").CallDeferred("add_child", _pongBot);
        else
            GetNode("./Entities").CallDeferred("add_child", _player2);

        _startTime = GetNode<Timer>("./Timers/StartTime");
        _goalTime = GetNode<Timer>("./Timers/GoalTime");
        _pongUi = GetNode<PongUI>("./PongUi");
        _endGameUI = GetNode<EndGameUI>("EndGameUI");

        _endGameUI.OnPlayAgain = () => RestartGame();
        _endGameUI.OnExit = () => GameManager.Instance.ReturnToMenu();

        _scorePointSFX = GetNode<AudioStreamPlayer>("ScorePointSfx");

        _scorePlayer1 = 0;
        _scorePlayer2 = 0;
        _pongUi.UpdateScores(_scorePlayer1, _scorePlayer2);

        StartGame();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("Pause"))
            GameManager.Instance.TogglePause();

        if (_startTime.TimeLeft > 0)
            _pongUi.ShowMessage("Starting in " + (int)(_startTime.TimeLeft + 1));
    }

    public void ManageGoal(int teamGoal)
    {
        _scorePointSFX.Play();
        _player1.CanMove = false;
        if (Singleplayer)
            _pongBot.CanMove = false;
        else
            _player2.CanMove = false;

        if (teamGoal == 1) _scorePlayer1++;
        if (teamGoal == 2) _scorePlayer2++;

        _pongUi.UpdateScores(_scorePlayer1, _scorePlayer2);
        _pongUi.ShowScores(true);
        _pongUi.ShowMessage("Goal from player " + teamGoal);

        _ball.CallDeferred("queue_free");

        bool hasWinPlayer1 = (_scorePlayer1 == BestOf);
        bool hasWinPlayer2 = (_scorePlayer2 == BestOf);

        if (hasWinPlayer1) { EndGame(1);  return; }
        if (hasWinPlayer2) { EndGame(2); return; }

        _goalTime.Start();
    }

    public void StartGame()
    {
        _startTime.Start();
        _player1.GlobalPosition = _player1StartPosition;
        if (Singleplayer)
            _pongBot.GlobalPosition = _player2StartPosition;
        else
            _player2.GlobalPosition = _player2StartPosition;
    }

    public void RestartGame()
    {
        _scorePlayer1 = 0;
        _scorePlayer2 = 0;
        _pongUi.UpdateScores(_scorePlayer1, _scorePlayer2);
        _pongUi.ShowScores(false);
        _pongUi.HideMessage();

        _player1.GlobalPosition = _player1StartPosition;
        _player1.CanMove = false;
        if (Singleplayer)
        {
            _pongBot.GlobalPosition = _player2StartPosition;
            _pongBot.CanMove = false;
        }
        else
        {
            _player2.GlobalPosition = _player2StartPosition;
            _player2.CanMove = false;
        }

        StartGame();
    }


    public void _on_start_time_timeout()
    {
        _pongUi.HideMessage();

        _ball = (PongBall)_ballScene.Instantiate();
        _ball.GlobalPosition = Vector2.Zero;
        _ball.ZIndex = 1;
        GetNode("./Entities").AddChild(_ball);

        _player1.CanMove = true;
        if (Singleplayer)
        {
            _pongBot.CanMove = true;
            _pongBot.SetBall(_ball);
        }
        else {
            _player2.CanMove = true;
        }
    }

    public void _on_goal_time_timeout()
    {
        _pongUi.ShowScores(false);
        StartGame();
    }


    public void EndGame(int winner)
    {
        string msg = "Player " + winner + " wins!";
        _endGameUI.ShowEndGame(msg, score: null);
    }
}

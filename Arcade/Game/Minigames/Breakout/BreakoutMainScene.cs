using Godot;
using System;

public partial class BreakoutMainScene : Node
{
    private int _columns = 16;
    private int _rows = 8;
    private int _totalBricks = 0;

    private int _score = 0;
    private int _lives = 3;

    private PackedScene _playerScene = GD.Load<PackedScene>("res://Minigames/Breakout/Entities/Player/Player.tscn");
    private PackedScene _brickScene = GD.Load<PackedScene>("res://Minigames/Breakout/Entities/Bricks/Brick.tscn");
    private PackedScene _ballScene = GD.Load<PackedScene>("res://Minigames/Breakout/Entities/Ball/Ball.tscn");

    private Texture2D _playerTexture = GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Player/Player.png");
    private Texture2D[] _brickTextures = new Texture2D[]
    {
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickRed.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickOrange.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickYellow.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickGreen.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickCeleste.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickBlue.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickPurple.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickPink.png")
    };

    private Node2D BricksGrid;
    private int _topLeftPositionX, _topLeftPositionY;
    [Export] private Vector2 _playerStartPosition = new Vector2(0, 140);
    [Export] private Vector2 _ballStartPosition = new Vector2(0, 133);
    private int[] _brickScores = { 70, 60, 50, 40, 30, 20, 10 };

    private BreakoutPlayer _player;
    private BreakoutBall _ball;

    private Timer _startTime;
    private BreakoutUI _breakoutUI;
    private EndGameUI _endGameUI;

    public override void _Ready()
    {
        _generateBrickLevel();

        _player = _playerScene.Instantiate<BreakoutPlayer>();
        _player.SpriteTexture = _playerTexture;
        GetNode("./Entities").CallDeferred("add_child", _player);

        _startTime = GetNode<Timer>("./Timers/StartTime");

        _breakoutUI = GetNode<BreakoutUI>("./BreakoutUI");
        _breakoutUI.UpdateScore(0);

        _endGameUI = GetNode<EndGameUI>("EndGameUI");

        _endGameUI.OnPlayAgain = () => RestartGame();
        _endGameUI.OnExit = () => GameManager.Instance.ReturnToMenu();

        StartGame();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("Pause"))
            GameManager.Instance.TogglePause();

        if (_startTime.TimeLeft > 0)
            _breakoutUI.ShowMessage("Starting in " + (int)(_startTime.TimeLeft + 1));
    }

    public void RestartGame()
    {
        _score = 0;
        _lives = 3;

        _breakoutUI.UpdateScore(_score);
        _breakoutUI.UpdateLiveCounter(_lives);

        foreach (Node child in BricksGrid.GetChildren())
            child.QueueFree();

        _generateBrickLevel();
        StartGame();
    }

    public void StartGame()
    {
        _startTime.Start();
        _player.GlobalPosition = _playerStartPosition;
    }

    public void BallLost()
    {
        _ball.CallDeferred("queue_free");
        _player.CanMove = false;
        if (_lives > 0)
        {
            _lives -= 1;
            _breakoutUI.UpdateLiveCounter(_lives);
            StartGame();
        }
        else
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        int bestScore = SaveManager.LoadStat(SaveManager.breakoutHighScore);

        if (_score > bestScore)
            SaveManager.SaveStat(SaveManager.breakoutHighScore, _score);

        _endGameUI.ShowEndGame("Game Over", score: _score);
    }

    public void AddScore(int value)
    {
        _ball.IncrementSpeed();
        _score += value;
        _breakoutUI.UpdateScore(_score);
        _totalBricks--;
        CheckWinCondition();
    }

    public void CheckWinCondition()
    {
        if (_totalBricks == 0)
        {
            if (_ball != null && GodotObject.IsInstanceValid(_ball))
            {
                _ball.CallDeferred("queue_free");
                _ball = null;
            }
            _player.CanMove = false;
            EndGame();
        }
    }

    public void _on_start_time_timeout()
    {
        _breakoutUI.HideMessage();

        _ball = (BreakoutBall)_ballScene.Instantiate();
        _ball.GlobalPosition = _ballStartPosition;
        _ball.ZIndex = 1;
        GetNode("./Entities").AddChild(_ball);

        _player.CanMove = true;
    }

    private void _generateBrickLevel()
    {
        BricksGrid = GetNode<Node2D>("BrickGrid");

        _topLeftPositionX = -((BreakoutBrick.Width * _columns) / 2) + (BreakoutBrick.Width / 2);
        _topLeftPositionY = -BreakoutBrick.Height * _rows;
        _totalBricks = 0;

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                if (row == 0)
                    continue;

                var brickInstance = _brickScene.Instantiate<BreakoutBrick>();

                float x = _topLeftPositionX + col * BreakoutBrick.Width;
                float y = _topLeftPositionY + row * BreakoutBrick.Height;
                brickInstance.Position = new Vector2(x, y);

                brickInstance.SpriteTexture = _brickTextures[(row - 1) % _brickTextures.Length];
                if (row != 0)
                    brickInstance.value = _brickScores[row - 1];

                BricksGrid.AddChild(brickInstance);
                _totalBricks++;
            }
        }
    }
}

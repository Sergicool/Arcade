using Godot;
using System;

public partial class BreakoutUI : CanvasLayer
{
    private Label _message, _scoreCount, _endGameScore;
    private Panel _endGamePanel;
    private Texture2D _live, _liveLost;
    private HBoxContainer _liveContainer;
    private AudioStreamPlayer _endGameSFX;

    BreakoutMainScene _breakoutMainScene;

    public override void _Ready()
    {
        _live = GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Ball/Ball.png");
        _liveLost = GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Ball/BallEmpty.png");
        _liveContainer = GetNode<HBoxContainer>("LiveContainer");
        _breakoutMainScene = GetNode<BreakoutMainScene>("/root/BreakoutMainScene");
        _message = GetNode<Label>("Message");
        _scoreCount = GetNode<Label>("ScoreMargin/ScoreCount");
        _endGameScore = GetNode<Label>("EndGamePanel/EndGameScore"); 
        _endGamePanel = GetNode<Panel>("EndGamePanel");
        _endGamePanel.Visible = false;
        _endGameSFX = GetNode<AudioStreamPlayer>("EndGame");
    }

    public void UpdateLiveCounter(int remainingLives)
    {
        for (int i = 0; i < _liveContainer.GetChildCount(); i++)
        {
            if (_liveContainer.GetChild(i) is TextureRect liveIcon)
            {
                liveIcon.Texture = (i < remainingLives) ? _live : _liveLost;
            }
        }
    }

    public void ShowMessage(string newMessage, bool visible = true)
    {
        _message.Text = newMessage;
        _message.Visible = visible;
    }

    public void HideMessage()
    {
        _message.Visible = false;
    }

    public void UpdateScore(int value)
    {
        _scoreCount.Text = "SCORE:" + value;
    }

    public void ShowEndGameMessage(int score)
    {
        _endGameSFX.Play();
        _endGameScore.Text = "SCORE " + score;
        _endGamePanel.Visible = true;
    }

    public void _on_play_again_pressed()
    {
        GameManager.Instance.CanPause = true;
        _endGamePanel.Visible = false;
        _breakoutMainScene.RestartGame();
    }

    public void _on_exit_pressed()
    {
        GameManager.Instance.CanPause = true;
        GameManager.Instance.ReturnToMenu();
    }
}

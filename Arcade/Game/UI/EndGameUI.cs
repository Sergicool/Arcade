using Godot;
using System;
using System.Diagnostics;

public partial class EndGameUI : CanvasLayer
{
    private Label _messageLabel, _scoreLabel, _timeLabel;
    private Panel _endGamePanel;
    private Button _playAgainButton, _exitButton;

    public Action OnPlayAgain;
    public Action OnExit;

    private AudioStreamPlayer _endGameSFX;

    public override void _Ready()
    {
        _endGamePanel = GetNode<Panel>("EndGamePanel");
        _messageLabel = GetNode<Label>("EndGamePanel/Message");
        _scoreLabel = GetNode<Label>("EndGamePanel/Score");
        _timeLabel = GetNode<Label>("EndGamePanel/Time");

        _playAgainButton = GetNode<Button>("EndGamePanel/PlayAgainButton");
        _exitButton = GetNode<Button>("EndGamePanel/ExitButton");

        _endGameSFX = GetNode<AudioStreamPlayer>("EndGame");

        _endGamePanel.Visible = false;
    }

    public void ShowEndGame(string message, int? score = null, double? time = null)
    {
        GameManager.Instance.CanPause = false;
        _endGamePanel.Visible = true;

        _messageLabel.Text = message;
        _scoreLabel.Text = score.HasValue ? $"Score: {score}" : "";
        _timeLabel.Text = time.HasValue ? $"Time: {Math.Round(time.Value, 2)}s" : "";

        _endGameSFX.Play();
    }

    public void HideEndGame()
    {
        GameManager.Instance.CanPause = true;
        _endGamePanel.Visible = false;
    }

    public void _on_play_again_pressed()
    {
        HideEndGame();
        OnPlayAgain?.Invoke();
    }

    public void _on_exit_pressed()
    {
        HideEndGame();
        OnExit?.Invoke();
        GameManager.Instance.ReturnToMenu();
    }
}

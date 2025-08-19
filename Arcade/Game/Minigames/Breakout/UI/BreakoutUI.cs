using Godot;
using System;

public partial class BreakoutUI : CanvasLayer
{
    private Label _message, _scoreCount, _endGameScore;
    private Panel _endGamePanel;

    BreakoutMainScene _breakoutMainScene;

    public override void _Ready()
    {
        _breakoutMainScene = GetNode<BreakoutMainScene>("/root/BreakoutMainScene");
        _message = GetNode<Label>("Message");
        _scoreCount = GetNode<Label>("ScoreMargin/ScoreCount");
        _endGameScore = GetNode<Label>("EndGamePanel/EndGameScore"); 
        _endGamePanel = GetNode<Panel>("EndGamePanel");
        _endGamePanel.Visible = false;
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

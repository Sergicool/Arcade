using Godot;
using System;

public partial class PongUI : CanvasLayer
{
    Panel PanelMessage;
    Label PlayerScore1, PlayerScore2, Message;

    public override void _Ready()
    {
        PlayerScore1 = GetNode<Label>("PlayerScore1");
        PlayerScore1.Visible = false;
        PlayerScore2 = GetNode<Label>("PlayerScore2");
        PlayerScore2.Visible = false;
        PanelMessage = GetNode<Panel>("PanelMessage");
        PanelMessage.Visible = false;
        Message = GetNode<Label>("PanelMessage/Message");
    }

    public void ShowMessage(string newMessage, bool visible = true)
    {
        Message.Text = newMessage;
        PanelMessage.Visible = visible;
    }

    public void HideMessage()
    {
        PanelMessage.Visible = false;
    }

    public void UpdateScores(int playerScore1, int playerScore2)
    {
        PlayerScore1.Text = playerScore1.ToString();
        PlayerScore2.Text = playerScore2.ToString();
    }

    public void ShowScores(bool visible = true)
    {
        PlayerScore1.Visible = visible;
        PlayerScore2.Visible = visible;
    }

}

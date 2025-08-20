using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
    private Label _breakout_highscore_label;
    private int _breakout_highscore = SaveManager.LoadStat(SaveManager.breakoutHighScore);
    public override void _Ready()
    {
        _breakout_highscore_label = GetNode<Label>("Breakout/VBox/Highscore");
        _breakout_highscore_label.Text = "Best Score: " + _breakout_highscore;
    }

    public void _on_1_player_button_pressed()
    {
        PongMainScene game = GameManager.Instance.LoadGame("Pong") as PongMainScene;
        if (game != null)
        {
            game.BestOf = 1;
            game.Singleplayer = true;
        }
    }

    public void _on_2_player_button_pressed()
    {
        PongMainScene game = GameManager.Instance.LoadGame("Pong") as PongMainScene;
        if (game != null)
        {
            game.BestOf = 3;
            game.Singleplayer = false;
        }
    }
    public void _on_play_brakeout_pressed()
    {
        BreakoutMainScene game = GameManager.Instance.LoadGame("BreakouT") as BreakoutMainScene;
    }
    
}

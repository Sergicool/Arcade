using Godot;
using System;

public partial class MainMenu : CanvasLayer
{
    public void _on_1_player_button_pressed()
    {
        PongMainScene game = GameManager.Instance.LoadGame("Pong") as PongMainScene;
        if (game != null)
        {
            game.BestOf = 2;
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
}

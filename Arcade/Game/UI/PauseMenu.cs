using Godot;
using System;

public partial class PauseMenu : CanvasLayer
{
    public override void _Ready()
    {
        ProcessMode = Node.ProcessModeEnum.Always; // Funciona aun con el arbol de escenas pausadas
        Visible = false;
    }

    public void TogglePause()
    {
        bool paused = !GetTree().Paused;
        GetTree().Paused = paused;
        Visible = paused;
    }

    public void _on_continue_button_pressed()
    {
        TogglePause();
    }

    public void _on_exit_button_pressed()
    {
        GetTree().Paused = false;
        GameManager.Instance.ReturnToMenu();
        Visible = false;
    }
}

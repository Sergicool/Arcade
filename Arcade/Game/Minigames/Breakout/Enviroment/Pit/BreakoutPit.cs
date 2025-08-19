using Godot;
using System;

public partial class BreakoutPit : Area2D
{
    private BreakoutMainScene _breakoutMainScene;

    public override void _Ready()
    {
        _breakoutMainScene = GetNode<BreakoutMainScene>("/root/BreakoutMainScene");
    }

    public void _on_body_entered(Node2D body)
    {
        _breakoutMainScene.Call("BallLost");
    }
}

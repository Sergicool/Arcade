using Godot;
using System;

public partial class Goal : Area2D
{
    [Export] public int teamGoal = 1;
    Node PongGameNode;
    public override void _Ready()
    {
        PongGameNode = GetNode<Node>("/root/PongGame");
    }

    public void _on_body_entered(Node2D body)
    {
        PongGameNode.Call("ManageGoal", [teamGoal]);
    }
}

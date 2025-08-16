using Godot;
using System;

public partial class Player : CharacterBody2D
{

    [Export] public int player = 1;
    [Export] public int speed = 140;
    public float height = 30;
    public Vector2 startPosition;
    public bool canMove;

    public override void _Ready()
    {
        startPosition = GlobalPosition;
        canMove = false;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!canMove) return;

        Vector2 moveDirection = Vector2.Zero;
        if (Input.IsActionPressed("PlayerUp" + player)) moveDirection.Y--;
        if (Input.IsActionPressed("PlayerDown" + player)) moveDirection.Y++;

        MoveAndCollide(moveDirection * speed * (float)delta);
    }

}

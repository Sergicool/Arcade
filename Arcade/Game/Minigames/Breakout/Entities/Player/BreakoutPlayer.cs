using Godot;
using System;

public partial class BreakoutPlayer : CharacterBody2D
{
    public Texture2D SpriteTexture;

    [Export] public int speed = 180;
    public float width = 30;
    public bool CanMove;

    public override void _Ready()
    {
        CanMove = false;
        if (SpriteTexture != null)
            GetNode<Sprite2D>("./Sprite").Texture = SpriteTexture;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!CanMove) return;

        Vector2 moveDirection = Vector2.Zero;
        if (Input.IsActionPressed("PlayerLeft1")) moveDirection.X--;
        if (Input.IsActionPressed("PlayerRight1")) moveDirection.X++;

        MoveAndCollide(moveDirection * speed * (float)delta);
    }
}

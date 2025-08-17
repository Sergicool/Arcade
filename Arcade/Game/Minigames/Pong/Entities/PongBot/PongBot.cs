using Godot;
using System;

public partial class PongBot : CharacterBody2D
{

    [Export] public int PlayerNumber = 1;
    public Texture2D SpriteTexture;

    [Export] public int speed = 140;
    public float height = 30;
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

        MoveAndCollide(moveDirection * speed * (float)delta);
    }

}

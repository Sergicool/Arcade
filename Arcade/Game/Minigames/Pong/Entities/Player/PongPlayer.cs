using Godot;
using System;

public partial class PongPlayer : CharacterBody2D
{

    [Export] public int PlayerNumber;
    public Texture2D SpriteTexture;

    [Export] public int speed = 140;
    public float height = 30;
    public bool CanMove;

    public override void _Ready()
    {
        CanMove = false;
        if(SpriteTexture != null)
            GetNode<Sprite2D>("./Sprite").Texture = SpriteTexture;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!CanMove) return;

        Vector2 moveDirection = Vector2.Zero;
        if (Input.IsActionPressed("PlayerUp" + PlayerNumber)) moveDirection.Y--;
        if (Input.IsActionPressed("PlayerDown" + PlayerNumber)) moveDirection.Y++;

        MoveAndCollide(moveDirection * speed * (float)delta);
    }

}

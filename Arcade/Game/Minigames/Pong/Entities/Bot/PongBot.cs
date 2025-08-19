using Godot;
using System;

public partial class PongBot : CharacterBody2D
{

    [Export] public int PlayerNumber = 1;
    public Texture2D SpriteTexture;
    private PongBall _ball;

    [Export] public int speed = 140;
    public float height = 30;
    public bool CanMove;

    public void SetBall(PongBall ball)
    {
        _ball = ball;
    }

    public override void _Ready()
    {
        CanMove = false;
        if (SpriteTexture != null)
            GetNode<Sprite2D>("./Sprite").Texture = SpriteTexture;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!CanMove) return;
        PongBall ball = GetNode<PongBall>("../Ball");

        Vector2 moveDirection = Vector2.Zero;
        if (_ball.GlobalPosition.Y < GlobalPosition.Y - height / 2)
            moveDirection.Y = -1;
        else if (_ball.GlobalPosition.Y > GlobalPosition.Y + height / 2)
            moveDirection.Y = 1;


        MoveAndCollide(moveDirection * speed * (float)delta);
    }

}

using Godot;
using System;
using static Godot.TextServer;

public partial class Ball : CharacterBody2D
{
    [Export] public float speed = 100f;
    [Export] public float speedIncrement = 10f;
    [Export] public float speedCap = 300f;
    [Export] public float maxBounceAngle = 50;

    public Vector2 Direction;

    public override void _Ready()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        float x, y;
        bool validNumber = false;
        while (!validNumber)
        {
            x = rng.RandfRange(-1, 1);
            y = rng.RandfRange(-1, 1);
            Direction = new Vector2(x, y);
            GD.Print(Direction);
            if (x <= -0.5 || x >= 0.5) validNumber = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        KinematicCollision2D collision = MoveAndCollide(Direction * speed * (float)delta);
        if (collision != null)
        {
            Vector2 newDirection = collision.GetNormal();
            GodotObject collider = collision.GetCollider();
            if (collider is Player player)
            {
                float offset = GlobalPosition.Y - player.GlobalPosition.Y;
                float normalizedOffset = offset / (player.height / 2);
                float bounceAngle = normalizedOffset * Mathf.DegToRad(maxBounceAngle);
                float dirX = MathF.Sign(Direction.X) * -1;
                Direction = new Vector2(Mathf.Cos(bounceAngle) * dirX, Mathf.Sin(bounceAngle)).Normalized();
                if (speed < speedCap)
                {
                    speed += speedIncrement;
                }
            }
            else
            {
                Direction = Direction.Bounce(newDirection);
            }
        }
    }
}

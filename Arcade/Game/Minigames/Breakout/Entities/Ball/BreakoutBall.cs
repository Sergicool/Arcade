using Godot;
using System;
using static Godot.TextServer;

public partial class BreakoutBall : CharacterBody2D
{
    [Export] public float speed = 100f;
    [Export] public float speedIncrement = 4f;
    [Export] public float speedCap = 400f;
    [Export] public float maxBounceAngle = 60;

    private Vector2 _direction;
    public override void _Ready()
    {
        RandomNumberGenerator rng = new RandomNumberGenerator();
        float x, y;
        x = rng.RandfRange(-1, 1);
        y = rng.RandfRange(-1, -0.8f);
        _direction = new Vector2(x, y);
    }

    public override void _PhysicsProcess(double delta)
    {
        KinematicCollision2D collision = MoveAndCollide(_direction * speed * (float)delta);
        if (collision != null)
        {
            Vector2 newDirection = collision.GetNormal();
            GodotObject collider = collision.GetCollider();

            if (collider is BreakoutPlayer player)
            {
                var body = (CharacterBody2D)collider;

                float offset = GlobalPosition.X - body.GlobalPosition.X;
                float normalizedOffset = offset / (player.width / 2);

                float bounceAngle = normalizedOffset * Mathf.DegToRad(maxBounceAngle);

                _direction = new Vector2(Mathf.Sin(bounceAngle), -Mathf.Cos(bounceAngle)).Normalized();
            }
            else if (collider is BreakoutBrick brick)
            {
                brick.OnHit();
                _direction = _direction.Bounce(newDirection);
            }
            else
            {
                _direction = _direction.Bounce(newDirection);
            }
        }
    }

    public void IncrementSpeed()
    {
        if (speed < speedCap)
            speed += speedIncrement;
    }
}

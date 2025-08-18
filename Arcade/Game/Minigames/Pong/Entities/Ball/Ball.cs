using Godot;
using System;
using static Godot.TextServer;

public partial class Ball : CharacterBody2D
{
    [Export] public float speed = 100f;
    [Export] public float speedIncrement = 15f;
    [Export] public float speedCap = 300f;
    [Export] public float maxBounceAngle = 50;

    private Vector2 _direction;
    private AudioStreamPlayer _hitWallSFX, _hitPlayerSFX;

    public override void _Ready()
    {
        _hitWallSFX = GetNode<AudioStreamPlayer>("HitWallSFX");
        _hitPlayerSFX = GetNode<AudioStreamPlayer>("HitPlayerSFX");
        RandomNumberGenerator rng = new RandomNumberGenerator();
        float x, y;
        bool validNumber = false;
        while (!validNumber)
        {
            x = rng.RandfRange(-1, 1);
            y = rng.RandfRange(-1, 1);
            _direction = new Vector2(x, y);
            if (x <= -0.5 || x >= 0.5) validNumber = true;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        KinematicCollision2D collision = MoveAndCollide(_direction * speed * (float)delta);
        if (collision != null)
        {
            Vector2 newDirection = collision.GetNormal();
            GodotObject collider = collision.GetCollider();

            if (collider is Player player || collider is PongBot bot)
            {
                var body = (CharacterBody2D)collider;
                _hitPlayerSFX.Play();

                float offset = GlobalPosition.Y - body.GlobalPosition.Y;
                float normalizedOffset = offset / (((dynamic)body).height / 2);
                float bounceAngle = normalizedOffset * Mathf.DegToRad(maxBounceAngle);

                float dirX = MathF.Sign(_direction.X) * -1;
                _direction = new Vector2(Mathf.Cos(bounceAngle) * dirX, Mathf.Sin(bounceAngle)).Normalized();

                if (speed < speedCap)
                    speed += speedIncrement;
            }
            else
            {
                _hitWallSFX.Play();
                _direction = _direction.Bounce(newDirection);
            }
        }
    }

}

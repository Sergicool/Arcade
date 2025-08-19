using Godot;
using System;

public partial class BreakoutBrick : CharacterBody2D
{
    public static int Width = 32;
    public static int Height = 10;
    public Texture2D SpriteTexture;
    public int value;

    BreakoutMainScene _breakoutMainScene;

    public override void _Ready()
    {
        _breakoutMainScene = GetNode<BreakoutMainScene>("/root/BreakoutMainScene");
        if (SpriteTexture != null)
            GetNode<Sprite2D>("./Sprite").Texture = SpriteTexture;
        if (value == 0)
            { value = 10; }
    }

    public void OnHit()
    {
        _breakoutMainScene.AddScore(value);
        QueueFree();
    }

}

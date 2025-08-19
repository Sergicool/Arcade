using Godot;
using System;

public partial class BreakoutBrick : CharacterBody2D
{
    public static int Width = 32;
    public static int Height = 10;
    public Texture2D SpriteTexture;
    public int value;

    public override void _Ready()
    {
        if (SpriteTexture != null)
            GetNode<Sprite2D>("./Sprite").Texture = SpriteTexture;
        if (value == 0)
            { value = 10; }
    }
}

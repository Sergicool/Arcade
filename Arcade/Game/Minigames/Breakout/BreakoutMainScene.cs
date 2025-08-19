using Godot;
using System;

public partial class BreakoutMainScene : Node
{
    private int _columns = 16;
    private int _rows = 8;

    private Node2D BricksGrid;
    private int _topLeftPositionX, _topLeftPositionY;
    private PackedScene _brickScene = GD.Load<PackedScene>("res://Minigames/Breakout/Entities/Bricks/Brick.tscn");

    private Texture2D[] _brickTextures = new Texture2D[]
    {
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickRed.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickOrange.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickYellow.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickGreen.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickCeleste.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickBlue.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickPurple.png"),
        GD.Load<Texture2D>("res://Minigames/Breakout/Entities/Bricks/BrickPink.png")
    };

    private int[] _brickScores = new int[] {0, 10, 20, 30, 40, 50, 60, 70 };

    public override void _Ready()
    {
        _generateBrickLevel();
    }

    private void _generateBrickLevel()
    {
        BricksGrid = GetNode<Node2D>("BrickGrid");

        _topLeftPositionX = -((BreakoutBrick.Width * _columns) / 2) + (BreakoutBrick.Width / 2);
        _topLeftPositionY = -BreakoutBrick.Height * _rows;

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _columns; col++)
            {
                if (row == 0)
                    continue;

                var brickInstance = _brickScene.Instantiate<BreakoutBrick>();

                float x = _topLeftPositionX + col * BreakoutBrick.Width;
                float y = _topLeftPositionY + row * BreakoutBrick.Height;
                brickInstance.Position = new Vector2(x, y);

                brickInstance.SpriteTexture = _brickTextures[(row - 1) % _brickTextures.Length];
                brickInstance.value = _brickScores[row];

                BricksGrid.AddChild(brickInstance);
            }
        }
    }
}

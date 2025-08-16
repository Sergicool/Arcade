using Godot;
using System;

public partial class PongGame : Node
{
    [Export] PackedScene ballScene;
    [Export] int bestOf = 1;

    Player player1, player2;
    Ball ball;
    Timer startTime, goalTime;
    PongUI PongUi;

    int scorePlayer1, scorePlayer2;

    public override void _Ready()
    {
        player1 = GetNode<Player>("./Entities/Player_1");
        player2 = GetNode<Player>("./Entities/Player_2");
        startTime = GetNode<Timer>("./Timers/StartTime");
        goalTime = GetNode<Timer>("./Timers/GoalTime");
        PongUi = GetNode<PongUI>("./PongUi");

        scorePlayer1 = 0;
        scorePlayer2 = 0;
        PongUi.UpdateScores(scorePlayer1, scorePlayer2);

        StartGame();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (startTime.TimeLeft > 0)
        {
            PongUi.ShowMessage("Starting in " + (int)(startTime.TimeLeft + 1));
        }
    }

    public void ManageGoal(int teamGoal)
    {
        player1.canMove = false;
        player2.canMove = false;

        if (teamGoal == 1) scorePlayer1++;
        if (teamGoal == 2) scorePlayer2++;

        PongUi.UpdateScores(scorePlayer1, scorePlayer2);
        PongUi.ShowScores(true);
        PongUi.ShowMessage("Goal from player " + teamGoal);

        ball.QueueFree();

        bool hasWinPlayer1 = (scorePlayer1 == bestOf);
        bool hasWinPlayer2 = (scorePlayer2 == bestOf);

        if (hasWinPlayer1) { EndGame(1);  return; }
        if (hasWinPlayer2) { EndGame(2); return; }

        goalTime.Start();
    }

    public void StartGame()
    {
        startTime.Start();
        player1.GlobalPosition = player1.startPosition;
        player2.GlobalPosition = player2.startPosition;
    }

    public void _on_start_time_timeout()
    {
        PongUi.HideMessage();

        ball = (Ball)ballScene.Instantiate();
        ball.GlobalPosition = Vector2.Zero;
        ball.ZIndex = 1;
        GetNode("./Entities").AddChild(ball);

        player1.canMove = true;
        player2.canMove = true;
    }

    public void _on_goal_time_timeout()
    {
        PongUi.ShowScores(false);
        StartGame();
    }

    public void EndGame(int winner)
    {
        PongUi.ShowMessage("Player " + winner + " win!");
    }

}

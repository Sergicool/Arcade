using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    private Node _currentScene;
    private PauseMenu _pauseMenuInstance;
    private string _mainMenuPath = "res://UI/MainMenu.tscn";
    private string _pauseMenuPath = "res://UI/PauseMenu.tscn";

    public override void _Ready()
    {
        if (Instance == null)
            Instance = this;
        else
            QueueFree();

        PackedScene pausePacked = GD.Load<PackedScene>(_pauseMenuPath);
        _pauseMenuInstance = pausePacked.Instantiate<PauseMenu>();
        GetTree().Root.CallDeferred("add_child", _pauseMenuInstance);
        _pauseMenuInstance.Visible = false;
    }

    public void TogglePause()
    {
        _pauseMenuInstance.TogglePause();
    }

    public void ReturnToMenu()
    {
        LoadMenu();
    }

    public void LoadMenu()
    {
        LoadScene(_mainMenuPath);
    }

    private void LoadScene(string path)
    {
        if (_currentScene != null)
            _currentScene.QueueFree();

        PackedScene packed = GD.Load<PackedScene>(path);
        _currentScene = packed.Instantiate();

        GetTree().Root.CallDeferred("add_child", _currentScene);
    }

    // Same as LoadScene, but returns the game scene to pass parameters
    public Node LoadGame(string name)
    {
        if (_currentScene != null)
            _currentScene.QueueFree();

        string path = $"res://Minigames/{name}/{name}MainScene.tscn";
        PackedScene packed = GD.Load<PackedScene>(path);
        Node instance = packed.Instantiate();

        _currentScene = instance;

        GetTree().Root.CallDeferred("add_child", _currentScene);

        return instance;
    }

}

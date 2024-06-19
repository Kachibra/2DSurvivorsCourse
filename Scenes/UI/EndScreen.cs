using Godot;
using System;

public partial class EndScreen : CanvasLayer
{
	private Button _restartButton;
	private Button _quitButton;
	public override void _Ready()
	{
		GetTree().Paused = true;
		_restartButton = GetNode<Button>("%RestartButton");
		_quitButton = GetNode<Button>("%QuitButton");

		_restartButton.Pressed += OnRestartButtonPressed;
		_quitButton.Pressed += OnQuitButtonPressed;
	}
    public override void _ExitTree()
    {
        _restartButton.Pressed -= OnRestartButtonPressed;
        _quitButton.Pressed -= OnQuitButtonPressed;
    }

    public void SetDefeat()
	{
		GetNode<Label>("%TitleLabel").Text = "Defeat";
		GetNode<Label>("%DescriptionLabel").Text = "You Lost.";
	}

	private void OnRestartButtonPressed()
	{
        GetTree().Paused = false;
        GetTree().ChangeSceneToFile("res://Scenes/Main/Main.tscn");

    }

    private void OnQuitButtonPressed()
	{
		GetTree().Quit();
	}

}

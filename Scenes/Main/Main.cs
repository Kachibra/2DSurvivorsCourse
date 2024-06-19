using Godot;
using System;

public partial class Main : Node
{
	[Export] PackedScene _endScreenScene;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GetNode<Player>("%Player").HealthComponent.Died += OnPlayerDied;
	}

    public override void _ExitTree()
    {
		_endScreenScene = null;
    }

    private void OnPlayerDied()
	{
		EndScreen endScreenInstance = _endScreenScene.Instantiate() as EndScreen;

		if (endScreenInstance == null)
		{
			return;
		}

		AddChild(endScreenInstance);
		endScreenInstance.SetDefeat();
	}
}

using Godot;
using System;

public partial class ExperienceVial : Node2D
{
	[Export] private int _experienceValue = 1;
	public override void _Ready()
	{
        GetNode<Area2D>("Area2D").AreaEntered += OnAreaEntered;
    }

    public override void _ExitTree()
    {
        GetNode<Area2D>("Area2D").AreaEntered -= OnAreaEntered;
    }

    private void OnAreaEntered(Area2D otherArea)
	{
		GetNode<GameEvents>("/root/GameEvents").EmitExperienceVialCollected(_experienceValue);
		QueueFree();
	}
}

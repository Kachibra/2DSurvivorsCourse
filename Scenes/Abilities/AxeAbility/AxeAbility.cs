using Godot;
using System;

public partial class AxeAbility : Node2D
{
	public HitboxComponent HitboxComponent { get; private set; }
	
	private const int MaxRadius = 75;

	private Vector2 _baseRotation = Vector2.Right;
	public override void _Ready()
	{
		_baseRotation = Vector2.Right.Rotated((float)GD.RandRange(0, Math.Tau));

		var tween = CreateTween();
		tween.TweenMethod(Callable.From<float>(TweenMethod), 0.0f, 2.0f, 3.0f);
		tween.TweenCallback(Callable.From(QueueFree));

		HitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
	}


	private void TweenMethod(float rotations)
	{
		float percent = rotations / 2;
		float currentRadius = percent * MaxRadius;
		Vector2 currentDirection = _baseRotation.Rotated(rotations * Mathf.Tau);

		Node2D player = GetTree().GetFirstNodeInGroup("player") as Node2D;

		if (player == null) 
		{
			return;
		}

		GlobalPosition = player.GlobalPosition + (currentDirection * currentRadius);
	}
}

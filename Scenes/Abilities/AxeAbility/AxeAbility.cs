using Godot;
using System;

public partial class AxeAbility : Node2D
{
	public HitboxComponent HitboxComponent { get; private set; }

    private const int MaxRadius = 75;
    private const float MaxRotations = 4.0f;

	private Vector2 _baseRotation = Vector2.Right;
    private float _waitTime = 6.0f;

    public void Initialize(float waitTime)
    {
        _waitTime = waitTime;
    }
	public override void _Ready()
	{
		_baseRotation = Vector2.Right.Rotated((float)GD.RandRange(0, Math.Tau));

		var tween = CreateTween();
		tween.TweenMethod(Callable.From<float>((float maxRotations) => TweenMethodBoth(maxRotations, MaxRotations)), 0.0f, MaxRotations, _waitTime);
		//tween.TweenMethod(Callable.From<float>(TweenMethodBackup), 0.0f, 2.0f, 3.0f);
        tween.TweenCallback(Callable.From(QueueFree));

        HitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
	}


    private void TweenMethodBoth(float rotations, float maxRotations)
    {
        float percent;
        float currentRadius;
        Vector2 currentDirection;

        if (rotations <= maxRotations/2)
        {
            percent = rotations / (maxRotations / 2);
            currentRadius = percent * MaxRadius;
            currentDirection = _baseRotation.Rotated(rotations * Mathf.Tau);
        }
        else
        {
            percent = (rotations - maxRotations) / (maxRotations / 2);
            currentRadius = percent * MaxRadius;
            Vector2 inRotation = _baseRotation.Rotated(Mathf.Pi);
            currentDirection = inRotation.Rotated((rotations - maxRotations) * Mathf.Tau);
        }

        Node2D player = GetTree().GetFirstNodeInGroup("player") as Node2D;

        if (player == null)
        {
            return;
        }

        GlobalPosition = player.GlobalPosition + (currentDirection * currentRadius);
    }

    /*
    private void TweenMethodBackup(float rotations)
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
    */

}

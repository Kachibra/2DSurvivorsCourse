using Godot;
using System;

public partial class GameCamera : Camera2D
{
	private Vector2 targetPosition = Vector2.Zero; // The target position the camera will move towards

	public override void _Ready()
	{
		MakeCurrent(); // Assign this camera as the current camera
	}

	public override void _Process(double delta)
	{
		AquireTarget();
		GlobalPosition = GlobalPosition.Lerp(targetPosition, 1.0f - Mathf.Exp(Convert.ToSingle(-delta) * 20.0f)); // Lerp camera GlobalPosition towards player GlobalPosition
	}

	// Get player GlobalPosition as targetPosition
	public void AquireTarget()
	{
        var playerNodes = GetTree().GetNodesInGroup("player"); // Get player nodes in group player
        if (playerNodes.Count > 0) // Make sure there is a player in the scene tree
        {
            var player = playerNodes[0] as Node2D; // get the first player in playerNodes
            targetPosition = player.GlobalPosition; // set target position to player GlobalPosition
        }
    }
}

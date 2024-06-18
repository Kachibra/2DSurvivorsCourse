using Godot;
using System;

public partial class BasicEnemy : CharacterBody2D
{
	private const int MaxSpeed = 50; // Maximum speed of the enemy
	private const float accelerationMultiplier = 2f; // Acceleration multiplier of enemy

    private HealthComponent _healthComponent;

    public override void _Ready()
	{
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
    }

	public override void _Process(double delta) 
	{
        Vector2 direction = GetDirectionToPlayer(); // Call function GetDirectionToPlayer

		var targetVelocity = (direction.Normalized() * MaxSpeed);
        Velocity = Velocity.Lerp(targetVelocity, 1 - Mathf.Exp((float)-delta * accelerationMultiplier)); // Calculate Velocity with lerp https://www.rorydriscoll.com/2016/03/07/frame-rate-independent-damping-using-lerp/

        MoveAndSlide(); // Move enemy an slide on collision
	}

	// Method returns the direction from this enemy and the player.
	public Vector2 GetDirectionToPlayer()
	{
		Node2D playerNode = GetTree().GetFirstNodeInGroup("player") as Node2D; // get player node in scene tree
		if (playerNode != null) // Make sure that a player exists
		{
			return (playerNode.GlobalPosition - GlobalPosition).Normalized(); // Return a Vector2 as a result of the players GlobalPostion
																			  // subtracted by this enemies GlobalPosition Normalized
		}
		return Vector2.Zero; // Return Zero if there is no player in the scene tree
	}
}

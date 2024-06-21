using Godot;
using Godot.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class SwordAbilityController : Node
{
	private const int MaxRange = 150; // constant integer represents the max range from the player a sword ability can activate

	[Export] PackedScene _swordAbility = new PackedScene(); // Add swordAbility as a scene that can be instantiated

    private int _damage = 4;
	private double _baseWaitTime;

	public override void _Ready()
	{
		//Timer timer = GetNode<Timer>("Timer");
		//timer.Timeout += OnTimerTimeout;

		_baseWaitTime = GetNode<Timer>("Timer").WaitTime;

		GetNode<Timer>("Timer").Timeout += OnTimerTimeout; // Connect child node signal Timer.Timeout to this Node
		GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded += OnAbilityUpgradeAdded;
	}

    public override void _ExitTree()
    {
        GetNode<GameEvents>("/root/GameEvents").AbilityUpgradeAdded -= OnAbilityUpgradeAdded;
		_swordAbility = null;
    }

    private void OnTimerTimeout() //Signal from Timer Node
    {
        Node2D player = GetTree().GetFirstNodeInGroup("player") as Node2D; // Access Class/Node manager with GetTree and get the first node in group named player (as Node2D, default Node).
		if (player == null) // Make sure player is not a null refrence.
		{
			return;
		}

		List<Node> enemies = GetTree().GetNodesInGroup("enemy").ToList(); // Get All nodes in sceene tree in group enemies (Node) and create an new generic list of type Node.
        List<Node2D> sortedEnemies = enemies.ConvertAll(enemy => (Node2D)enemy) // Convert all Node items in enemies to Node2D.
			.Where(enemy => enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition) < Mathf.Pow(MaxRange, 2)).ToList(); // Then preform a query asking if the enemy location is within max range.

		if (sortedEnemies.Count == 0) // Return if No enemy is within MaxRange of player
		{
			return;
		}

		List<Node2D> closestEnemies = sortedEnemies.OrderBy(enemy => enemy.GlobalPosition.DistanceSquaredTo(player.GlobalPosition)).ToList(); // If there are enemies within the max range of the player, find the enemy that is closest to the player.

        SwordAbility swordInstance = _swordAbility.Instantiate() as SwordAbility; // Create a new sword instance in memory
		Node foregroundLayer = GetTree().GetFirstNodeInGroup("foregroundLayer");

        if (swordInstance == null) // Check to make sure there is a swordInstance
        {
            return;
        }


        foregroundLayer.AddChild(swordInstance); // Add swordInstance to the parent (main) node as a child
		swordInstance.HitboxComponent.Damage = _damage;
        swordInstance.GlobalPosition = closestEnemies[0].GlobalPosition; // get the first item (closest enemy) from closestEnemies
		swordInstance.GlobalPosition += Vector2.Right.Rotated((float)GD.RandRange(0f, Mathf.Tau)) * 4; // Set GlobalPosition of swordInstance to a random point on the circumfrence of a circle with an origin on the enemy

		var enemyDirection = closestEnemies[0].GlobalPosition - swordInstance.GlobalPosition; // Get the direction of the enemy from updated swordIntance GlobalPosition
		swordInstance.Rotation = enemyDirection.Angle(); // Rotate swordInstance

        //swordInstance.GetNode<AnimationPlayer>("AnimationPlayer").Play("swing");
	}

	private void OnAbilityUpgradeAdded(AbilityUpgrade upgrade, Godot.Collections.Dictionary<string, Dictionary> currentUpgrades)
	{
		if (upgrade.Id != "swordRate")
		{
			return;
		}

		double percentReduction = (double)currentUpgrades["swordRate"]["quantity"] * 0.1;
		//GD.Print(percentReduction);
		GetNode<Timer>("Timer").WaitTime = Math.Max(_baseWaitTime * (1 - percentReduction), 0.1);
        GetNode<Timer>("Timer").Start();

    }

}

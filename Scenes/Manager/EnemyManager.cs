using DSurvivors.scenes.manager;
using Godot;
using Godot.Collections;
using System;
using System.ComponentModel.Design;

public partial class EnemyManager : Node
{
	private const int SpawnerRadius = 400; // Range around player that an enemy can spawn

	[Export] PackedScene _basicEnemyScene = new PackedScene(); //Inherits packed scene Basic Enemy
	[Export] ArenaTimeManager _arenaTimeManager;

	private Timer _enemySpawnTimer;

	private double _baseSpawnTime;


	public override void _Ready()
	{
		_enemySpawnTimer = GetNode<Timer>("Timer");

        _baseSpawnTime = _enemySpawnTimer.WaitTime;

        _enemySpawnTimer.Timeout += OnTimerTimeout; //Connects timer.timeout to this node
		_arenaTimeManager.ArenaDifficultyIncreased += OnArenaDifficultyIncreased;
	}

	private Vector2 GetSpawnPosition()
	{
        Node2D player = GetTree().GetFirstNodeInGroup("player") as Node2D; // get player object in scene tree
        if (player == null) // return if there is no player in the scene tree
        {
            return Vector2.Zero;
        }

        Vector2 spawnPosition = Vector2.Zero;
        Vector2 randomDirection = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau)); // get random direction 2PI around the player

        for (int i = 0; i < 4; i++)
        {
            spawnPosition = player.GlobalPosition + (randomDirection * SpawnerRadius); // get a spawnPosition in a random direction a certain distance away from the player.

            PhysicsRayQueryParameters2D queryParameters = PhysicsRayQueryParameters2D.Create(player.GlobalPosition, spawnPosition, 1);
            Dictionary result = GetTree().Root.World2D.DirectSpaceState.IntersectRay(queryParameters);

            if (result.Count == 0)
            {
                break;
            } else
            {
                randomDirection = randomDirection.Rotated((float)(Math.PI / 180) * 90f);
            }
        }

        return spawnPosition;
    }

    private void OnTimerTimeout()
	{
		_enemySpawnTimer.Start();
		
		Node2D enemyInstance = _basicEnemyScene.Instantiate() as Node2D; // create an enemyInstance in memory
        if (enemyInstance == null) // make sure there is an enemyInstance in memory
        {
            return;
        }

		Node entitiesLayer = GetTree().GetFirstNodeInGroup("entitiesLayer");
        entitiesLayer.AddChild(enemyInstance); // Add enemyInstance to the scene tree
        enemyInstance.GlobalPosition = GetSpawnPosition(); // set the GlobalPosition of the new enemy.
    }

	private void OnArenaDifficultyIncreased(int arenaDifficulty)
	{
		double timeOff = (0.1 / 12) * arenaDifficulty; // enemy spawn wait time reduced by 0.1 every minute. Interval depends on arenaDifficulty.
		timeOff = Math.Min(timeOff, .7);
		_enemySpawnTimer.WaitTime = _baseSpawnTime - timeOff;
        GD.Print(_enemySpawnTimer.WaitTime);
    }
}

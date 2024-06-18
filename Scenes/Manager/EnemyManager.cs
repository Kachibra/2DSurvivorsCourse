using DSurvivors.scenes.manager;
using Godot;
using System;

public partial class EnemyManager : Node
{
	private const int SpawnerRadius = 350; // Range around player that an enemy can spawn

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

	private void OnTimerTimeout()
	{
		_enemySpawnTimer.Start();

		var player = GetTree().GetFirstNodeInGroup("player") as Node2D; // get player object in scene tree
        if (player == null) // return if there is no player in the scene tree
        {
			return;
        }

		var randomDirection = Vector2.Right.Rotated((float)GD.RandRange(0, Mathf.Tau)); // get random direction 2PI around the player
		var spawnPosition = player.GlobalPosition + (randomDirection * SpawnerRadius); // get a spawnPosition in a random direction a certain distance away from the player.

		var enemyInstance = _basicEnemyScene.Instantiate() as Node2D; // create an enemyInstance in memory
        if (enemyInstance == null) // make sure there is an enemyInstance in memory
        {
            return;
        }

		var entitiesLayer = GetTree().GetFirstNodeInGroup("entitiesLayer");
        entitiesLayer.AddChild(enemyInstance); // Add enemyInstance to the scene tree
        enemyInstance.GlobalPosition = spawnPosition; // set the GlobalPosition of the new enemy.
    }

	private void OnArenaDifficultyIncreased(int arenaDifficulty)
	{
		double timeOff = (0.1 / 12) * arenaDifficulty;
		timeOff = Math.Min(timeOff, .7);
		_enemySpawnTimer.WaitTime = _baseSpawnTime - timeOff;
        GD.Print(_enemySpawnTimer.WaitTime);
    }
}

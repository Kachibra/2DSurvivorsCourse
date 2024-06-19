using Godot;
using System;

public partial class AxeAbilityController : Node
{
    [Export] PackedScene _axeAbilityScene;

    private Timer _axeSpawnTimer;
    private int _damage = 10;
    public override void _Ready()
    {
        _axeSpawnTimer = GetNode<Timer>("Timer");
        _axeSpawnTimer.Timeout += OnAxeSpawnTimerTimeout;
    }

    public override void _ExitTree()
    {
        _axeAbilityScene = null;
    }


    private void OnAxeSpawnTimerTimeout()
    {
        Node2D player = GetTree().GetFirstNodeInGroup("player") as Node2D;

        if (player == null) 
        {
            return;
        }

        Node foregroundLayer = GetTree().GetFirstNodeInGroup("foregroundLayer");

        if (foregroundLayer == null) // Check to make sure there is a swordInstance
        {
            return;
        }

        AxeAbility axeAbilityInstance = _axeAbilityScene.Instantiate() as AxeAbility;

        if (axeAbilityInstance == null) // Check to make sure there is a swordInstance
        {
            return;
        }

        foregroundLayer.AddChild(axeAbilityInstance);
        axeAbilityInstance.HitboxComponent.Damage = _damage;
        axeAbilityInstance.GlobalPosition = player.GlobalPosition;
    }
}

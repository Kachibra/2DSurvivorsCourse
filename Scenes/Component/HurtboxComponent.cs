using Godot;
using System;

public partial class HurtboxComponent : Area2D
{
    [Export] HealthComponent _healthComponent;
    public override void _Ready()
    {
        AreaEntered += OnAreaEntered;
    }

    public override void _ExitTree()
    {
        AreaEntered -= OnAreaEntered;
    }

    public void OnAreaEntered(Area2D otherArea)
    {
        if (otherArea is not HitboxComponent) 
        {
            return;
        }

        if (_healthComponent == null ) 
        {
            return;
        }

        HitboxComponent hitboxComponent = (HitboxComponent)otherArea;
        _healthComponent.Damage(hitboxComponent.Damage);
    }
}

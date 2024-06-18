using Godot;
using System.Runtime.CompilerServices;

public partial class VialDropComponent : Node
{
	[Export(PropertyHint.Range, "0,1,")] private float _dropPercent = 0.5f;
	[Export] private HealthComponent _healthComponent;
    [Export] private PackedScene _vialScene;
    public override void _Ready()
	{
		_healthComponent.Died += OnDied;
	}

	private void OnDied()
	{
		if (GD.Randf() > _dropPercent)
		{
			return;
		}

		if (_vialScene == null) 
		{
			return;
		}

        if (Owner is not Node2D)
		{
			return;
		}

        var spawnPosition = (Owner as Node2D).GlobalPosition;
		var vialInstance = _vialScene.Instantiate() as Node2D;

		if (vialInstance == null)
		{
			return;
		}

		vialInstance.GlobalPosition = spawnPosition;
        var entitiesLayer = GetTree().GetFirstNodeInGroup("entitiesLayer");
        entitiesLayer.AddChild(vialInstance);
	}
}

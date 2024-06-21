using Godot;
using Godot.Collections;
using System;

public partial class UpgradeScreen : CanvasLayer
{
	[Signal] public delegate void UpgradeSelectedEventHandler(AbilityUpgrade upgrade);

	[Export] private PackedScene _upgradeCardScene = new PackedScene();
	private HBoxContainer _cardContainer;
	public override void _Ready()
	{
		_cardContainer = GetNode<HBoxContainer>("%CardContainer");
		GetTree().Paused = true;
	}

    public override void _ExitTree()
    {
        _upgradeCardScene = null;
    }

    public void SetAbilityUpgrades(Array<AbilityUpgrade> upgrades)
	{
        foreach (AbilityUpgrade upgrade in upgrades)
		{
			AbilityUpgradeCard cardInstance = _upgradeCardScene.Instantiate() as AbilityUpgradeCard;

			if (cardInstance == null)
			{
				continue;
			}

			_cardContainer.AddChild(cardInstance);
			cardInstance.SetAbilityUpgrade(upgrade);
			cardInstance.AbilitySelected += () => OnAbilitySelected(upgrade);
		}
	}

	private void OnAbilitySelected(AbilityUpgrade upgrade)
	{
		EmitSignal(SignalName.UpgradeSelected, upgrade);
		GetTree().Paused = false;
		QueueFree();
	}
}

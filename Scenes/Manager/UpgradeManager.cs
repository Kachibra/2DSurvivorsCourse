using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class UpgradeManager : Node
{
	[Export] private Array<AbilityUpgrade> _upgrade_pool = new Array<AbilityUpgrade>();
	[Export] private ExperienceManager _experienceManager;
	[Export] private PackedScene _upgradeScreenScene = new PackedScene();

	private Dictionary<string, Dictionary> _currentUpgrades = new Dictionary<string, Dictionary>();

    public override void _Ready()
	{
		_experienceManager.LevelUp += OnLevelUp;
    }

	private void OnLevelUp(int currentLevel)
	{
        AbilityUpgrade chosenUpgrade = _upgrade_pool.PickRandom();
        AbilityUpgrade[] chosenUpgrades =  new AbilityUpgrade[] { chosenUpgrade };

        if (chosenUpgrade == null)
        {
            return;
        }

        UpgradeScreen upgradeScreenInstance = _upgradeScreenScene.Instantiate() as UpgradeScreen;
        AddChild(upgradeScreenInstance);
        upgradeScreenInstance.SetAbilityUpgrades(chosenUpgrades);
        upgradeScreenInstance.UpgradeSelected += OnUpgradeSelected;
    }

	private void ApplyUpgrade(AbilityUpgrade upgrade)
	{
        var hasUpgrade = _currentUpgrades.ContainsKey(upgrade.Id);
        if (hasUpgrade == false)
        {
            _currentUpgrades.Add(upgrade.Id, new Dictionary { { "resource", upgrade }, { "quantity", 1 } });
        }
        else
        {
            _currentUpgrades[upgrade.Id]["quantity"] = (int)_currentUpgrades[upgrade.Id]["quantity"] + 1;
        }

        GetNode<GameEvents>("/root/GameEvents").EmitAbilityUpgradeAdded(upgrade, _currentUpgrades);
        GD.Print(_currentUpgrades);
    }

    private void OnUpgradeSelected(AbilityUpgrade upgrade)
    {
        ApplyUpgrade(upgrade);
    }
}

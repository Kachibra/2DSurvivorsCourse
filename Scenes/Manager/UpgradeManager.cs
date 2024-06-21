using Godot;
using Godot.Collections;
using System;
using System.Linq;

public partial class UpgradeManager : Node
{
	[Export] private Array<AbilityUpgrade> _upgradePool = new Array<AbilityUpgrade>();
	[Export] private ExperienceManager _experienceManager;
	[Export] private PackedScene _upgradeScreenScene = new PackedScene();

	private Dictionary<string, Dictionary> _currentUpgrades = new Dictionary<string, Dictionary>();

    public override void _Ready()
	{
		_experienceManager.LevelUp += OnLevelUp;
    }

    public override void _ExitTree()
    {
        _experienceManager.LevelUp -= OnLevelUp;
        _upgradeScreenScene = null;
    }

	private void ApplyUpgrade(AbilityUpgrade upgrade)
	{
        bool hasUpgrade = _currentUpgrades.ContainsKey(upgrade.Id);
        if (hasUpgrade == false)
        {
            _currentUpgrades.Add(upgrade.Id, new Dictionary { { "resource", upgrade }, { "quantity", 1 } });
        }
        else
        {
            _currentUpgrades[upgrade.Id]["quantity"] = (int)_currentUpgrades[upgrade.Id]["quantity"] + 1;
        }

        if (upgrade.MaxQuantity > 0)
        {
            int currentQuantity = (int)_currentUpgrades[upgrade.Id]["quantity"];
            if (currentQuantity == upgrade.MaxQuantity)
            {
                _upgradePool = FilterByUpgradeId(_upgradePool, upgrade.Id);
            }
        }

        GetNode<GameEvents>("/root/GameEvents").EmitAbilityUpgradeAdded(upgrade, _currentUpgrades);
        GD.Print(_currentUpgrades);
    }

    private Array<AbilityUpgrade> FilterByUpgradeId(Array<AbilityUpgrade> upgrades, string upgradeId)
    {
        Array<AbilityUpgrade> filteredUpgrades = new Array<AbilityUpgrade>();

        for (int i = 0; i < upgrades.Count; i++)
        {
            if (upgrades[i].Id != upgradeId)
            {
                filteredUpgrades.Add(upgrades[i]);
            }
        }
        
        return filteredUpgrades;
    }

    private Array<AbilityUpgrade> PickUpgrades()
    {

        Array<AbilityUpgrade> filteredUpgrades = _upgradePool.Duplicate();
        //GD.Print(filteredUpgrades);
        Array<AbilityUpgrade> chosenUpgrades = new Array<AbilityUpgrade>();

        for (int i = 0; i < 2; i++)
        {
            if (filteredUpgrades.Count == 0)
            {
                break;
            }

            AbilityUpgrade chosenUpgrade = filteredUpgrades.PickRandom();
            chosenUpgrades.Add(chosenUpgrade);
            filteredUpgrades = FilterByUpgradeId(filteredUpgrades, chosenUpgrade.Id);
        }

        //GD.Print(filteredUpgrades);
        GD.Print(chosenUpgrades);

        return chosenUpgrades;
    }

    private void OnUpgradeSelected(AbilityUpgrade upgrade)
    {
        ApplyUpgrade(upgrade);
    }

    private void OnLevelUp(int currentLevel)
    {
        UpgradeScreen upgradeScreenInstance = _upgradeScreenScene.Instantiate() as UpgradeScreen;
        AddChild(upgradeScreenInstance);
        Array<AbilityUpgrade> chosenUpgrades = PickUpgrades();
        upgradeScreenInstance.SetAbilityUpgrades(chosenUpgrades);
        upgradeScreenInstance.UpgradeSelected += OnUpgradeSelected;
    }
}

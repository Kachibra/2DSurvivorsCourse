using Godot;
using Godot.Collections;
using System;

public partial class GameEvents : Node
{
    [Signal] public delegate void ExperienceVialCollectedEventHandler(float number);
    [Signal] public delegate void AbilityUpgradeAddedEventHandler(AbilityUpgrade upgrade, Dictionary<string, Dictionary> currentUpgrades);
    //[Signal] public delegate void EventHandler();
    public void EmitExperienceVialCollected(float number)
    {
        EmitSignal(SignalName.ExperienceVialCollected, number);
    }

    public void EmitAbilityUpgradeAdded(AbilityUpgrade upgrade, Dictionary<string, Dictionary> currentUpgrades)
    {
        EmitSignal(SignalName.AbilityUpgradeAdded, upgrade, currentUpgrades);
    }

}

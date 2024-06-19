using Godot;
using System;

public partial class ExperienceManager : Node
{
	[Signal] public delegate void ExperienceUpdatedEventHandler(float currentExperience, float targetExperience);
	[Signal] public delegate void LevelUpEventHandler(int newLevel);

	private const float TargetExperienceGrowth = 5;

    [Export] private float _targetExperience = 5;
    private float _currentExperience = 0;
	private int _currentLevel = 1;
	public override void _Ready()
	{
		GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected += OnExperienceVialCollected;
	}

	public override void _ExitTree()
	{
        GetNode<GameEvents>("/root/GameEvents").ExperienceVialCollected -= OnExperienceVialCollected;
    }

    private void IncrementExperience(float number)
	{
		_currentExperience = Mathf.Min(_currentExperience + number, _targetExperience);
		EmitSignal(SignalName.ExperienceUpdated, _currentExperience, _targetExperience);
		//GD.Print(_currentExperience + ", " + _targetExperience);
		if (_currentExperience == _targetExperience)
		{
			_currentLevel += 1;
			_targetExperience += TargetExperienceGrowth;
			_currentExperience = 0;
            EmitSignal(SignalName.ExperienceUpdated, _currentExperience, _targetExperience);
			EmitSignal(SignalName.LevelUp, _currentLevel); // UpgradeManager
        }
	}

	private void OnExperienceVialCollected(float number)
	{
		IncrementExperience(number);
    }
}

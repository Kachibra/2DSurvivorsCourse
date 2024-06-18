using Godot;
using System;

public partial class HealthComponent : Node
{
	[Signal] public delegate void DiedEventHandler();
	[Signal] public delegate void HealthChangedEventHandler();
	[Export] private float _maxHealth = 10;
	private float _currentHealth;

	public override void _Ready()
	{
		_currentHealth = _maxHealth;
	}

	public void Damage(float damageAmount)
	{
		_currentHealth = Mathf.Max(_currentHealth - damageAmount, 0);
		EmitSignal(SignalName.HealthChanged);
		Callable.From(CheckDeath).CallDeferred();
		
	}

	public float GetHealthPercent()
	{
		if (_maxHealth <= 0)
		{
			return 0;
		}
		return Mathf.Min(_currentHealth / _maxHealth, 1);
	}

	private void CheckDeath()
	{
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            EmitSignal(SignalName.Died);
            Owner.QueueFree();
        }
    }
}

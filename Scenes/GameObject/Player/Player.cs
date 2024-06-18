using Godot;
using System;

public partial class Player : CharacterBody2D
{
	// Take note down. Very nice.
    public HealthComponent HealthComponent => _healthComponent;

    private const int MaxSpeed = 125; // MaxSpeed of player
	private const int AccelerationSmoothing = 25; // Acceleration smoothing multiplier for player movement

	Timer _damageIntervalTimer;
    private HealthComponent _healthComponent;
    private int _numberCollidingBodies = 0;
	private ProgressBar _healthBar;

	public override void _Ready()
	{
		_damageIntervalTimer = GetNode<Timer>("DamageIntervalTimer");
		_healthComponent = GetNode<HealthComponent>("HealthComponent");
		_healthBar = GetNode<ProgressBar>("HealthBar");

		GetNode<Area2D>("CollisionArea2D").BodyEntered += OnBodyEntered;
		GetNode<Area2D>("CollisionArea2D").BodyExited += OnBodyExited;
		_damageIntervalTimer.Timeout += OnDamageIntervalTimerTimeout;
		_healthComponent.HealthChanged += OnHealthChanged;

		UpdateHealthDisplay();
	}

	public override void _Process(double delta)
	{
		Vector2 movementVector = GetMovementVector();
		Vector2 direction = movementVector.Normalized(); // Normailize
		Vector2 targetVelocity = direction * MaxSpeed; // Set targetVelocity

		Velocity = Velocity.Lerp(targetVelocity, 1 - Mathf.Exp((float)-delta * AccelerationSmoothing));

		MoveAndSlide();
	}
	
	// Get movement vector directly from player input
	public Vector2 GetMovementVector()
	{
		Vector2 movementVector = Vector2.Zero;

		var xMovement = Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"); // Read inputs for x movement
		var yMovement = Input.GetActionStrength("move_down") - Input.GetActionStrength("move_up"); // Read inputs for y movement

        return new Vector2(xMovement, yMovement); // return a Vector2 containing the players input.
	}

	private void CheckDealDamage()
	{
		if (_numberCollidingBodies == 0 || !_damageIntervalTimer.IsStopped()) 
		{
			return;
		}

		_healthComponent.Damage(1);
		_damageIntervalTimer.Start();
	}

	private void UpdateHealthDisplay()
	{
        _healthBar.Value = _healthComponent.GetHealthPercent();
    }

    private void OnBodyEntered(Node2D otherBody)
	{
		_numberCollidingBodies += 1;
		CheckDealDamage();
	}

	private void OnBodyExited(Node2D otherBody)
	{
		_numberCollidingBodies -= 1;
	}

	private void OnDamageIntervalTimerTimeout()
	{
		CheckDealDamage();
	}

	private void OnHealthChanged()
	{
		UpdateHealthDisplay();
	}

}

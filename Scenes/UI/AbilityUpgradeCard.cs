using Godot;
using System;

public partial class AbilityUpgradeCard : PanelContainer
{
	[Signal] public delegate void AbilitySelectedEventHandler();

	private Label _nameLabel;
	private Label _descriptionLabel;
	public override void _Ready()
	{
		_nameLabel = GetNode<Label>("%NameLabel");
		_descriptionLabel = GetNode<Label>("%DescriptionLabel");

		GuiInput += OnGuiInput;
	}

	public void SetAbilityUpgrade(AbilityUpgrade upgrade)
	{
		_nameLabel.Text = upgrade.Name;
		_descriptionLabel.Text = upgrade.Description;
	}

	private void OnGuiInput(InputEvent inputEvent)
	{
		if (inputEvent.IsActionPressed("left_click"))
		{
			EmitSignal(SignalName.AbilitySelected);
		}
	}
}

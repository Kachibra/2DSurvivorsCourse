using Godot;
using System;

public partial class SwordAbility : Node2D
{
	public HitboxComponent HitboxComponent { get; set; }
	public override void _Ready()
	{
		HitboxComponent = GetNode<HitboxComponent>("HitboxComponent");
	}
}

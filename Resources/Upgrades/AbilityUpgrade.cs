using Godot;
using System;

public partial class AbilityUpgrade : Resource
{
	[Export] private string _id;
	[Export] private string _name;
	[Export(PropertyHint.MultilineText)] private string _description;

	public string Id => _id;
	public string Name => _name;
	public string Description => _description;

}

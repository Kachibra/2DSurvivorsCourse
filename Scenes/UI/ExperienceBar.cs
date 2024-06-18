using Godot;
using System;

public partial class ExperienceBar : CanvasLayer
{
	[Export] private ExperienceManager _experienceManager;

	private ProgressBar _progressBar;
	public override void _Ready()
	{
		_progressBar = GetNode<ProgressBar>("MarginContainer/ProgressBar");
		_progressBar.Value = 0;

		_experienceManager.ExperienceUpdated += OnExperienceUpdated;
	}

	private void OnExperienceUpdated(float currentExperience, float targetExperience)
	{
		float progressPercent = currentExperience / targetExperience;
        _progressBar.Value = progressPercent;
	}
}

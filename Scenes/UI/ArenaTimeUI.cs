using DSurvivors.scenes.manager;
using Godot;
using System;

namespace DSurvivors.scenes.ui;

public partial class ArenaTimeUI : CanvasLayer
{
    [Export] private ArenaTimeManager _arenaTimeManager;
    private Label _timeLabel;

    public override void _Ready()
    {
        _timeLabel = GetNode<Label>("%TimeLabel");
    }

    public override void _Process(double delta)
    {
        if (_arenaTimeManager == null)
        {
            return;
        }
        var timeElapsed = (float)_arenaTimeManager.GetTimeElapsed();
        _timeLabel.Text = FormatSecondsToString(timeElapsed);
    }

    private string FormatSecondsToString(float totalSeconds)
    {
        var minutes = Mathf.FloorToInt(totalSeconds / 60);
        var seconds = Mathf.FloorToInt(totalSeconds - minutes * 60);

        return minutes.ToString() + ":" + String.Format("{0:00}", seconds);
    }
}

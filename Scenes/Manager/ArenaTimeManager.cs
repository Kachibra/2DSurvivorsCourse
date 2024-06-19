using DSurvivors.scenes.ui;
using Godot;
using System;

namespace DSurvivors.scenes.manager;

public partial class ArenaTimeManager : Node
{
    [Signal] public delegate void ArenaDifficultyIncreasedEventHandler(int arenaDifficulty);
    
    private const int DifficultyInterval = 5;
    
    [Export] PackedScene _endScreenScene = new PackedScene();
    
    private Timer _timer;
    
    private int _arenaDifficulty = 0;

    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");

        _timer.Timeout += OnTimerTimeout;
    }
    public override void _Process(double delta)
    {
        double nextTimeTarget = _timer.WaitTime - ((_arenaDifficulty + 1) * DifficultyInterval);
        if (_timer.TimeLeft <= nextTimeTarget)
        {
            _arenaDifficulty += 1;
            EmitSignal(SignalName.ArenaDifficultyIncreased, _arenaDifficulty);
        }
    }

    public override void _ExitTree()
    {
        _timer.Timeout -= OnTimerTimeout;
        _endScreenScene = null;
    }

    public double GetTimeElapsed()
    {
        return _timer.WaitTime - _timer.TimeLeft;
    }

    private void OnTimerTimeout()
    {
        var endScreenInstance = _endScreenScene.Instantiate();

        if (endScreenInstance == null)
        {
            return;
        }

        AddChild(endScreenInstance);
    }
}

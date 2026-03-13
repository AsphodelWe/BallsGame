using System;
using Reflex.Attributes;
using UnityEngine;

public class TargetStrategyFactory : ITargetStrategyFactory
{
    [Inject] private BallRegistry _ballRegistry;
    public ITargetStrategy CreateStrategy(TargetStrategyConfig config, SideConfig side)
    {
        return config switch
        {
            TrackTargetConfig track => new TrackTargetStrategy(_ballRegistry, side, track.SmoothTime),
            NoTargetConfig noTarget => new NoTargetStrategy(),
            _ => throw new ArgumentException($"Unknown config: {config.GetType()}")
        };
    }
}

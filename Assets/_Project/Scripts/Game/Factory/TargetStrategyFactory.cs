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
            TrackTargetWithMarkConfig trackWithMark => new TrackTargetWithMarkStrategy(_ballRegistry, side, trackWithMark.SmoothTime, trackWithMark.Mark),
            NoTargetConfig => new NoTargetStrategy(),
            _ => throw new ArgumentException($"Unknown config: {config.GetType()}")
        };
    }
}

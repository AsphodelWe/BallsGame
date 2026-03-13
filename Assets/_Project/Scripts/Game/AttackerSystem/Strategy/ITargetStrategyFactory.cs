using UnityEngine;

public interface ITargetStrategyFactory
{
    ITargetStrategy CreateStrategy(TargetStrategyConfig config, SideConfig side);
}

using Reflex.Attributes;
using UnityEngine;

public abstract class AttackerFactory
{
    [Inject] protected ITargetStrategyFactory _targetStrategyFactory;
    public abstract Attacker CreateAttacker(AttackerConfig config, SideConfig side, Transform slot, BallPresent target = null);
    protected T SetupAttacker<T>(T attacker, AttackerConfig config, SideConfig side, BallPresent target) where T : Attacker
    {
        attacker.Side = side;
        attacker.Damage = config.Damage;

        if (config.TargetStrategyConfig != null)
        {
            attacker.TargetStrategy = _targetStrategyFactory.CreateStrategy(config.TargetStrategyConfig, side);
        }

        return attacker;
    }
}

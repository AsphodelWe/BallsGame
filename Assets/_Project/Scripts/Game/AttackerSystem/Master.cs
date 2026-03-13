using Reflex.Attributes;
using UnityEngine;

public abstract class Master : MonoBehaviour
{
    [Inject] protected AttackerFactory _attackerFactory;
    [Inject] protected BallRegistry _ballRegistry;
    protected Transform _slot;
    protected AttackerConfig _attackerConfig;
    protected SideConfig _side;
    protected BallPresent _target;
    protected Attacker _attacker;
    public virtual void Initialize(AttackerConfig config, SideConfig side, Transform slot)
    {
        _attackerConfig = config;
        _side = side;
        _slot = slot;
    }

    protected virtual void EquipAttacker()
    {
        _attacker = _attackerFactory.CreateAttacker(_attackerConfig, _side, _slot);
    }
}

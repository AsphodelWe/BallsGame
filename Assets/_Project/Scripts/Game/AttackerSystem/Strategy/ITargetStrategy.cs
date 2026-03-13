using UnityEngine;

public interface ITargetStrategy
{
    void UpdateTarget(Attacker attacker);
    void SetTarget(BallPresent target);
    BallPresent CurrentTarget { get; }
}

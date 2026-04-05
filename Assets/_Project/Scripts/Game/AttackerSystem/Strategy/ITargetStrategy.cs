using System;
using UnityEngine;

public interface ITargetStrategy : IDisposable
{
    void UpdateTarget(Attacker attacker);
    void SetTarget(BallPresent target);
    BallPresent CurrentTarget { get; }
}

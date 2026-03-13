using UnityEngine;

public class NoTargetStrategy : ITargetStrategy
{
    public BallPresent CurrentTarget => null;
    public void SetTarget(BallPresent target)
    {
        Debug.Log("NoTargetStrategy: попытка установить цель, но слежение отключено");
    }
    public void UpdateTarget(Attacker attacker){}

}

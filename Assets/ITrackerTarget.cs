using UnityEngine;

public interface ITrackerTarget
{
    public BallPresent Target { get;set;}
    public void MoveTarget();
    public float SmoothTime{get; set;}
    public void SetTargetData(BallPresent target, float smoothTime);
}

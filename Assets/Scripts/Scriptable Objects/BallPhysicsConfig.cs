using UnityEngine;

[CreateAssetMenu(fileName = "BallPhysicsConfig", menuName = "Scriptable Objects/BallPhysicsConfig")]
public class BallPhysicsConfig : ScriptableObject
{
    [Header("Speed")]
    public float BaseSpeed = 6f;
    public float MaxSpeedMultiplier = 2f;
    public float SpeedDecayRate = 0.1f;
    [Range(0.5f, 1f)] public float SpeedThreshold = 0.8f;

    public float SpeedMultiplier = 1f;
    public float BoostForce = 3f;
    public float BounceImpulse = 5f;

    [Range(0f, 10f)] public float MinSpeed = 1f;

    public float WallBoostMultiplier = 0.3f;
    public float BallBoostMultiplier = 0.2f;

    [Range(0.5f, 1.5f)]public float RandomWallKickForce = 1f;
}

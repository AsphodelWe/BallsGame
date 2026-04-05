using UnityEngine;

[CreateAssetMenu(fileName = "TargetWithMarkStrategy", menuName = "Scriptable Objects/TargetWithMarkStrategy")]
public class TrackTargetWithMarkConfig : TargetStrategyConfig
{
    public GameObject Mark;
    public float SmoothTime = 0.3f;

}

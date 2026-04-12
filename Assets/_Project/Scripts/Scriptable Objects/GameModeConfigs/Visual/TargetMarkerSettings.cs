using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "TargetMarkerSettings", menuName = "Scriptable Objects/TargetMarkerSettings")]
public class TargetMarkerSettings : ScriptableObject
{
    [Header("Scale Animation")]
    public float ScaleMin = 1.1f;
    public float ScaleMax = 1.2f;
    public float Duration = 0.5f;
    public Ease Ease = Ease.InOutSine;
}

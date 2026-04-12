using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "DamageNumberSettings", menuName = "Scriptable Objects/DamageNumberSettings")]
public class DamageNumberSettings : ScriptableObject
{
    [Header("Flight")]
    public float FlyHeight = 15f;
    public float FlyDuration = 0.5f;
    public Ease FlyEase = Ease.OutQuad;

    [Header("Fade")]
    public float FadeDuration = 1f;

    [Header("Scale")]
    public float ScaleEnd = 1.5f;
    public float ScaleDuration = 1f;
    public Ease ScaleEase = Ease.OutQuad;
}

using DG.Tweening;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraEffectsConfig", menuName = "Scriptable Objects/CameraEffectsConfig")]
public class CameraEffectsConfig : ScriptableObject
{
    [Header("Vertical Bob (вверх-вниз)")]
    public float BobHeight = 0.2f;
    public float BobDuration = 1.5f;
    public Ease BobEase = Ease.InOutSine;

    [Header("Horizontal Bob (влево-вправо)")]
    public float BobWidth = 0.15f;
    public float HorizontalBobDuration = 1.8f;
    public Ease HorizontalBobEase = Ease.InOutSine;

    [Header("Breathing (пульсация зума)")]
    public float ZoomAmplitude = 0.3f;
    public float ZoomDuration = 2f;
    public Ease ZoomEase = Ease.InOutSine;

    [Header("Circle Path (круг)")]
    public float CircleRadius = 1.5f;
    public float CircleDuration = 4f;
    public int CircleSegments = 4;
    public PathType CirclePathType = PathType.CatmullRom;
    public LoopType CircleLoopType = LoopType.Yoyo;
}

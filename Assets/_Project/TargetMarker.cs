using DG.Tweening;
using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    [SerializeField] private float _scaleMultiplierAnimationMin = 1.1f;
    [SerializeField] private float _scaleMultiplierAnimationMax = 1.2f;
    [SerializeField] private float _timeAnimation = 0.5f;
    private Tween _scaleTween;
    private void Start()
    {
        Vector3 startScale = transform.localScale;
        float targetScale = Random.Range(_scaleMultiplierAnimationMin, _scaleMultiplierAnimationMax);

        _scaleTween = transform.DOScale(startScale * targetScale, _timeAnimation)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine);
    }

    private void OnDestroy()
    {
        _scaleTween?.Kill();
    }
}

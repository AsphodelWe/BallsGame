using DG.Tweening;
using UnityEngine;

public class TargetMarker : MonoBehaviour
{
    [SerializeField] private TargetMarkerSettings _settings;
    private Tween _scaleTween;
    private void Start()
    {
        float targetScale = Random.Range(_settings.ScaleMin, _settings.ScaleMax);

        _scaleTween = transform.DOScale(targetScale, _settings.Duration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(_settings.Ease);
    }

    private void OnDestroy()
    {
        _scaleTween?.Kill();
    }
}

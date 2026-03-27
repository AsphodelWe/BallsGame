using UnityEngine;
using DG.Tweening;
using Reflex.Attributes;

public class MapPresent : MonoBehaviour
{
    [Inject] IMapScaleProvider _mapScaleProvider;
    [Inject] private BattleData _battleData;
    [SerializeField] private LayerMask _ballLayer;
    private bool _isRotate;
    private Tween _shakeMap;
    private Tween _rotateMap;
    public bool IsRotate => _isRotate;
    void Start()
    {
        if (_battleData.RotateMap)
        {
            _rotateMap = gameObject.transform.DORotate(new Vector3(0, 0, 360), 5, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((_ballLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            float mapScale = _mapScaleProvider.ScaleMultiplier;
            float shakeStrength = 0.05f * Mathf.Sqrt(mapScale);
            float shakeDuration = 0.1f;

            _shakeMap?.Kill();
            transform.localScale = Vector3.one * _mapScaleProvider.CurrentScale;

            _shakeMap = transform.DOShakeScale(shakeDuration, shakeStrength, 90);
        }
    }
}

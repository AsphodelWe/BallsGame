using UnityEngine;
using DG.Tweening;

public class MapPresent : MonoBehaviour
{
    [SerializeField] private LayerMask _ballLayer;

    [SerializeField] private bool _isRotate;
    private Tween _shakeMap;
    private Tween _rotateMap;
    void Start()
    {
        if (_isRotate)
        {
            _rotateMap = gameObject.transform.DORotate(new Vector3(0, 0, 360), 5, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if ((_ballLayer.value & (1 << collision.gameObject.layer)) != 0)
        {
            if (_shakeMap != null && _shakeMap.IsActive())
            {
                _shakeMap.Restart();
            }
            else
            {
                _shakeMap = transform.DOShakeScale(0.1f, 0.05f, 90);
            }
        }
    }
}

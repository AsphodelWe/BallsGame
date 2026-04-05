using System;
using UnityEngine;
using DG.Tweening;

public class BallView : MonoBehaviour
{
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _ballLayer;
    [SerializeField] private DamageNumber _damageNumberPrefab;
    [SerializeField] private bool _isNumberDamage;
    private ObjectPool<DamageNumber> _damageNumberPool;
    private Sprite _flag;
    private SpriteRenderer _spriteRenderer;
    private Tween _shakeBall;
    private Tween _changeColor;
    public void Initialize(Sprite flag)
    {
        if (_isNumberDamage) CreateNumberPool();

        _flag = flag;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _shakeBall = transform.DOShakeScale(0.3f, 0.1f, 90).SetAutoKill(false).Pause();
    }
    public Sprite GetFlag => _flag;

    public void PlayHitEffect(float duration = 0.1f)
    {
        if (_spriteRenderer == null) return;

        _changeColor?.Kill();

        try
        {
            _changeColor = _spriteRenderer.DOColor(Color.red, duration / 2)
                .OnComplete(() =>
                {
                    if (_spriteRenderer != null)
                        _spriteRenderer.DOColor(Color.white, duration / 2);
                });
        }
        catch (MissingReferenceException)
        { }

    }

    private void CreateNumberPool()
    {
        _damageNumberPool = new ObjectPool<DamageNumber>(
        _damageNumberPrefab,
        null,
        5,
        10
    );
    }

    public void ShowDamageNumber(int damage)
    {
        if (_damageNumberPool == null) return;
        var number = _damageNumberPool.Get();

        Vector3 offset = new Vector3(0, 0.3f, 0);

        number.Initialize(damage, gameObject.transform, offset);
    }

    void OnDestroy()
    {
        _shakeBall?.Kill();
        _changeColor?.Kill();

        if (Application.isPlaying && gameObject.scene.isLoaded)
        {
            _damageNumberPool?.Clear();
        }
    }
}

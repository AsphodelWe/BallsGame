using System;
using UnityEngine;
using DG.Tweening;
using System.Threading;
using Cysharp.Threading.Tasks;

public class BallView : MonoBehaviour
{
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _ballLayer;
    [SerializeField] private DamageNumber _damageNumberPrefab;
    [SerializeField] private bool _isNumberDamage;
    [SerializeField] private GameObject _deathEffect;
    private ObjectPool<DamageNumber> _damageNumberPool;
    private Sprite _flag;
    private SpriteRenderer _spriteRenderer;
    private Tween _shakeBall;
    private CancellationTokenSource _hitCts;
    private bool _isPlayingHitEffect;
    public void Initialize(Sprite flag)
    {
        if (_isNumberDamage) CreateNumberPool();

        _flag = flag;
        _spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        _shakeBall = transform.DOShakeScale(0.3f, 0.1f, 90).SetAutoKill(false).Pause();
    }
    public Sprite GetFlag => _flag;

    public async void PlayHitEffect(float duration = 0.1f)
    {
        if (_spriteRenderer == null) return;
        if (_isPlayingHitEffect) return;

        _isPlayingHitEffect = true;

        _hitCts?.Cancel();
        _hitCts?.Dispose();
        _hitCts = new CancellationTokenSource();

        Color originalColor = _spriteRenderer.color;
        _spriteRenderer.color = Color.red;

        try
        {
            await UniTask.Delay((int)(duration / 2 * 1000), cancellationToken: _hitCts.Token);

            if (_spriteRenderer != null && gameObject != null)
            {
                _spriteRenderer.color = originalColor;
            }
        }
        catch (OperationCanceledException)
        {
            if (_spriteRenderer != null && gameObject != null)
            {
                _spriteRenderer.color = originalColor;
            }
        }
        finally
        {
            _isPlayingHitEffect = false;
        }
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

    public void PlayDeathEffect()
    {
        GameObject effect = Instantiate(_deathEffect, transform.position, Quaternion.identity);
        Destroy(effect, 1f);
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

        _hitCts?.Cancel();
        _hitCts?.Dispose();

        if (Application.isPlaying && gameObject.scene.isLoaded)
        {
            _damageNumberPool?.Clear();
        }
    }
}

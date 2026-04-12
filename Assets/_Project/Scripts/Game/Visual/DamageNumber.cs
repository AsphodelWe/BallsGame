using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour, IPoolable<DamageNumber>
{
    [SerializeField] private TextMeshPro _text;
    [SerializeField] private DamageNumberSettings _settings;
    private Transform _target;
    private Vector3 _offset;
    private Tween _moveTween;
    private bool _isActive;

    public event Action<DamageNumber> OnDespawnRequested;

    public void Initialize(int damage, Transform target, Vector3 offset)
    {
        _text.text = "-" + damage.ToString() + " HP";
        _target = target;
        _offset = offset;
        _isActive = true;

        transform.position = target.position + offset;

        _text.color = Color.white;
        _text.transform.localScale = Vector3.one;
        _text.transform.localPosition = Vector3.zero;

        _moveTween?.Kill();
        FlyAnimation();
    }

    private void FlyAnimation()
    {
        Sequence seq = DOTween.Sequence();

        seq.Join(_text.transform.DOLocalMoveY(_settings.FlyHeight, _settings.FlyDuration)
            .SetEase(_settings.FlyEase));

        Color endColor = new Color(1, 1, 1, 0);
        seq.Join(DOTween.To(() => _text.color, x => _text.color = x, endColor, _settings.FadeDuration));

        seq.Join(_text.transform.DOScale(_settings.ScaleEnd, _settings.ScaleDuration)
            .SetEase(_settings.ScaleEase));

        seq.OnComplete(() =>
        {
            if (_isActive)
                OnDespawnRequested?.Invoke(this);
        });

        _moveTween = seq;
    }

    private void Update()
    {
        if (!_isActive) return;
        if (_target == null) return;
        transform.position = _target.position + _offset;
    }

    public void OnSpawn()
    {
        _isActive = true;
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        _isActive = false;
        _moveTween?.Kill();
        gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        _moveTween?.Kill();
    }
}

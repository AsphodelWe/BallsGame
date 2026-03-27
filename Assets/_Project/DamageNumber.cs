using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class DamageNumber : MonoBehaviour, IPoolable<DamageNumber>
{
    [SerializeField] private TextMeshPro _text;
    private Transform _target;
    private Vector3 _offset;
    private Tween _moveTween;
    public event Action<DamageNumber> OnDespawnRequested;

    public void Initialize(int damage, Transform target, Vector3 offset)
    {
        _text.text = "-" + damage.ToString() + " HP";
        _target = target;
        _offset = offset;

        transform.position = target.position + offset;

        _text.color = Color.white;
        _text.transform.localScale = Vector3.one;
        _text.transform.localPosition = Vector3.zero;

        Sequence seq = DOTween.Sequence();

        seq.Join(_text.transform.DOLocalMoveY(15f, 0.5f).SetEase(Ease.OutQuad));

        Color startColor = _text.color;
        Color endColor = new Color(startColor.r, startColor.g, startColor.b, 0);
        seq.Join(DOTween.To(() => _text.color, x => _text.color = x, endColor, 1f));

        seq.Join(_text.transform.DOScale(1.5f, 1f));

        seq.OnComplete(() => OnDespawnRequested?.Invoke(this));

        _moveTween = seq;
    }

    void Update()
    {
        if (_target == null) return;
        transform.position = _target.position + _offset;

    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }

    public void OnDespawn()
    {
        _moveTween?.Kill();
        gameObject.SetActive(false);
    }
    public void OnAnimationEnd()
    {
        OnDespawnRequested?.Invoke(this);
    }
}

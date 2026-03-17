using System;
using UnityEngine;
using DG.Tweening;

public class BallView : MonoBehaviour
{
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _ballLayer;
    private Sprite _flag;
    private Tween _shakeBall;
    public void Initialize(Sprite flag)
    {
        _flag = flag;
        _shakeBall = transform.DOShakeScale(0.3f, 0.1f, 90).SetAutoKill(false).Pause();
    }
    public Sprite GetFlag => _flag;

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (IsInLayerMask(collision.gameObject, _ballLayer))
        {
            _shakeBall.Restart();
        }
    }
    
    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }

}

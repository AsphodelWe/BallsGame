using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable<Bullet>
{
    private int _damage;
    private SideConfig _side;
    public event Action<Bullet> OnDespawnRequested;

    public void Initialize(SideConfig side)
    {
        _side = side;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.TryGetComponent<BallPresent>(out var ball))
        {
            if (ball.GetSide != _side)
            {
                //нанести урон
            }
        }
        OnDespawnRequested.Invoke(this);
    }

    public void OnDespawn() => gameObject.SetActive(false);
    public void OnSpawn() => gameObject.SetActive(true);
}

using UnityEngine;
using System;

public interface IPoolable<T> where T : MonoBehaviour, IPoolable<T>
{
    event Action<T> OnDespawnRequested;
    void Initialize(BulletConfig config, int damage, Vector2 direction, SideConfig side);
    void OnSpawn();
    void OnDespawn();
}

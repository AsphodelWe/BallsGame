using UnityEngine;
using System;

public interface IPoolable<T> where T : MonoBehaviour, IPoolable<T>
{
    event Action<T> OnDespawnRequested;
    void OnSpawn();
    void OnDespawn();
}

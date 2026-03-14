using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class ObjectPool<T> where T : MonoBehaviour, IPoolable<T>
{
    private readonly IObjectPool<T> _pool;
    private readonly T _prefab;
    private readonly Transform _parent;
    public ObjectPool(T prefab, Transform parent = null, int defaultCapacity = 20, int maxSize = 30)
    {
        _prefab = prefab;
        _parent = parent;

        _pool = new UnityEngine.Pool.ObjectPool<T>(
            createFunc: CreateItem,
            actionOnGet: OnGetItem,
            actionOnRelease: OnReleaseItem,
            actionOnDestroy: OnDestroyItem,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );
    }

    private T CreateItem()
    {
        T item = Object.Instantiate(_prefab);
        return item;
    }

    private void OnGetItem(T item)
    {
        item.OnSpawn();
        item.OnDespawnRequested += OnItemDespawnRequested;
    }

    private void OnItemDespawnRequested(T item)
    {
        item.OnDespawnRequested -= OnItemDespawnRequested;
        Release(item);
    }
    private void OnReleaseItem(T item) => item.OnDespawn();
    private void OnDestroyItem(T item) => Object.Destroy(item.gameObject);
    public T Get() => _pool.Get();
    public void Release(T item)
    {
        _pool.Release(item);
    }
}

using System;
using UnityEngine;

public class ShootExplosion : MonoBehaviour, IPoolable<ShootExplosion>
{
    private float _timer;
    public event Action<ShootExplosion> OnDespawnRequested;
    private ParticleSystem _effect;

    private float _timeDurationEffect;
    private void Awake()
    {
        _effect = GetComponent<ParticleSystem>();
        if (_effect == null)
            Debug.LogError($"ParticleSystem not found on {gameObject.name}", this);

        _timeDurationEffect = _effect.main.duration;
    }

    private void Update()
    {
        _timer += Time.deltaTime;
        if (_timer > _timeDurationEffect)
        {
            ReturnToPool();
        }
    }

    public void OnSpawn()
    {
        gameObject.SetActive(true);
        _timer = 0f;

        if (_effect != null)
        {
            _effect.Clear();
            _effect.Play();
        }
    }

    public void OnDespawn()
    {
        if (_effect != null)
            _effect.Stop();

        gameObject.SetActive(false);
    }

    private void ReturnToPool()
    {
        OnDespawnRequested?.Invoke(this);
    }
}

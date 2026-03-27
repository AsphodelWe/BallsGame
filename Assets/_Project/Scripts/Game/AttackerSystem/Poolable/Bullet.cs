using System;
using UnityEngine;

public class Bullet : MonoBehaviour, IPoolable<Bullet>
{
    [SerializeField] private LayerMask _wallLayer;
    private BulletConfig _config;
    private int _damage;
    private float _speed;
    private Vector2 _direction;
    private SideConfig _side;
    public event Action<Bullet> OnDespawnRequested;
    private TrailRenderer _trail;
    private float _timer;

    void Awake()
    {
        _trail = GetComponent<TrailRenderer>();
    }

    public void Initialize(BulletConfig config, int damage, Vector2 direction, SideConfig side)
    {
        _config = config;
        _damage = damage;
        _speed = _config.Speed;
        _direction = direction.normalized;
        _side = side;
    }

    private void Update()
    {
        transform.position += (Vector3)_direction * _speed * Time.deltaTime;

        _timer += Time.deltaTime;
        if (_timer >= _config.LifeTime)
        {
            ReturnToPool();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (TryHitBall(other)) return;
        if (TryHitWall(other)) return;
    }

    private bool TryHitBall(Collider2D other)
    {
        if (!other.TryGetComponent<IDamagable>(out var ball)) return false;
        if (ball.GetSide == _side) return false;

        Vector2 hitDirection = (other.transform.position - transform.position).normalized;
        
        ball.TakeDamage(_damage, hitDirection);
        ReturnToPool();
        return true;
    }

    private bool TryHitWall(Collider2D other)
    {
        if ((_wallLayer.value & (1 << other.gameObject.layer)) != 0)
            ReturnToPool();
        return true;
    }

    private void ReturnToPool()
    {
        OnDespawnRequested?.Invoke(this);
    }

    public void OnDespawn()
    {
        if (_trail != null)
            _trail.Clear();
        _timer = 0f;

        gameObject.SetActive(false);
    }
    public void OnSpawn()
    {
        gameObject.SetActive(true);
    }
}

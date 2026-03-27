using System;
using UnityEngine;

public class Weapon : Attacker
{
    [SerializeField] private Transform _firePoint;
    public Transform FirePoint => _firePoint;
    public IWeaponAttackHandler AttackHandler { get; set; }
    public override ITargetStrategy TargetStrategy { get; set; }
    public ObjectPool<Bullet> BulletPool { get; private set; }
    public ObjectPool<ShootExplosion> ShootEffectPool { get; private set; }

    public event Action<Vector2, float> OnRecoil;

    private void Update()
    {
        TargetStrategy?.UpdateTarget(this);
        AttackHandler?.Attack(this);
    }

    public void SetBulletPull(ObjectPool<Bullet> bulletPool)
    {
        BulletPool = bulletPool;
    }

    public void SetShotEffectPull(ObjectPool<ShootExplosion> shootEffectPool)
    {
        ShootEffectPool = shootEffectPool;
    }

    private void OnDestroy()
    {
        (AttackHandler as IDisposable)?.Dispose();
    }

    public void TriggerRecoil(Vector2 direction, float force)
    {
        OnRecoil?.Invoke(direction, force);
    }
}

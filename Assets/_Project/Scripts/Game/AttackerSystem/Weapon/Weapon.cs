using System;
using UnityEngine;

public class Weapon : Attacker
{
    [SerializeField] private Transform _firePoint;
    public Transform FirePoint => _firePoint;
    public IWeaponAttackHandler AttackHandler { get; set; }
    public override ITargetStrategy TargetStrategy { get; set; }
    public ObjectPool<Bullet> BulletPool { get; private set; }

    private void Update()
    {
        TargetStrategy?.UpdateTarget(this);
        AttackHandler?.Attack(this);
    }

    public void SetBulletPull(ObjectPool<Bullet> bulletPool)
    {
        BulletPool = bulletPool;
    }

    private void OnDestroy()
    {
        (AttackHandler as IDisposable)?.Dispose();
    }
}

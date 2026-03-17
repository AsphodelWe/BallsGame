using System;
using UnityEngine;

public class WeaponAttackHandlerFactory : IWeaponAttackHandlerFactory
{
    public IWeaponAttackHandler CreateHandler(IShootConfig config, ObjectPool<Bullet> bulletPool, ObjectPool<ShootExplosion> effectPrefab)
    {
        return config switch
        {
            AutoWeaponConfig auto => new AutoWeaponAttackHandler(auto, bulletPool, effectPrefab),
            SingleShootConfig single => new SingleShootHandler(single, bulletPool, effectPrefab),
/*             BurstShootConfig burst => new BurstShootHandler(burst),
            SingleShootConfig single => new SingleShootHandler(single), */
            _ => throw new ArgumentException($"Unknown config type: {config.GetType()}")
        };
    }
}

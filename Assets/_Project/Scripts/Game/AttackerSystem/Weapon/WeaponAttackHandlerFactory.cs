using System;
using UnityEngine;

public class WeaponAttackHandlerFactory : IWeaponAttackHandlerFactory
{
    public IWeaponAttackHandler CreateHandler(IShootConfig config, ObjectPool<Bullet> bulletPool)
    {
        return config switch
        {
            AutoWeaponConfig auto => new AutoWeaponAttackHandler(auto, bulletPool),
            SingleShootConfig single => new SingleShootHandler(single, bulletPool),
/*             BurstShootConfig burst => new BurstShootHandler(burst),
            SingleShootConfig single => new SingleShootHandler(single), */
            _ => throw new ArgumentException($"Unknown config type: {config.GetType()}")
        };
    }
}

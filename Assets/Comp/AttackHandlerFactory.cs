using System;
using UnityEngine;

public class AttackHandlerFactory
{
        public IWeaponAttackHandler CreateHandler(IShootConfig config)
    {
        return config switch
        {
            AutoWeaponConfig auto => new AutoWeaponAttackHandler(auto),
/*             BurstShootConfig burst => new BurstShootHandler(burst),
            SingleShootConfig single => new SingleShootHandler(single), */
            _ => throw new ArgumentException($"Unknown config type: {config.GetType()}")
        };
    }
}

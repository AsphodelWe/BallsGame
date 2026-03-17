using UnityEngine;

public interface IWeaponAttackHandlerFactory
{
    IWeaponAttackHandler CreateHandler(IShootConfig config, ObjectPool<Bullet> bulletPool, ObjectPool<ShootExplosion> effectPool);
}

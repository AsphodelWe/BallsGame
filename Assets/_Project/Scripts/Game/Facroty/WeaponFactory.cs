using Reflex.Attributes;
using UnityEngine;

public class WeaponFactory : AttackerFactory
{
    [Inject] private IWeaponAttackHandlerFactory _handlerFactory;

    private const int DEFAULT_POOL_CAPACITY = 20;
    private const int DEFAULT_POOL_MAX_SIZE = 30;
    private const float POOL_BUFFER_FACTOR = 1.5f;

    public override Attacker CreateAttacker(AttackerConfig config, SideConfig side, Transform slot, BallPresent target = null)
    {
        var weaponConfig = config as WeaponConfig;
        if (weaponConfig == null) Debug.Log("WeaponConfig null");

        var shootConfig = weaponConfig.GetModule<IShootConfig>();
        if (shootConfig == null) Debug.Log("Нет модуля Стрельбы");

        var magazineConfig = weaponConfig.GetModule<IMagazineWeapon>();
        if (magazineConfig == null) Debug.Log("Нет модуля магазина");

        var weapon = Object.Instantiate(weaponConfig.Prefab, slot).GetComponent<Weapon>();
        weapon.transform.ApplyCurrentScaleToWorld();

        var bulletPool = CreatePull(shootConfig, weapon, magazineConfig);

        weapon.AttackHandler = _handlerFactory.CreateHandler(shootConfig, bulletPool);
        SetupAttacker(weapon, config, side, target);

        return weapon;
    }

    private ObjectPool<Bullet> CreatePull(IShootConfig shootConfig, Weapon weapon, IMagazineWeapon magazineConfig = null)
    {
        var (capacity, maxSize) = GetPoolSizes(magazineConfig);
        var bulletPool = new ObjectPool<Bullet>(
            shootConfig.BulletPrefab.GetComponent<Bullet>(),
            weapon.transform,
            capacity,
            maxSize
        );

        weapon.SetBulletPull(bulletPool);

        return bulletPool;
    }
    private (int capacity, int maxSize) GetPoolSizes(IMagazineWeapon magazineConfig)
    {
        if (magazineConfig == null)
            return (DEFAULT_POOL_CAPACITY, DEFAULT_POOL_MAX_SIZE);
            

        int maxAmmo = magazineConfig.MaxAmmo;
        return (maxAmmo, (int)(maxAmmo * POOL_BUFFER_FACTOR));
    }

}

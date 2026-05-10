using Reflex.Attributes;
using UnityEngine;
using UnityEngine.Animations;

public class WeaponFactory : AttackerFactory
{
    [Inject] private IWeaponAttackHandlerFactory _handlerFactory;

    private const int DEFAULT_POOL_CAPACITY = 20;
    private const int DEFAULT_POOL_MAX_SIZE = 30;
    private const float POOL_BUFFER_FACTOR = 1.5f;

    public override Attacker CreateAttacker(AttackerConfig config, SideConfig side, Transform slot, BallPresent target = null)
    {
        var weaponConfig = config as WeaponConfig;

        WeaponSaveSystem.LoadWeapon(weaponConfig);
        
        if (weaponConfig == null)
        {
            Debug.LogError($"[WeaponFactory] Неверный тип конфига: {config?.GetType().Name ?? "null"}. Ожидался WeaponConfig.");
            return null;
        }

        var shootModule = weaponConfig.GetActiveModule<IShootConfig>();
        if (shootModule == null)
        {
            Debug.LogError($"[WeaponFactory] У {weaponConfig.Name} нет активного модуля IShootConfig!");
            return null;
        }

        var magazineModule = weaponConfig.GetModule<IMagazineWeapon>();

        var weapon = Object.Instantiate(weaponConfig.Prefab, slot).GetComponent<Weapon>();
        weapon.transform.ApplyCurrentScaleToWorld();

        var bulletPool = CreateBulletPull(shootModule, weapon, magazineModule);
        var effectPool = CreateEffectPull(shootModule, weapon);

        weapon.AttackHandler = _handlerFactory.CreateHandler(shootModule, bulletPool, effectPool);

        SetupAttacker(weapon, weaponConfig, side, target);

        return weapon;
    }

    private ObjectPool<Bullet> CreateBulletPull(IShootConfig shootConfig, Weapon weapon, IMagazineWeapon magazineConfig = null)
    {
        var (capacity, maxSize) = GetPoolSizes(magazineConfig);
        var bulletPool = new ObjectPool<Bullet>(
            shootConfig.BulletPrefab.GetComponent<Bullet>(),
            null,
            capacity,
            maxSize
        );

        weapon.SetBulletPull(bulletPool);

        return bulletPool;
    }

    private ObjectPool<ShootExplosion> CreateEffectPull(IShootConfig shootConfig, Weapon weapon)
    {

        Transform parent = shootConfig is SingleShootConfig ? weapon.transform : null;
        var effectPool = new ObjectPool<ShootExplosion>
            (shootConfig.ShootEffectPrefab.GetComponent<ShootExplosion>(),
            parent,
            DEFAULT_POOL_CAPACITY,
            DEFAULT_POOL_MAX_SIZE);

        weapon.SetShotEffectPull(effectPool);

        return effectPool;
    }


    private (int capacity, int maxSize) GetPoolSizes(IMagazineWeapon magazineConfig)
    {
        if (magazineConfig == null)
            return (DEFAULT_POOL_CAPACITY, DEFAULT_POOL_MAX_SIZE);


        int maxAmmo = magazineConfig.MaxAmmo;
        return (maxAmmo, (int)(maxAmmo * POOL_BUFFER_FACTOR));
    }

}

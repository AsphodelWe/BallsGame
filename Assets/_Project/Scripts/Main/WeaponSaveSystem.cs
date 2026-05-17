using UnityEngine;

public static class WeaponSaveSystem
{
    [System.Serializable]
    private class WeaponSaveData
    {
        public string weaponName;
        public int damage;
        public string activeWeaponType;
        public string targetStrategyName;
        
        
        public float fireRate;
        public float recoilForce;
        public float hitForce;
        public float reloadTime;
        public int maxAmmo;
        public float bulletDelay;
        public int burstSize;
        public float burstDelay;
        public int bulletCount;
        public int spreadAngle;
        public bool isMarkTarget;
    }

    public static void SaveWeapon(WeaponConfig weapon)
    {
        var module = weapon.GetActiveModule<IShootConfig>();
        
        var data = new WeaponSaveData
        {
            weaponName = weapon.Name,
            damage = weapon.Damage,
            activeWeaponType = weapon.GetActiveWeaponType().ToString(),
            targetStrategyName = weapon.TargetStrategyConfig?.Name
        };

        if (module != null)
            SaveModuleData(module, data);

        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString($"weapon_{weapon.Name}", json);
        PlayerPrefs.Save();
        
        Debug.Log($"✅ {weapon.Name} сохранено в JSON");
    }

    private static void SaveModuleData(IShootConfig module, WeaponSaveData data)
    {
        data.fireRate = module.FireRate;
        data.recoilForce = module.RecoilForce;
        data.hitForce = module.HitForce;

        switch (module)
        {
            case AutoWeaponConfig auto:
                data.reloadTime = auto.FireRateMagazine;
                data.maxAmmo = auto.MaxAmmo;
                break;

            case BurstWeaponConfig burst:
                data.bulletDelay = burst.BulletDelay;
                data.burstSize = burst.BurstSize;
                data.burstDelay = burst.BurstDelay;
                break;

            case ShotgunConfig shotgun:
                data.bulletCount = shotgun.BulletCount;
                data.spreadAngle = shotgun.SpreadAngle;
                break;

            case SniperShootConfig sniper:
                data.isMarkTarget = sniper._isMarkTarget;
                break;
        }
    }

    public static void LoadWeapon(WeaponConfig weapon)
    {
        string json = PlayerPrefs.GetString($"weapon_{weapon.Name}", "");
        if (string.IsNullOrEmpty(json)) return;

        var data = JsonUtility.FromJson<WeaponSaveData>(json);

        weapon.Damage = data.damage;
        weapon.SetActiveWeaponType(System.Enum.Parse<AttackWeaponType>(data.activeWeaponType));

        var module = weapon.GetActiveModule<IShootConfig>();
        if (module != null)
            LoadModuleData(module, data);

        Debug.Log($"📂 {weapon.Name} загружено из JSON");
    }

    private static void LoadModuleData(IShootConfig module, WeaponSaveData data)
    {
        switch (module)
        {
            case AutoWeaponConfig auto:
                auto.FireRate = data.fireRate;
                auto.RecoilForce = data.recoilForce;
                auto.HitForce = data.hitForce;
                auto.FireRateMagazine = data.reloadTime;
                auto.MaxAmmo = data.maxAmmo;
                break;

            case BurstWeaponConfig burst:
                burst.RecoilForce = data.recoilForce;
                burst.HitForce = data.hitForce;
                burst.BulletDelay = data.bulletDelay;
                burst.BurstSize = data.burstSize;
                burst.BurstDelay = data.burstDelay;
                break;

            case SingleShootConfig single:
                single.FireRate = data.fireRate;
                single.RecoilForce = data.recoilForce;
                break;

            case ShotgunConfig shotgun:
                shotgun.FireRate = data.fireRate;
                shotgun.RecoilForce = data.recoilForce;
                shotgun.HitForce = data.hitForce;
                shotgun.BulletCount = data.bulletCount;
                shotgun.SpreadAngle = data.spreadAngle;
                break;

            case SniperShootConfig sniper:
                sniper.FireRate = data.fireRate;
                sniper.RecoilForce = data.recoilForce;
                sniper.HitForce = data.hitForce;
                sniper._isMarkTarget = data.isMarkTarget;
                break;
        }
    }
    
    public static string GetSavedTargetStrategyName(WeaponConfig weapon)
    {
        string json = PlayerPrefs.GetString($"weapon_{weapon.Name}", "");
        if (string.IsNullOrEmpty(json)) return null;
        
        var data = JsonUtility.FromJson<WeaponSaveData>(json);
        return data.targetStrategyName;
    }
}

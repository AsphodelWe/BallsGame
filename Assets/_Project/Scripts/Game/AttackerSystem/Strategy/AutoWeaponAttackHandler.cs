using UnityEngine;

public class AutoWeaponAttackHandler : IWeaponAttackHandler
{
    private AutoWeaponConfig _config;
    private int _currentAmmo;
    private float _lastShotTime;
    public AutoWeaponAttackHandler(AutoWeaponConfig config)
    {
        _config = config;
        _currentAmmo = config.MaxAmmo;
    }

    public void Attack(Weapon weapon)
    {
        //Debug.Log("ТУТУТУТУ");
    }

}

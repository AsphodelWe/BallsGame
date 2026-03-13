using UnityEngine;

public abstract class ShootWeaponConfig : WeaponConfig
{
    [Header("Bullet")]
    public GameObject BulletPrefab;
    public float BulletSpeed;

    public void InitializeShootWeapon()
    {
    }
}

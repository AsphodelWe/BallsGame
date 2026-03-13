using UnityEngine;

[CreateAssetMenu(fileName = "AutoWeaponConfig", menuName = "Scripts/Scriptable Objects/AutoWeaponConfig")]
public class AutoWeaponConfig : ScriptableObject, IShootConfig
{
    public float FireRate;
    public int MaxAmmo;
    public GameObject BulletPrefab;
    public float BulletSpeed;

    GameObject IShootConfig.BulletPrefab => BulletPrefab;
    float IShootConfig.BulletSpeed => BulletSpeed;
}

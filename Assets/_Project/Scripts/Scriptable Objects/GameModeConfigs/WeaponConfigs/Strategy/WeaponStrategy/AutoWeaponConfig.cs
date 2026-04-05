using UnityEngine;

[CreateAssetMenu(fileName = "AutoWeaponConfig", menuName = "Scripts/Scriptable Objects/AutoWeaponConfig")]
public class AutoWeaponConfig : ScriptableObject, IShootConfig, IMagazineWeapon
{
    [Header("⚡ShootModule")]
    public float FireRate;
    public BulletConfig BulletConfig;

    [Header("📦MagazineModule")]
    public int MaxAmmo;
    public float FireRateMagazine;

    [Header("🎨Visual")]
    public GameObject _prefabShootEffect;

    public float RecoilForce = 5f;
    public float ReloadTime => FireRateMagazine;
    public GameObject ShootEffectPrefab => _prefabShootEffect;
    GameObject IShootConfig.BulletPrefab => BulletConfig.Prefab;
    float IShootConfig.BulletSpeed => BulletConfig.Speed;
    int IMagazineWeapon.MaxAmmo => MaxAmmo;
    float IShootConfig.RecoilForce => RecoilForce;

    [Header("🔊 Audio")]
    public AudioClip ShootSound;
    [Range(0f, 1f)] public float ShootVolume = 0.5f;
}

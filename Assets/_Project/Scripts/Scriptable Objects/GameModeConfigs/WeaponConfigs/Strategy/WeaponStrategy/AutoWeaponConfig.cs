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
    public float HitForce = 3f;
    public float ReloadTime = 5f;
    public GameObject ShootEffectPrefab => _prefabShootEffect;

    float IShootConfig.BulletSpeed => BulletConfig.Speed;
    float IShootConfig.RecoilForce => RecoilForce;
    float IShootConfig.HitForce => HitForce;
    float IShootConfig.FireRate => FireRate;
    GameObject IShootConfig.BulletPrefab => BulletConfig.Prefab;

    float IMagazineWeapon.ReloadTime => ReloadTime;
    int IMagazineWeapon.MaxAmmo => MaxAmmo;


    [Header("🔊 Audio")]
    public AudioClip ShootSound;
    [Range(0f, 1f)] public float ShootVolume = 0.5f;

    [Header("Тип Стрельбы")]
    public AttackWeaponType WeaponType = AttackWeaponType.Auto;
}

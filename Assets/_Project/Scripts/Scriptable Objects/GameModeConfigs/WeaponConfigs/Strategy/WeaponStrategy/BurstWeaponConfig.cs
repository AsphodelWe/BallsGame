using UnityEngine;

[CreateAssetMenu(fileName = "BurstWeaponConfig", menuName = "Scriptable Objects/BurstWeaponConfig")]
public class BurstWeaponConfig : ScriptableObject, IShootConfig, IMagazineWeapon
{
    [Header("⚡ShootModule")]
    [Range(0.05f, 1)] public float BulletDelay;
    public BulletConfig BulletConfig;

    [Header("📦MagazineModule")]
    [Range(2, 10)] public int BurstSize;
    public float BurstDelay;

    [Header("🎨Visual")]
    public GameObject _prefabShootEffect;

    public float RecoilForce = 5f;
    public float HitForce = 3f;
    public GameObject ShootEffectPrefab => _prefabShootEffect;

    float IShootConfig.BulletSpeed => BulletConfig.Speed;
    float IShootConfig.RecoilForce => RecoilForce;
    float IShootConfig.HitForce => HitForce;
    float IShootConfig.FireRate => BulletDelay;
    GameObject IShootConfig.BulletPrefab => BulletConfig.Prefab;

    int IMagazineWeapon.MaxAmmo => BurstSize;
    float IMagazineWeapon.ReloadTime => BurstDelay;

    [Header("🔊 Audio")]
    public AudioClip ShootSound;
    [Range(0f, 1f)] public float ShootVolume = 0.5f;

    [Header("Тип Стрельбы")]
    public AttackWeaponType WeaponType = AttackWeaponType.Burst;
}

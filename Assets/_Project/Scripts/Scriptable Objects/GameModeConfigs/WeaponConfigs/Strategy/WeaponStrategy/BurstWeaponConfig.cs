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
    public float ReloadTime => BurstDelay;
    public GameObject ShootEffectPrefab => _prefabShootEffect;
    GameObject IShootConfig.BulletPrefab => BulletConfig.Prefab;
    float IShootConfig.BulletSpeed => BulletConfig.Speed;
    int IMagazineWeapon.MaxAmmo => BurstSize;
    float IShootConfig.RecoilForce => RecoilForce;

    [Header("🔊 Audio")]
    public AudioClip ShootSound;
    [Range(0f, 1f)] public float ShootVolume = 0.5f;
}

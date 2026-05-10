using UnityEngine;

[CreateAssetMenu(fileName = "SniperShootConfig", menuName = "Scriptable Objects/SniperShootConfig")]
public class SniperShootConfig : ScriptableObject, IShootConfig
{
    [Header("⚡ShootModule")]
    public float FireRate;
    public BulletConfig BulletConfig;

    [Header("🎨Visual")]
    public GameObject _shootEffectPrefab;
    public GameObject ShootEffectPrefab => _shootEffectPrefab;

    public float RecoilForce = 5f;
    public float HitForce = 3f;

    float IShootConfig.BulletSpeed => BulletConfig.Speed;
    float IShootConfig.RecoilForce => RecoilForce;
    float IShootConfig.FireRate => FireRate;
    float IShootConfig.HitForce => HitForce;
    GameObject IShootConfig.BulletPrefab => BulletConfig.Prefab;
    public bool _isMarkTarget;

    [Header("🔊 Audio")]
    public AudioClip ShootSound;
    [Range(0f, 1f)] public float ShootVolume = 0.5f;

    [Header("Тип Стрельбы")]
    public AttackWeaponType WeaponType = AttackWeaponType.Sniper;
}

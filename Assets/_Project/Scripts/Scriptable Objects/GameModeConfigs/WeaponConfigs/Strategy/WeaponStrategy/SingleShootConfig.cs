using UnityEngine;

[CreateAssetMenu(fileName = "SingleShootConfig", menuName = "Scriptable Objects/SingleShootConfig")]
public class SingleShootConfig : ScriptableObject, IShootConfig
{
    [Header("⚡ShootModule")]
    public float FireRate;
    public BulletConfig BulletConfig;

    [Header("🎨Visual")]
    public GameObject _shootEffectPrefab;
    public GameObject ShootEffectPrefab => _shootEffectPrefab;

    public float RecoilForce = 5f;
    public float HitForce = 3f;

    GameObject IShootConfig.BulletPrefab => BulletConfig.Prefab;
    float IShootConfig.BulletSpeed => BulletConfig.Speed;
    float IShootConfig.RecoilForce => RecoilForce;
    float IShootConfig.FireRate => FireRate;
    float IShootConfig.HitForce => HitForce;

    [Header("🔊 Audio")]
    public AudioClip ShootSound;
    [Range(0f, 1f)] public float ShootVolume = 0.5f;

    [Header("Тип Стрельбы")]
    public AttackWeaponType WeaponType = AttackWeaponType.Single;
}

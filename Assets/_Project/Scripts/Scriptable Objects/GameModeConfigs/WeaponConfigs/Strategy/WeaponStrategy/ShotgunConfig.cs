using UnityEngine;

[CreateAssetMenu(fileName = "ShotgunConfig", menuName = "Scriptable Objects/ShotgunConfig")]
public class ShotgunConfig : ScriptableObject, IShootConfig
{
    [Header("⚡ShootModule")]
    public float FireRate;
    public BulletConfig BulletConfig;

    [Header("🎨Visual")]
    public GameObject _shootEffectPrefab;
    public GameObject ShootEffectPrefab => _shootEffectPrefab;

    public float RecoilForce = 5f;
    public float HitForce = 3f;
    public int BulletCount = 5;
    public int SpreadAngle = 30;

    float IShootConfig.BulletSpeed => BulletConfig.Speed;
    float IShootConfig.RecoilForce => RecoilForce;
    float IShootConfig.HitForce => HitForce;
    float IShootConfig.FireRate => FireRate;
    GameObject IShootConfig.BulletPrefab => BulletConfig.Prefab;

    [Header("🔊 Audio")]
    public AudioClip ShootSound;
    [Range(0f, 1f)] public float ShootVolume = 0.5f;

    [Header("Тип Стрельбы")]
    public AttackWeaponType WeaponType = AttackWeaponType.Shotgun;
}

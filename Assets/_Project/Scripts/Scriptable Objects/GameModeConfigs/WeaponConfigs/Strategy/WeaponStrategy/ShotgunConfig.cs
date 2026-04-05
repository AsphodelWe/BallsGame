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

    public int BulletCount = 5;

    public float SpreadAngle = 30f;

    GameObject IShootConfig.BulletPrefab => BulletConfig.Prefab;
    float IShootConfig.BulletSpeed => BulletConfig.Speed;
    float IShootConfig.RecoilForce => RecoilForce;

    [Header("🔊 Audio")]
    public AudioClip ShootSound;
    [Range(0f, 1f)] public float ShootVolume = 0.5f;
}

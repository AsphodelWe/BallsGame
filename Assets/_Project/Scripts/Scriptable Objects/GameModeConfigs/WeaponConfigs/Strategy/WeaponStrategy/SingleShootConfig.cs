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

    GameObject IShootConfig.BulletPrefab => BulletConfig.Prefab;
    float IShootConfig.BulletSpeed => BulletConfig.Speed;
    float IShootConfig.RecoilForce => RecoilForce;
}

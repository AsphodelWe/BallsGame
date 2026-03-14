using UnityEngine;

[CreateAssetMenu(fileName = "SingleShootConfig", menuName = "Scriptable Objects/SingleShootConfig")]
public class SingleShootConfig : ScriptableObject, IShootConfig
{
    [Header("⚡ShootModule")]
    public float FireRate;
    public BulletConfig BulletConfig;

    GameObject IShootConfig.BulletPrefab => BulletConfig.Prefab;
    float IShootConfig.BulletSpeed => BulletConfig.Speed;
}

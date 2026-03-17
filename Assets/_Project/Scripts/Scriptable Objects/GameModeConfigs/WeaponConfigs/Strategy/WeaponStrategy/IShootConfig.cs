using UnityEngine;

public interface IShootConfig
{
    GameObject BulletPrefab { get; }
    GameObject ShootEffectPrefab { get; }
    float BulletSpeed { get; }
}

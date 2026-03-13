using UnityEngine;

public interface IShootConfig
{
    GameObject BulletPrefab { get; }
    float BulletSpeed { get; }
}

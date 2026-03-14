using UnityEngine;

[CreateAssetMenu(fileName = "BulletConfig", menuName = "Scriptable Objects/BulletConfig")]
public class BulletConfig : ScriptableObject
{
    public GameObject Prefab;
    public float Speed;
    public float LifeTime;
}

using UnityEngine;

[CreateAssetMenu(fileName = "MapConfig", menuName = "Scriptable Objects/MapConfig")]
public class MapConfig : ScriptableObject
{
    public string MapName;
    [Header("Preview")]
    public GameObject PreviewPrefab;
    [Header("Battle")]
    public GameObject BattlePrefab;
    public GameObject SpawnPreview()
    {
        return Instantiate(PreviewPrefab, Vector2.zero, Quaternion.identity);
    }
}

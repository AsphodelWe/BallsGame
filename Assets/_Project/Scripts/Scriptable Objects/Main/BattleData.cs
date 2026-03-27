using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "Scriptable Objects/BattleData")]
public class BattleData : ScriptableObject
{
    [System.NonSerialized] private MapConfig _selectedMap;
    [System.NonSerialized] private List<Ghost> _ghostList = new();
    [System.NonSerialized] private bool _rotateMap = false;
    [System.NonSerialized] private float _mapScale = 1f;

    public MapConfig SelectedMap => _selectedMap;
    public IReadOnlyList<Ghost> GhostList => _ghostList;

    public void SetSelectedMap(MapConfig map) => _selectedMap = map;
    public void AddGhost(Ghost ghost) => _ghostList.Add(ghost);
    public void RemoveGhost(Ghost ghost) => _ghostList.Remove(ghost);
    public void RemoveAllGhost() => _ghostList.Clear();
    public float MapScale => _mapScale;
    public void SetMapScale(float scale) => _mapScale = scale;
    public bool RotateMap => _rotateMap;
    public void SetRotateMap(bool rotate) => _rotateMap = rotate;
}

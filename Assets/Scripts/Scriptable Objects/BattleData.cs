using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BattleData", menuName = "Scriptable Objects/BattleData")]
public class BattleData : ScriptableObject
{
    [System.NonSerialized] private MapConfig _selectedMap;
    [System.NonSerialized] private List<Ghost> _ghostList = new();

    public MapConfig SelectedMap => _selectedMap;
    public IReadOnlyList<Ghost> GhostList => _ghostList;
    
    public void SetSelectedMap(MapConfig map) => _selectedMap = map;
    public void AddGhost(Ghost ghost) => _ghostList.Add(ghost);
}

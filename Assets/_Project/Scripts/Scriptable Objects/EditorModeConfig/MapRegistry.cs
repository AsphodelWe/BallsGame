using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "MapRegistry", menuName = "Scriptable Objects/MapRegistry")]
public class MapRegistry : ScriptableObject
{
    [SerializeField] private MapConfig[] _maps;
    [SerializeField] private int _selectedIndex;
    public MapConfig[] AllMaps => _maps;
    public MapConfig GetMap(int index) => _maps[index];
    public MapConfig SelectedMap => _maps[_selectedIndex];
    public void SelectMap(int index)
    {
        if (index >= 0 && index < _maps.Length)
            _selectedIndex = index;
    }
}

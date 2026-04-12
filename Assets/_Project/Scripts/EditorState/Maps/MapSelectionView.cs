using Reflex.Attributes;
using Unity.VisualScripting;
using UnityEngine;

public class MapSelectionView : MonoBehaviour, IMapView
{
    private GameObject _currentMap;
    public Collider2D GetCollider => _currentMap != null? _currentMap.GetComponent<Collider2D>(): null;
    public GameObject ShowMap(MapConfig mapConfig)
    {
        if (mapConfig == null)
        {
            Debug.LogError("MapConfig is null!");
            return null;
        }

        if (_currentMap != null)
        {
            Destroy(_currentMap);
            _currentMap = null;
        }

        _currentMap = mapConfig.SpawnPreview();

        if (_currentMap == null)
        {
            Debug.LogError($"Failed to spawn preview for {mapConfig.name}");
        }

        return _currentMap;
    }

    public void Hide() => gameObject.SetActive(false);

}

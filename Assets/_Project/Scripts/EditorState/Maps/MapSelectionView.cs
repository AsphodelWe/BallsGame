using Reflex.Attributes;
using Unity.VisualScripting;
using UnityEngine;

public class MapSelectionView : MonoBehaviour, IMapView
{
    private GameObject _currentMap;
    public Collider2D GetCollider => _currentMap.GetComponent<Collider2D>();
    public void ShowMap(MapConfig mapConfig)
    {
        if (_currentMap != null)
        {
            Destroy(_currentMap);
            _currentMap = null;
        }
        _currentMap = mapConfig.SpawnPreview();
    }
    public void Hide() => gameObject.SetActive(false);


}

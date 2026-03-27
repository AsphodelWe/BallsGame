using Reflex.Attributes;
using Unity.VisualScripting;
using UnityEngine;

public class MapSelectionView : MonoBehaviour, IMapView
{
    private GameObject _currentMap;
    public Collider2D GetCollider => _currentMap.GetComponent<Collider2D>();

    public GameObject ShowMap(MapConfig mapConfig)
    {
        if (_currentMap != null)
        {
            Destroy(_currentMap);
            _currentMap = null;
        }
        return _currentMap = mapConfig.SpawnPreview();
    }
    public void Hide() => gameObject.SetActive(false);


}

using UnityEngine;

public interface IMapView
{
    Collider2D GetCollider { get; }
    GameObject ShowMap(MapConfig mapConfig);
    void Hide();
}

using UnityEngine;

public interface IMapView
{
    Collider2D GetCollider { get; }
    void ShowMap(MapConfig mapConfig);
    void Hide();
}

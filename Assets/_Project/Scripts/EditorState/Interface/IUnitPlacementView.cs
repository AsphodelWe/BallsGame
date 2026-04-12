using UnityEngine;
using R3;
public interface IUnitPlacementView
{
    Vector3 GetWorldPosition();
    Observable<Vector3> GetMouseWorldPositionStream();
    Observable<Vector3> GetMouseClickStreamLeft();
    Observable<Unit> GetMouseClickStreamRight();
    LayerMask GetBallLayer { get; }
    void Show();
    void Hide();
}

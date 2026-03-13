using UnityEngine;
using R3;
public interface IUnitPlacementView
{
    Observable<Vector3> GetMouseWorldPositionStream();
    Observable<Vector3> GetMouseClickStream();
    CountryConfig GetCountryFromPosition(Vector3 worldPos);
    bool IsOverSprite(Vector3 worldPos, PlaceLayer placeLayer);
    void Show();
    void Hide();
}

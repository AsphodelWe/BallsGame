using UnityEngine;
using R3;
public interface IUnitPlacementView
{
    Observable<Vector3> GetMouseWorldPositionStream();
    Observable<Ghost> GetMouseClickStreamRight();
    Observable<Vector3> GetMouseClickStream();
    CountryConfig GetCountryFromPosition(Vector3 worldPos);
    void Show();
    void Hide();
}

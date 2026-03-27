using UnityEngine;
using UnityEngine.InputSystem;
using R3;
using System;

public class UnitPlacementView : MonoBehaviour, IUnitPlacementView
{
    [SerializeField] private LayerMask _ballLayer;
    [SerializeField] private LayerMask _groundLayer;

    [Header("Visual")]
    [SerializeField] private GameObject _visualContainer;

    public CountryConfig GetCountryFromPosition(Vector3 worldPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(worldPos, _ballLayer);
        if (hit != null)
        {
            var countryComponent = hit.GetComponent<CountryComponent>();
            return countryComponent?.CountryConfig;
        }
        return null;
    }
    public static Vector3 GetWorldPosition()
    {
        Vector3 screenPos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        return worldPos;
    }

    public Observable<Vector3> GetMouseWorldPositionStream()
    {
        return Observable.EveryUpdate()
            .Where(_ => Mouse.current != null)
            .Select(_ => GetWorldPosition());
    }

    public Observable<Vector3> GetMouseClickStream()
    {
        return GetMouseWorldPositionStream()
            .Where(_ => Mouse.current.leftButton.wasPressedThisFrame);
    }

    public Observable<Ghost> GetMouseClickStreamRight()
    {
        return Observable.EveryUpdate()
            .Where(_ => Mouse.current != null && Mouse.current.rightButton.wasPressedThisFrame)
            .Select(_ => GetWorldPosition())
            .Select(pos => GetGhostAtPosition(pos))
            .Where(ghost => ghost != null);
            
    }

    private Ghost GetGhostAtPosition(Vector3 worldPos)
    {
        Collider2D hit = Physics2D.OverlapPoint(worldPos, _ballLayer);
        Debug.Log(hit);
        return hit?.GetComponent<Ghost>();
    }

    public void Show()
    {
        gameObject.SetActive(true);
        _visualContainer.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}

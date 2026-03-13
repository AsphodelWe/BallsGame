using UnityEngine;
using R3;
using UnityEngine.InputSystem;
using Reflex.Attributes;
using UnityEngine.Video;
using System;

public enum PlaceLayer
{
    GroundLayer,
    BallLayer
}

public class BuildController
{
    [Inject] private IEventBus _eventBus;
    [Inject] private IUnitPlacementView _unitPlacementView;
    private Ghost _ghost;
    private Collider2D _mapBounds;
    private CompositeDisposable _disposables = new();
    public event Action<Ghost> OnGhostPlaced;

    public void StartStream()
    {
        GetMapCollider();
        SetupCountrySelection();
        SetupBuildingPlacement();
        SetupGhostMovement();
    }

    private void SetupCountrySelection()
    {

        _unitPlacementView.GetMouseClickStream()
            .Where(worldPos => _unitPlacementView.IsOverSprite(worldPos, PlaceLayer.BallLayer))
            .Select(worldPos => _unitPlacementView.GetCountryFromPosition(worldPos))
            .Where(country => country != null)
            .Subscribe(country => CreateGhost(country))

            .AddTo(_disposables);
    }

    private void SetupBuildingPlacement()
    {

        _unitPlacementView.GetMouseClickStream()
       .Where(_ => _ghost != null)
       .Where(pos => IsWithinMap(pos))
       .Subscribe(_ => PlaceGhost())
       .AddTo(_disposables);
    }

    private void SetupGhostMovement()
    {
        _unitPlacementView.GetMouseWorldPositionStream()
            .Where(_ => _ghost != null)
            .Subscribe(position =>
            {
                _ghost.SetPosition(position);
                _ghost.SetColor(IsWithinMap(position) ? Color.green : Color.red);
            })
            .AddTo(_disposables);
    }

    private void CreateGhost(CountryConfig country)
    {
        _ghost?.Destroy();
        _ghost = new Ghost(country, 0.5f);
    }

    private bool IsWithinMap(Vector3 position)
    {
        if (_mapBounds == null) return false;

        var result = _mapBounds.OverlapPoint(position);


        return result;
    }

    private void GetMapCollider()
    {
        _eventBus.OnMapBoundsReady
            .Subscribe(bounds =>
            {
                _mapBounds = bounds;
            })
            .AddTo(_disposables);
    }

    private void PlaceGhost()
    {
        if (_ghost == null) return;

        _ghost.Place();
        OnGhostPlaced?.Invoke(_ghost);

        _ghost = null;
    }

}

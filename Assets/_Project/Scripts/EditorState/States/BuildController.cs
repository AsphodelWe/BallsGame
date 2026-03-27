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
    [Inject] private IUnitPlacementUI _unitPlacementUI;
    [Inject] private GhostFacroty _ghostFactory;
    private Ghost _ghost;
    private Collider2D _mapBounds;
    private CompositeDisposable _disposables = new();
    public event Action<Ghost> OnGhostPlaced;
    public event Action<Ghost> OnGhostDeleted;
    public void Initialize()
    {
        _unitPlacementUI.OnSelectCountry += (country) => CreateGhost(country);

        GetMapCollider();
        SetupBuildingPlacement();
        SetupGhostMovement();
        SetupDeleteGhost();

    }

    private void SetupDeleteGhost()
    {
        _unitPlacementView.GetMouseClickStreamRight()
        .Subscribe(ghost =>
        {
            OnGhostDeleted.Invoke(ghost);
            ghost.Destroy();
        });

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
        _ghost = _ghostFactory.Create(country, 0.5f);
    }

    public bool IsWithinMap(Vector3 position)
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

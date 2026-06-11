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

public class BuildController : IDisposable
{
    [Inject] private IEventBus _eventBus;
    [Inject] private IUnitPlacementView _unitPlacementView;
    [Inject] private IUnitPlacementUI _unitPlacementUI;
    [Inject] private GhostFactory _ghostFactory;

    private Ghost _currentGhost;
    private Collider2D _mapBounds;
    private CompositeDisposable _disposables = new();
    public Subject<Ghost> OnGhostPlaced { get; } = new();
    public Subject<Ghost> OnGhostDeleted { get; } = new();

    public void Initialize()
    {
        _unitPlacementUI.OnSelectCountry
            .Subscribe(CreateGhost)
            .AddTo(_disposables);

        GetMapCollider();
        SetupGhostMovement();
        SetupGhostPlacement();
        SetupDeleteLogic();
    }

    private void GetMapCollider()
    {
        _eventBus.OnMapBoundsReady
            .Subscribe(bounds => _mapBounds = bounds)
            .AddTo(_disposables);
    }

    private void CreateGhost(CountryConfig country)
    {
        _currentGhost?.Destroy();
        _currentGhost = _ghostFactory.Create(country);

        var mousePos = _unitPlacementView.GetWorldPosition();
        if (IsPositionValid(mousePos))
        {
            _currentGhost.SetPosition(mousePos);
            _currentGhost.SetColor(IsWithinMap(mousePos) ? Color.green : Color.red);
        }
    }

    private void SetupGhostMovement()
    {
        _unitPlacementView.GetMouseWorldPositionStream()
            .Where(_ => _currentGhost != null)
            .Subscribe(position =>
            {
                _currentGhost.SetPosition(position);

                bool isValid = IsValidPlacementPosition(position);
                _currentGhost.SetColor(isValid ? Color.green : Color.red);
            })
            .AddTo(_disposables);
    }

    private void SetupGhostPlacement()
    {
        _unitPlacementView.GetMouseClickStreamLeft()
            .Where(_ => _currentGhost != null)
            .Where(pos => IsValidPlacementPosition(pos))
            .Subscribe(_ => PlaceGhost())
            .AddTo(_disposables);

        Observable.EveryUpdate()
            .Where(_ => _currentGhost != null)
            .Where(_ => Mouse.current.leftButton.isPressed)
            .Select(_ => _unitPlacementView.GetWorldPosition())
            .Where(pos => IsValidPlacementPosition(pos))
            .Subscribe(_ => PlaceGhost())
            .AddTo(_disposables);
    }

    private void PlaceGhost()
    {
        if (_currentGhost == null) return;
        if (!IsWithinMap(_currentGhost.transform.position)) return;

        _currentGhost.Place();
        OnGhostPlaced.OnNext(_currentGhost);


//#if UNITY_ANDROID || UNITY_IOS || UNITY_WEBGL
        _currentGhost = null;
/* #else
        var country = _currentGhost.Country;
        _currentGhost = _ghostFactory.Create(country);

        var mousePos = _unitPlacementView.GetWorldPosition();
        if (IsPositionValid(mousePos))
        {
            _currentGhost.SetPosition(mousePos);
            _currentGhost.SetColor(IsWithinMap(mousePos) ? Color.green : Color.red);
        }
#endif */
    }

    private void SetupDeleteLogic()
    {
        _unitPlacementView.GetMouseClickStreamRight()
            .Subscribe(_ => TryDeleteUnderMouse())
            .AddTo(_disposables);
    }

    private void TryDeleteUnderMouse()
    {
        if (_currentGhost != null)
        {
            OnGhostDeleted.OnNext(_currentGhost);
            _currentGhost.Destroy();
            _currentGhost = null;
            return;
        }

        var mousePos = _unitPlacementView.GetWorldPosition();
        if (!IsPositionValid(mousePos)) return;

        Ghost placedGhost = GetGhostAtPosition(mousePos);
        if (placedGhost != null)
        {
            OnGhostDeleted.OnNext(placedGhost);
            placedGhost.Destroy();
        }
    }

    private Ghost GetGhostAtPosition(Vector3 worldPos)
    {
        var hit = Physics2D.OverlapPoint(worldPos, _unitPlacementView.GetBallLayer);
        return hit?.GetComponent<Ghost>();
    }

    public bool IsWithinMap(Vector3 position)
    {
        return _mapBounds != null && _mapBounds.OverlapPoint(position);
    }

    private bool IsPositionValid(Vector3 pos)
    {
        return !float.IsNaN(pos.x) &&
               !float.IsInfinity(pos.x) &&
               pos != Vector3.negativeInfinity &&
               pos != Vector3.zero;
    }

    public bool IsValidPlacementPosition(Vector3 position)
    {
        if (!IsWithinMap(position))
            return false;

        if (_currentGhost == null)
            return true;

        var collider = _currentGhost.GetComponent<Collider2D>();
        collider.enabled = false;

        var hit = Physics2D.OverlapPoint(position, _unitPlacementView.GetBallLayer);

        collider.enabled = true;

        return hit == null;
    }

    public void Dispose()
    {
        if (_currentGhost != null)
        {
            _currentGhost.Destroy();
            _currentGhost = null;
        }

        _disposables?.Dispose();
        OnGhostPlaced?.Dispose();
        OnGhostDeleted?.Dispose();
    }
}

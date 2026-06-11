using System;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class MobileUnitPlacementView : MonoBehaviour, IUnitPlacementView
{
    [Inject] private CameraController _cameraController;
    [Inject] private IEventBus _eventBus;
    [SerializeField] private UnitPlacementUI _ui;
    [SerializeField] private GameObject _visualContainer;
    [SerializeField] private LayerMask _ballLayer;
    private Camera _mainCamera;
    private Collider2D _mapBounds;
    private CompositeDisposable _disposables = new();

    private Subject<Vector3> _positionSubject = new();
    private Subject<Vector3> _leftClickSubject = new();
    private Subject<Unit> _rightClickSubject = new();

    public LayerMask GetBallLayer => _ballLayer;

    private CountryConfig _pendingCountry;
    

    private void Awake()
    {
        _mainCamera = _cameraController?.GetComponent<Camera>() ?? Camera.main;

        _eventBus.OnMapBoundsReady
            .Subscribe(bounds => _mapBounds = bounds)
            .AddTo(_disposables);

        EnhancedTouchSupport.Enable();
        InitializeStreams();
    }

    private void InitializeStreams()
    {
        EnhancedTouchSupport.Enable();

        Observable.EveryUpdate()
            .Where(_ => Touch.activeTouches.Count > 0)
            .Where(_ => Touch.activeTouches[0].phase == TouchPhase.Began)
            .Where(_ => !IsPointerOverUI())
            .Where(_ => IsPointerOverMap())
            .Select(_ => GetWorldPosition(Touch.activeTouches[0].screenPosition))
            .Where(pos => IsPositionValid(pos))
            .Do(pos =>
            {
                if (_pendingCountry != null)
                {
                    _ui.SpawnGhostForCountry(_pendingCountry);
                }
            })
            .Subscribe(pos => _leftClickSubject.OnNext(pos))
            .AddTo(_disposables);
    }

    public void PrepareForNewGhost(CountryConfig country)
    {
        _pendingCountry = country;
    }

    public Vector3 GetWorldPosition()
    {
        if (Touch.activeTouches.Count == 0) return Vector3.negativeInfinity;
        return GetWorldPosition(Touch.activeTouches[0].screenPosition);
    }

    private Vector3 GetWorldPosition(Vector2 screenPos)
    {
        if (_mainCamera == null) return Vector3.negativeInfinity;
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
            return Vector3.negativeInfinity;
        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        return worldPos;
    }

    private bool IsPointerOverMap()
    {
        if (_mapBounds == null) return false;
        var worldPos = GetWorldPosition(Touch.activeTouches[0].screenPosition);
        return _mapBounds.OverlapPoint(worldPos);
    }

    private bool IsPositionValid(Vector3 pos)
    {
        return !float.IsNaN(pos.x) && !float.IsInfinity(pos.x) &&
               pos != Vector3.negativeInfinity && pos != Vector3.zero;
    }

    public Observable<Vector3> GetMouseWorldPositionStream() => _positionSubject.AsObservable();
    public Observable<Vector3> GetMouseClickStreamLeft() => _leftClickSubject.AsObservable();
    public Observable<Unit> GetMouseClickStreamRight() => _rightClickSubject.AsObservable();

    public void Show()
    {
        gameObject.SetActive(true);
        _visualContainer.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
        _visualContainer.SetActive(false);
    }

    public void Dispose()
    {
        _disposables?.Dispose();
        _positionSubject?.Dispose();
        _leftClickSubject?.Dispose();
        _rightClickSubject?.Dispose();
    }

    private bool IsPointerOverUI()
    {
        if (Touch.activeTouches.Count == 0) return false;

        var touch = Touch.activeTouches[0];
        var eventSystem = UnityEngine.EventSystems.EventSystem.current;

        if (eventSystem == null) return false;

        return eventSystem.IsPointerOverGameObject(touch.touchId);
    }


    private void OnDestroy() => Dispose();
}

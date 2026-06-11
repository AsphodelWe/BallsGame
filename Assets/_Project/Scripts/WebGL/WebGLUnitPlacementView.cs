using System;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;

public class WebGLUnitPlacementView : MonoBehaviour, IUnitPlacementView
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
    private bool _isMobile;

    private void Awake()
    {
        _mainCamera = _cameraController?.GetComponent<Camera>() ?? Camera.main;

        _eventBus.OnMapBoundsReady
            .Subscribe(bounds => _mapBounds = bounds)
            .AddTo(_disposables);

        _isMobile = DeviceDetector.IsMobile();

        if (_isMobile)
            EnhancedTouchSupport.Enable();


        InitializeStreams();
    }

    private void InitializeStreams()
    {
        if (_isMobile) InitializeTouchStreams();
        else InitializeMouseStreams();
    }

    private void InitializeTouchStreams()
    {
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
                    _ui.SpawnGhostForCountry(_pendingCountry);
            })
            .Subscribe(pos => _leftClickSubject.OnNext(pos))
            .AddTo(_disposables);
    }

    private void InitializeMouseStreams()
    {


        Observable.EveryUpdate()
            .Where(_ => Mouse.current != null)
            .Where(_ => Mouse.current.leftButton.wasPressedThisFrame)
            .Where(_ => !IsPointerOverUI_Mouse())
            .Where(_ => IsPointerOverMap_Mouse())
            .Select(_ => GetWorldPosition_Mouse())
            .Where(pos => IsPositionValid(pos))
            .Do(pos =>
            {
                if (_pendingCountry != null)
                    _ui.SpawnGhostForCountry(_pendingCountry);
            })
            .Subscribe(pos => _leftClickSubject.OnNext(pos))
            .AddTo(_disposables);
    }

    public void PrepareForNewGhost(CountryConfig country) => _pendingCountry = country;

    public Vector3 GetWorldPosition() =>
        _isMobile ? GetWorldPosition(Touch.activeTouches[0].screenPosition) : GetWorldPosition_Mouse();

    private Vector3 GetWorldPosition(Vector2 screenPos)
    {
        if (_mainCamera == null) return Vector3.negativeInfinity;
        if (screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y < 0 || screenPos.y > Screen.height)
            return Vector3.negativeInfinity;
        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        return worldPos;
    }

    private Vector3 GetWorldPosition_Mouse() => GetWorldPosition(Mouse.current.position.ReadValue());

    private bool IsPointerOverMap() =>
        _isMobile ? IsPointerOverMap_Touch() : IsPointerOverMap_Mouse();

    private bool IsPointerOverMap_Touch() =>
        _mapBounds?.OverlapPoint(GetWorldPosition(Touch.activeTouches[0].screenPosition)) ?? false;

    private bool IsPointerOverMap_Mouse() =>
        _mapBounds?.OverlapPoint(GetWorldPosition_Mouse()) ?? false;

    private bool IsPointerOverUI()
    {
        var eventSystem = UnityEngine.EventSystems.EventSystem.current;
        if (eventSystem == null) return false;

        if (_isMobile)
        {
            if (Touch.activeTouches.Count == 0) return false;
            return eventSystem.IsPointerOverGameObject(Touch.activeTouches[0].touchId);
        }
        return eventSystem.IsPointerOverGameObject();
    }

    private bool IsPointerOverUI_Mouse() => IsPointerOverUI();

    private bool IsPositionValid(Vector3 pos) =>
        !float.IsNaN(pos.x) && !float.IsInfinity(pos.x) &&
        pos != Vector3.negativeInfinity && pos != Vector3.zero;

    public Observable<Vector3> GetMouseWorldPositionStream() => _positionSubject.AsObservable();
    public Observable<Vector3> GetMouseClickStreamLeft() => _leftClickSubject.AsObservable();
    public Observable<Unit> GetMouseClickStreamRight() => _rightClickSubject.AsObservable();

    public void Show() { gameObject.SetActive(true); _visualContainer.SetActive(true); }
    public void Hide() { gameObject.SetActive(false); _visualContainer.SetActive(false); }

    public void Dispose()
    {
        _disposables?.Dispose();
        _positionSubject?.Dispose();
        _leftClickSubject?.Dispose();
        _rightClickSubject?.Dispose();
    }

    private void OnDestroy() => Dispose();
}

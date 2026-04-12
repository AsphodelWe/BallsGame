using UnityEngine;
using UnityEngine.InputSystem;
using R3;
using System;
using Reflex.Attributes;
using Reflex.Extensions;

public class UnitPlacementView : MonoBehaviour, IUnitPlacementView
{
    [Inject] private CameraController _cameraController;
    [SerializeField] private GameObject _visualContainer;
    [SerializeField] private LayerMask _ballLayer;
    private Camera _mainCamera;
    private CompositeDisposable _disposables = new();
    
    private Subject<Vector3> _mousePositionSubject = new();
    private Subject<Vector3> _mouseLeftClickSubject = new();
    private Subject<Unit> _mouseRightClickSubject = new();

    public LayerMask GetBallLayer => _ballLayer;
    
    private void Awake()
    {
        _mainCamera = _cameraController?.GetComponent<Camera>() ?? Camera.main;
        InitializeStreams();
    }
    
    private void InitializeStreams()
    {
        // Позиция мыши (каждый кадр при движении)
        Observable.EveryUpdate()
            .Where(_ => IsMouseValid())
            .Select(_ => GetWorldPosition())
            .Where(pos => IsPositionValid(pos))
            .DistinctUntilChanged()
            .Subscribe(pos => _mousePositionSubject.OnNext(pos))
            .AddTo(_disposables);
        
        // Левый клик
        Observable.EveryUpdate()
            .Where(_ => IsMouseValid())
            .Where(_ => Mouse.current.leftButton.wasPressedThisFrame)
            .Select(_ => GetWorldPosition())
            .Where(pos => IsPositionValid(pos))
            .Subscribe(pos => _mouseLeftClickSubject.OnNext(pos))
            .AddTo(_disposables);
        
        // Правый клик
        Observable.EveryUpdate()
            .Where(_ => IsMouseValid())
            .Where(_ => Mouse.current.rightButton.wasPressedThisFrame)
            .Subscribe(_ => _mouseRightClickSubject.OnNext(Unit.Default))
            .AddTo(_disposables);
    }
    
    private bool IsMouseValid()
    {
        if (Mouse.current == null || _mainCamera == null) return false;
        
        var eventSystem = UnityEngine.EventSystems.EventSystem.current;
        if (eventSystem != null && eventSystem.IsPointerOverGameObject())
            return false;
            
        return true;
    }
    
    private bool IsPositionValid(Vector3 pos)
    {
        return !float.IsNaN(pos.x) && 
               !float.IsInfinity(pos.x) && 
               pos != Vector3.negativeInfinity &&
               pos != Vector3.zero;
    }
    
    public Vector3 GetWorldPosition()
    {
        if (_mainCamera == null) return Vector3.negativeInfinity;
        
        Vector3 screenPos = Mouse.current.position.ReadValue();
        
        if (screenPos.x < 0 || screenPos.x > Screen.width || 
            screenPos.y < 0 || screenPos.y > Screen.height)
            return Vector3.negativeInfinity;
            
        Vector3 worldPos = _mainCamera.ScreenToWorldPoint(screenPos);
        worldPos.z = 0;
        return worldPos;
    }
    
    // Публичные стримы (Observable)
    public Observable<Vector3> GetMouseWorldPositionStream() => _mousePositionSubject.AsObservable();
    public Observable<Vector3> GetMouseClickStreamLeft() => _mouseLeftClickSubject.AsObservable();
    public Observable<Unit> GetMouseClickStreamRight() => _mouseRightClickSubject.AsObservable();
    
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
        _mousePositionSubject?.Dispose();
        _mouseLeftClickSubject?.Dispose();
        _mouseRightClickSubject?.Dispose();
    }
    
    private void OnDestroy() => Dispose();
}

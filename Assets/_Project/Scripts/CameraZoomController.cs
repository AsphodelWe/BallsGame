using DG.Tweening;
using R3;
using Reflex.Attributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraZoomController : MonoBehaviour
{
    [Inject] private MapScaler _mapScaler;
    [SerializeField] private float _zoomSpeed = 1f;
    [SerializeField] private float _minZoom = 3f;
    [SerializeField] private float _maxZoom = 10f;
    [SerializeField] private float _timeChangeCam = 0.1f;
    private Camera _cam;
    private float _currentZoom;
    private CompositeDisposable _disposables = new();

    private float _initialCameraSize;

    public void Awake()
    {
        if (_cam == null)
            _cam = GetComponent<Camera>();


    }

    void Start()
    {
        _initialCameraSize = _cam.orthographicSize;
        _mapScaler.OnScaleChanged += (value) => OnMapScaleChanged(value);

        Observable.EveryUpdate()
    .Select(_ => Mouse.current.scroll.ReadValue().y)
    .Where(delta => delta != 0)
    .Subscribe(delta =>
    {
        _currentZoom -= delta * _zoomSpeed;
        _currentZoom = Mathf.Clamp(_currentZoom, _minZoom, _maxZoom);

        DOTween.Kill(_cam);
        _cam.DOOrthoSize(_currentZoom, _timeChangeCam);
    })
    .AddTo(_disposables);
    }


    private void OnMapScaleChanged(float scale)
    {
        _cam.orthographicSize = _initialCameraSize * scale;
    }

    void OnDestroy()
    {
        _disposables.Dispose();
        DOTween.Kill(_cam);
    }
}

using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private CameraEffectsConfig _cameraEffectsConfig;
    [SerializeField] private CameraSettings _cameraSettings;
    private float _initialCameraSize;
    private Camera _cam;
    private MapScaler _mapScaler;
    private Tween _verticalBobTween;
    private Tween _horizontalBobTween;
    private Tween _breathingTween;
    private Tween _circleTween;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        _cam = GetComponent<Camera>();
    }

    void Start()
    {
        _initialCameraSize = _cam.orthographicSize;
    }

    public void SetMapScaler(MapScaler mapScaler)
    {
        _mapScaler = mapScaler;
        _mapScaler.OnScaleChanged += (value) => OnMapScaleChanged(value);
    }

    private void OnMapScaleChanged(float scale)
    {
        _cam.orthographicSize = _initialCameraSize * scale;
    }

    public void SetCameraZoom(float orthographicSize)
    {
        if (_cam == null)
            _cam = GetComponent<Camera>();

        _cam.orthographicSize = orthographicSize;
    }

    public void SetEditorPosition() => gameObject.transform.position = _cameraSettings.EditorPosition;
    public void SetBattlePosition() => gameObject.transform.position = _cameraSettings.BattlePosition;
    public void SetEditorZoom() => _cam.orthographicSize = _cameraSettings.EditorZoom;

    public void VerticalBob()
    {
        _verticalBobTween?.Kill();
        _verticalBobTween = transform.DOLocalMoveY(_cameraSettings.BattlePosition.y + _cameraEffectsConfig.BobHeight, _cameraEffectsConfig.BobDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void HorizontalBob()
    {
        _horizontalBobTween?.Kill();
        _horizontalBobTween = transform.DOLocalMoveX(_cameraSettings.BattlePosition.x + _cameraEffectsConfig.BobWidth, _cameraEffectsConfig.BobDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void Breathing()
    {
        _breathingTween?.Kill();
        _breathingTween = _cam.DOOrthoSize(_cameraSettings.EditorZoom + _cameraEffectsConfig.ZoomAmplitude, _cameraEffectsConfig.ZoomDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

    public void BeginCirclePath()
    {
        _circleTween?.Kill();

        Vector3[] path = new Vector3[_cameraEffectsConfig.CircleSegments];
        float angleStep = 360f / _cameraEffectsConfig.CircleSegments;

        for (int i = 0; i < _cameraEffectsConfig.CircleSegments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            path[i] = _cameraSettings.BattlePosition + new Vector3(Mathf.Cos(angle), Mathf.Sin(angle)) * _cameraEffectsConfig.CircleRadius;
        }

        _circleTween = transform.DOLocalPath(path, _cameraEffectsConfig.CircleDuration, _cameraEffectsConfig.CirclePathType)
            .SetLoops(-1, _cameraEffectsConfig.CircleLoopType);
    }


    void OnDestroy()
    {

        if (_mapScaler != null)
            _mapScaler.OnScaleChanged -= OnMapScaleChanged;

        _verticalBobTween?.Kill();
        _horizontalBobTween?.Kill();
        _breathingTween?.Kill();
        _circleTween?.Kill();
    }
}

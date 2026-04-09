using DG.Tweening;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Vector3 _editorPosition = new Vector3(0, 0, -10);
    [SerializeField] private Vector3 _battlePosition = new Vector3(0, 0.5f, -10);
    [SerializeField] private float _editorZoom = 5f;
    private Camera _cam;
    private float _initialCameraSize;
    private static CameraController _instance;
    private MapScaler _mapScaler;

    [SerializeField] private float bobHeight = 0.2f;
    [SerializeField] private float bobDuration = 1.5f;

    private Tween _bobTween;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        if (_cam == null)
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

    void OnDestroy()
    {

        if (_mapScaler != null)
            _mapScaler.OnScaleChanged -= OnMapScaleChanged;

        _bobTween?.Kill();
    }

    public void SetEditorPosition() => gameObject.transform.position = _editorPosition;
    public void SetBattlePosition() => gameObject.transform.position = _battlePosition;
    public void SetEditorZoom() => _cam.orthographicSize = _editorZoom;

    public void BeginBob()
    {
        _bobTween?.Kill();

        _bobTween = transform.DOLocalMoveY(_battlePosition.y + bobHeight, bobDuration)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo);
    }

}

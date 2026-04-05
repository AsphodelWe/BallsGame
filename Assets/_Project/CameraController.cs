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
    }

    public void SetEditorPosition() => gameObject.transform.position = _editorPosition;
    public void SetBattlePosition() => gameObject.transform.position = _battlePosition;
    public void SetEditorZoom() => _cam.orthographicSize = _editorZoom;
}

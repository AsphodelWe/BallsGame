using UnityEngine;

public class CameraBattleController : MonoBehaviour
{
    private Camera _cam;
    public void SetCameraZoom(float orthographicSize)
    {
        if (_cam == null)
            _cam = GetComponent<Camera>();
            
        _cam.orthographicSize = orthographicSize;
    }
}

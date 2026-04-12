using UnityEngine;

[CreateAssetMenu(fileName = "CameraSettings", menuName = "Scriptable Objects/CameraSettings")]
public class CameraSettings : ScriptableObject
{
    public Vector3 EditorPosition = new Vector3(0, 0, -10);
    public Vector3 BattlePosition = new Vector3(0, 0.5f, -10);
    public float EditorZoom = 5f;
}

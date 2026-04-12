using UnityEngine;

public interface ITimeManager
{
    void Pause();
    void Resume();
    void SetScale(float scale);
    float GetScale();
}

using UnityEngine;

public class TimeManager : ITimeManager
{
    public void Pause() => Time.timeScale = 0;
    public void Resume() => Time.timeScale = 1;
    public void SetScale(float scale) => Time.timeScale = scale;
    public float GetScale() => Time.timeScale;
}

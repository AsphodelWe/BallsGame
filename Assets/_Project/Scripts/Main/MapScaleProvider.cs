using UnityEngine;

public class MapScaleProvider : IMapScaleProvider
{
    public float CurrentScale { get; set; }
    public float BaseScale { get; set; }
    public float ScaleMultiplier => CurrentScale / BaseScale;
}

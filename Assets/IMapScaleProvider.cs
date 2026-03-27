using UnityEngine;

public interface IMapScaleProvider
{
    float CurrentScale { get; set;}
    float BaseScale{get;set;}
    float ScaleMultiplier => CurrentScale/BaseScale;
}

using System;
using Reflex.Attributes;
using UnityEngine;

public class MapScaler
{
    [Inject] private MapRegistry _mapRegistry;
    [Inject] private IMapScaleProvider _mapScaleProvider;
    public event Action<float> OnScaleChanged;
    private float _scale;
    public void Initialize()
    {
        if (_mapRegistry.PlayableMap != null)
        {
            _scale = _mapRegistry.PlayableMap.transform.localScale.x;
            _mapScaleProvider.BaseScale = _scale;
            _mapScaleProvider.CurrentScale = _scale;
            OnScaleChanged?.Invoke(_scale);
        }
    }

    public void ChangeScale(float scaleValue)
    {
        _scale = scaleValue;
        var map = _mapRegistry.PlayableMap;
        map.transform.localScale = Vector3.one * scaleValue;
        _mapScaleProvider.CurrentScale = scaleValue;
        OnScaleChanged?.Invoke(_scale);
    }

    public float GetScale => _scale;
}

using UnityEngine;

public class GlobalServices
{
    public static IMapScaleProvider MapScaleProvider { get; } = new MapScaleProvider();
}

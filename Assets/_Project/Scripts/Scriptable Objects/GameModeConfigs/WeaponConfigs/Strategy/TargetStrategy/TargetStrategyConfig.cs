using UnityEngine;

public abstract class TargetStrategyConfig : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "";
}

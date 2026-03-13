using UnityEngine;

public class SideBattleData
{
    public SideConfig Config { get; }
    
    public SideBattleData(SideConfig config)
    {
        Config = config;
    }
    public bool IsEnemy(SideBattleData other) => other.Config == Config.EnemySide;
}

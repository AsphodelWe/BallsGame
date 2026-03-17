using UnityEngine;

public class BallData
{
    public string CountryName { get; }
    public int MaxHealth { get; }
    public SideConfig Side { get; }
    public BallPhysicsConfig PhysicsConfig { get; }
    public Sprite Flag;
    public BallData(CountryConfig config)
    {
        CountryName = config.CountryName;
        MaxHealth = config.CountryGameplayConfig.MaxHealth;
        Side = config.Side;
        PhysicsConfig = config.PhysicsConfig;
        Flag = config.CountryFlag;
    }
}

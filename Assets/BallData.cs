using UnityEngine;

public class BallData
{
    public string CountryName { get; }
    public int MaxHealth { get; }
    public int Damage { get; }
    public float CooldownWeapon { get; }
    public SideConfig Side { get; }
    public BallPhysicsConfig _physicsConfig { get; }
    public BallData(CountryConfig config)
    {
        CountryName = config.CountryName;
        MaxHealth = config.CountryGameplayConfig.MaxHealth;
        Damage = config.CountryGameplayConfig.Damage;
        CooldownWeapon = config.CountryGameplayConfig.CooldownWeapon;
        Side = config.Side;
        _physicsConfig = config.PhysicsConfig;
    }
}

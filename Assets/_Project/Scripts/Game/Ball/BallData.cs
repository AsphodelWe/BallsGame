using UnityEngine;

public class BallData
{
    public string CountryName { get; }
    public BallPhysicsConfig PhysicsConfig { get; }
    public Sprite Flag;
    public AttackerConfig SelectedAttacker { get; }
    public SideConfig SelectedSide { get; }
    public int SelectedSloIndex { get; }
    public int MaxHealth { get; }

    public BallData(CountryConfig config)
    {
        CountryName = config.CountryName;
        PhysicsConfig = config.PhysicsConfig;
        Flag = config.CountryFlag;

        SelectedAttacker = config.SelectedAttacker ?? config.DefaultAttacker;
        SelectedSloIndex = config.SelectedSlotIndex >= 0 ? config.SelectedSlotIndex : config.DefaultAttackerSlotIndex;
        MaxHealth = config.CountryGameplayConfig.MaxHealth > 0? config.CountryGameplayConfig.MaxHealth: config.CountryGameplayConfig.DefaultMaxHealth;
        SelectedSide = config.Side?? config.DefaultSide;
    }
}

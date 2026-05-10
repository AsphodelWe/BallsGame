using System;
using System.Collections.Generic;
using ConditionalField;
using UnityEngine;

public enum AttackType
{
    Gun,
    Drone,
    Laser
}
public abstract class AttackerConfig : ScriptableObject
{
    public string Name;
    public GameObject Prefab;

    public int Damage = 10;
    public int DefaultDamage = 1;

    public virtual void SetupAttacker(Attacker attacker, SideConfig side) { }
    public abstract AttackType AttackerType { get; }
    public abstract List<AttackWeaponType> GetAllowedWeaponTypes();

    public int EffectiveDamage => Damage > 0 ? Damage : DefaultDamage;

    public TargetStrategyConfig TargetStrategyConfig;
    public Sprite ImageUI;
    public abstract bool IsTargetStrategyAllowed(TargetStrategyConfig strategy);
}

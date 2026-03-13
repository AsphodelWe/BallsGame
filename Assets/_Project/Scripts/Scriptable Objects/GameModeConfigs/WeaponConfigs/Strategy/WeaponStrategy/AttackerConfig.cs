using System;
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
    public GameObject Prefab;
    public int Damage = 10;
    public virtual void SetupAttacker(Attacker attacker, SideConfig side){}
    public abstract AttackType AttackerType {get;}
    public TargetStrategyConfig TargetStrategyConfig;
}

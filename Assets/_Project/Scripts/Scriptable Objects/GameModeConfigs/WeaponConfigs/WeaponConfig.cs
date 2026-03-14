using System;
using System.Collections.Generic;
using ConditionalField;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Scripts/Scriptable Objects/WeaponConfig")]
public class WeaponConfig : AttackerConfig
{
    //[SerializeField] private ScriptableObject _attackConfig;
    [SerializeField] private List<ScriptableObject> _modules;

/*     public IShootConfig AttackConfig
    {
        get => _attackConfig as IShootConfig;
        set => _attackConfig = value as ScriptableObject;
    } */
    public override AttackType AttackerType => AttackType.Gun;
    public override void SetupAttacker(Attacker attacker, SideConfig side)
    {
        if (attacker is Weapon weapon)
        {
            weapon.Damage = Damage;
            weapon.Side = side;
        }
    }

    public T GetModule<T>() where T : class
    {
        foreach (var module in _modules)
        {
            if (module is T result)
                return result;
        }
        return null;
    }
}

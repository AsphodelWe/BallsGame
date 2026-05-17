using System;
using System.Collections.Generic;
using System.Linq;
using ConditionalField;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponConfig", menuName = "Scripts/Scriptable Objects/WeaponConfig")]
public class WeaponConfig : AttackerConfig
{
    [SerializeField] private List<ScriptableObject> _modules;

    [SerializeField] private AttackWeaponType _activeWeaponType;

    [SerializeField] private bool HasMarkerAbility = false;

    public override AttackType AttackerType => AttackType.Gun;

    public T GetModule<T>() where T : class
    {
        foreach (var module in _modules)
        {
            if (module is T result)
                return result;
        }
        return null;
    }

    public T GetActiveModule<T>() where T : class
    {
        foreach (var module in _modules)
        {
            if (module is T && IsModuleOfActiveType(module))
                return module as T;
        }
        return GetModule<T>();
    }

    private bool IsModuleOfActiveType(ScriptableObject module)
    {
        return module switch
        {
            AutoWeaponConfig => _activeWeaponType == AttackWeaponType.Auto,
            BurstWeaponConfig => _activeWeaponType == AttackWeaponType.Burst,
            SingleShootConfig => _activeWeaponType == AttackWeaponType.Single,
            ShotgunConfig => _activeWeaponType == AttackWeaponType.Shotgun,
            SniperShootConfig => _activeWeaponType == AttackWeaponType.Sniper,
            _ => false
        };
    }

    public override List<AttackWeaponType> GetAllowedWeaponTypes()
    {
        List<AttackWeaponType> allowed = new List<AttackWeaponType>();

        foreach (var module in _modules)
        {
            if (module is AutoWeaponConfig) allowed.Add(AttackWeaponType.Auto);
            else if (module is BurstWeaponConfig) allowed.Add(AttackWeaponType.Burst);
            else if (module is SingleShootConfig) allowed.Add(AttackWeaponType.Single);
            else if (module is ShotgunConfig) allowed.Add(AttackWeaponType.Shotgun);
            else if (module is SniperShootConfig) allowed.Add(AttackWeaponType.Sniper);
        }

        return allowed;
    }

    public void SetActiveWeaponType(AttackWeaponType type)
    {
        var allowedTypes = GetAllowedWeaponTypes();
        if (allowedTypes.Contains(type))
        {
            _activeWeaponType = type;
        }
        else
        {
            Debug.LogWarning($"WeaponConfig: {Name} не поддерживает {type}. Установлен первый доступный.");
            _activeWeaponType = allowedTypes.FirstOrDefault();
        }
    }
    public AttackWeaponType GetActiveWeaponType() => _activeWeaponType;


    public override void SetupAttacker(Attacker attacker, SideConfig side)
    {
        if (attacker is Weapon weapon)
        {
            weapon.Damage = Damage;
            weapon.Side = side;
        }
    }

    public override bool IsTargetStrategyAllowed(TargetStrategyConfig strategy)
    {
        if (strategy is TrackTargetWithMarkConfig)
            return HasMarkerAbility;
        return true;
    }
}

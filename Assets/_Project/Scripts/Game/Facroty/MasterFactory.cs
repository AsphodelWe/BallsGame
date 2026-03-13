using System;
using Reflex.Attributes;
using Reflex.Core;
using Reflex.Injectors;
using UnityEngine;

public class MasterFactory : IMasterFactory
{
    [Inject] private Container _container;
    public Master CreateMaster(AttackerConfig config, SideConfig side, Transform slot)
    {
        return config switch
        {
            WeaponConfig weapon when weapon.AttackerType == AttackType.Gun =>
                CreateWeaponMaster(weapon, side, slot),

            _ => throw new ArgumentException($"Unknown config type: {config.GetType()}")
        };
    }

    private Master CreateWeaponMaster(WeaponConfig config, SideConfig side, Transform slot)
    {
        var masterObj = new GameObject("WeaponMaster");

        masterObj.transform.SetParent(slot);
        masterObj.transform.ResetLocal();

        var master = masterObj.AddComponent<WeaponMaster>();

        GameObjectInjector.InjectRecursive(masterObj, _container);
        master.Initialize(config, side, slot);


        return master;
    }


}

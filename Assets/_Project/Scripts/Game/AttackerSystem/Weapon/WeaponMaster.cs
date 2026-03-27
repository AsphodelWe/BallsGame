using System.Threading;
using Cysharp.Threading.Tasks;
using Reflex.Attributes;
using UnityEngine;

public class WeaponMaster : Master
{
    private Weapon _weapon => _attacker as Weapon;
    public override bool HasRecoil => true;
    public override void Initialize(AttackerConfig config, SideConfig side, Transform slot)
    {
        base.Initialize(config, side, slot);

        EquipAttacker();

        _weapon.OnRecoil += (dir, force) => TriggerRecoil(dir, force);
    }
}

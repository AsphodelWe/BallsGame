using Reflex.Attributes;
using UnityEngine;

public class WeaponFactory : AttackerFactory
{
    [Inject] private IWeaponAttackHandlerFactory _handlerFactory;
    public override Attacker CreateAttacker(AttackerConfig config, SideConfig side, Transform slot, BallPresent target = null)
    {
        var weaponConfig = config as WeaponConfig;
        if (weaponConfig == null) return null;

        var weapon = Object.Instantiate(weaponConfig.Prefab, slot).GetComponent<Weapon>();
        weapon.transform.ApplyCurrentScaleToWorld();

        weapon.AttackHandler = _handlerFactory.CreateHandler(weaponConfig.AttackConfig);

        SetupAttacker(weapon, config, side, target);

        return weapon;
    }
}

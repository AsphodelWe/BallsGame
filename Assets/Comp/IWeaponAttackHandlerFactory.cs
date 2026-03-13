using UnityEngine;

public interface IWeaponAttackHandlerFactory
{
    IWeaponAttackHandler CreateHandler(IShootConfig config);
}

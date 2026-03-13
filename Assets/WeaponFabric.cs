using UnityEngine;

public class WeaponFabric
{
    public Attacker CreateAttacker(AttackerConfig config, SideConfig side, Transform _slot, BallPresent target = null)
    {
        GameObject obj = Object.Instantiate(config.Prefab, _slot.position, Quaternion.identity);
        obj.transform.SetParent(_slot);
        Attacker attacker = obj.GetComponent<Attacker>();

        if (attacker == null) return null;

/*         config.InitializeAttacker(attacker, side);
        config.ValidateAndSetup(attacker, target); */

        return attacker;
    }
}

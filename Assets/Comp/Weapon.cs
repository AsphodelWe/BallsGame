using UnityEngine;

public class Weapon : Attacker
{
    [SerializeField] private Transform _firePoint;
    public Transform FirePoint => _firePoint;
    public IWeaponAttackHandler AttackHandler { get; set; }
    public override ITargetStrategy TargetStrategy { get; set; }

    private void Update()
    {
        TargetStrategy?.UpdateTarget(this);
        AttackHandler?.Attack(this);
    }
}

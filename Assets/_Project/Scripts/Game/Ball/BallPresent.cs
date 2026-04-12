using System;
using Reflex.Attributes;
using Reflex.Core;
using UnityEngine;

public class BallPresent : MonoBehaviour, IDamagable
{
    [Inject] private BallRegistry _ballRegistry;
    [Inject] IMasterFactory _masterFactory;

    [Header("Аттакер")]
    [SerializeField] private AttackerConfig _attackerConfig;
    [Header("Физика")]
    [SerializeField] private BallPhysics _ballPhysics;

    [Header("Вью")]
    [SerializeField] private BallView _ballView;

    [Header("Оружие")]
    [SerializeField] private Transform _attackerSlot;

    [Header("Здоровье")]
    [SerializeField] private BallHealth _ballHealth;
    private BallData _ballData;
    private Master _master;
    public event Action OnDestroyed;
    public void Initialize(BallData ballData)
    {
        _ballData = ballData;

        _ballPhysics.Initialize(_ballData.PhysicsConfig);
        _ballHealth.Initialize(_ballData.MaxHealth);
        _ballHealth.OnDied += Die;
        _ballView.Initialize(ballData.Flag);
        EquipAttacker(_attackerConfig);

    }
    public void EquipAttacker(AttackerConfig config)
    {
        _master = _masterFactory.CreateMaster(config, _ballData.Side, _attackerSlot);

        if (_master.HasRecoil)
            _master.OnRecoil += (dir, force) => _ballPhysics.ApplyForce(dir, force);

    }

    public void TakeDamage(int damage, Vector2 hitDirection, float hitForce)
    {
        _ballHealth.GetDamage(damage);
        _ballPhysics.ApplyForce(hitDirection, hitForce);
        _ballView.PlayHitEffect();
        _ballView.ShowDamageNumber(damage);
    }

    public void Die()
    {
        _ballView.PlayDeathEffect();
        _ballHealth.OnDied -= Die;
        _ballRegistry.UnRegister(this);
        Destroy(gameObject);
    }

    public SideConfig GetSide => _ballData.Side;
    public BallHealth Health => _ballHealth;
    public Sprite GetSprite => _ballView.GetFlag;

    void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }


}

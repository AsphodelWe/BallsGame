using System;
using Reflex.Attributes;
using Reflex.Core;
using UnityEngine;

public class BallPresent : MonoBehaviour, IDamagable
{
    [Inject] private BallRegistry _ballRegistry;
    [Inject] IMasterFactory _masterFactory;
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

    public void Inizialize(BallData ballData)
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
    }

    public void TakeDamage(int damage)
    {
        _ballHealth.GetDamage(damage);
    }

    public void Die()
    {
        _ballHealth.OnDied -= Die;
        _ballRegistry.UnRegister(this);
        Destroy(gameObject);
    }

    public SideConfig GetSide => _ballData.Side;
    public BallHealth Health => _ballHealth;
    public Sprite GetSprite => _ballView.GetFlag;
}

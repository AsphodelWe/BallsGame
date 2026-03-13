using Reflex.Attributes;
using Reflex.Core;
using UnityEngine;

public class BallPresent : MonoBehaviour
{
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

        _ballPhysics.Initialize(_ballData._physicsConfig);
        _ballHealth.Initialize(_ballData.MaxHealth);
        _ballView.Initialize();
        EquipAttacker(_attackerConfig);

    }
    public void EquipAttacker(AttackerConfig config)
    {
        _master = _masterFactory.CreateMaster(config, _ballData.Side, _attackerSlot);
    }

    public SideConfig GetSide => _ballData.Side;

}

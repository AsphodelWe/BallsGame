using System;
using System.Collections.Generic;
using System.Linq;
using Reflex.Attributes;
using Reflex.Core;
using UnityEngine;

public class BallPresent : MonoBehaviour, IDamagable
{
    [Inject] private BallRegistry _ballRegistry;
    [Inject] IMasterFactory _masterFactory;
    [SerializeField] private Transform _weaponSlots;
    private BallPhysics _ballPhysics;
    private BallView _ballView;
    private BallHealth _ballHealth;
    private BallData _ballData;
    private Master _master;
    public event Action OnDestroyed;

    private void Awake()
    {
        _ballPhysics = GetComponent<BallPhysics>();
        _ballView = GetComponent<BallView>();
        _ballHealth = GetComponent<BallHealth>();
    }

    public void Initialize(BallData ballData)
    {
        _ballData = ballData;
        _ballPhysics.Initialize(_ballData.PhysicsConfig);
        _ballHealth.Initialize(_ballData.MaxHealth);
        _ballView.Initialize(ballData.Flag);

        _ballHealth.OnDied += Die;

        var slot = _weaponSlots.Cast<Transform>().OrderBy(t => t.GetSiblingIndex()).ToArray()[ballData.SelectedSloIndex];

        EquipAttacker(ballData.SelectedAttacker, slot);

    }
    public void EquipAttacker(AttackerConfig weapon, Transform selectedSlot)
    {
        _master = _masterFactory.CreateMaster(weapon, _ballData.SelectedSide, selectedSlot);

        if (_master.HasRecoil)
            _master.OnRecoil += (dir, force) => _ballPhysics.ApplyForce(dir, force);

    }

    public void TakeDamage(int damage, Vector2 hitDirection, float hitForce)
    {
        _ballHealth.GetDamage(damage);
        _ballPhysics.ApplyForce(hitDirection, hitForce);
        _ballView.PlayHitEffect();
        _ballView.ShowDamageNumber(damage);
        AudioSource.PlayClipAtPoint(_ballData.HitSound, transform.position, _ballData.Volume);
    }

    public void Die()
    {
        _ballView.PlayDeathEffect();
        _ballHealth.OnDied -= Die;
        _ballRegistry.UnRegister(this);
        Destroy(gameObject);
    }

    public SideConfig GetSide => _ballData.SelectedSide;
    public BallHealth Health => _ballHealth;
    public Sprite GetSprite => _ballView.GetFlag;

    void OnDestroy()
    {
        OnDestroyed?.Invoke();
    }


}

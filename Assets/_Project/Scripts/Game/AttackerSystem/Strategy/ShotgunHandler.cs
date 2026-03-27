using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class ShotgunHandler: IWeaponAttackHandler, IDisposable
{
    private ShotgunConfig _config;
    private ObjectPool<Bullet> _bulletPool;
    private ObjectPool<ShootExplosion> _shotEffectPool;
    private CancellationTokenSource _cts;
    private CancellationTokenSource _linkedCts;
    private bool _isShooting;
    public ShotgunHandler(ShotgunConfig config, ObjectPool<Bullet> bulletPool, ObjectPool<ShootExplosion> effectPrefab)
    {
        _config = config;
        _bulletPool = bulletPool;
        _shotEffectPool = effectPrefab;
        _cts = new CancellationTokenSource();
    }

    public void Attack(Weapon weapon)
    {
        if (_isShooting) return;
        _isShooting = true;

        _linkedCts?.Cancel();
        _linkedCts?.Dispose();
        _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, weapon.destroyCancellationToken);

        ShootLoop(weapon, _linkedCts.Token).Forget();
    }

    private void CreateBullet(Weapon weapon)
    {
        var bullet = _bulletPool.Get();

        bullet.transform.position = weapon.FirePoint.position;
        bullet.transform.rotation = weapon.FirePoint.rotation;

        float angle = UnityEngine.Random.Range(-_config.SpreadAngle, _config.SpreadAngle) * Mathf.Deg2Rad;
        Vector2 direction = (Quaternion.Euler(0, 0, angle) * weapon.FirePoint.right).normalized;

        bullet.Initialize(
            config: _config.BulletConfig,
            damage: weapon.Damage,
            direction: direction,
            side: weapon.Side
        );

        weapon.TriggerRecoil(-weapon.FirePoint.right, _config.RecoilForce);
    }

    private void CreateShootEffect(Weapon weapon)
    {
        var effect = _shotEffectPool.Get();

        effect.transform.position = weapon.FirePoint.position;
        effect.transform.rotation = weapon.FirePoint.rotation;
    }

    private async UniTask Shoot(Weapon weapon, CancellationToken token)
    {

        for (int i = 0; i < _config.BulletCount; i++)
        {
            CreateBullet(weapon);
        }

        CreateShootEffect(weapon);

        await UniTask.Delay((int)(_config.FireRate * 1000), cancellationToken: token);
    }

    private async UniTaskVoid ShootLoop(Weapon weapon, CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                await Shoot(weapon, token);
            }
        }
        finally
        {
            _isShooting = false;
        }

    }

    public void Dispose()
    {
        _linkedCts?.Cancel();
        _linkedCts?.Dispose();
        _linkedCts = null;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;
    }
}

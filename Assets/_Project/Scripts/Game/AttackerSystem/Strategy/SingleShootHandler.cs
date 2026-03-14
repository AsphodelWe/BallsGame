using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class SingleShootHandler : IWeaponAttackHandler
{
    private SingleShootConfig _config;
    private ObjectPool<Bullet> _bulletPool;
    private CancellationTokenSource _cts;
    private bool _isShooting;
    public SingleShootHandler(SingleShootConfig config, ObjectPool<Bullet> bulletPool)
    {
        _config = config;
        _bulletPool = bulletPool;
        _cts = new CancellationTokenSource();
    }

    public void Attack(Weapon weapon)
    {
        if (_isShooting) return;

        _isShooting = true;
        ShootLoop(weapon, _cts.Token).Forget();
    }

    private void CreateBullet(Weapon weapon)
    {
        var bullet = _bulletPool.Get();

        bullet.transform.position = weapon.FirePoint.position;
        bullet.transform.rotation = weapon.FirePoint.rotation;

        bullet.Initialize(
            config: _config.BulletConfig,
            damage: weapon.Damage,
            direction: weapon.FirePoint.right,
            side: weapon.Side
        );
        bullet.transform.position = weapon.FirePoint.position;
    }

    private async UniTask Shoot(Weapon weapon, CancellationToken token)
    {
        CreateBullet(weapon);

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

    public void Stop()
    {
        _cts?.Cancel();
        _cts?.Dispose();
        _cts = new CancellationTokenSource();
    }

    private void OnDisable()
    {
        Stop();
    }

}

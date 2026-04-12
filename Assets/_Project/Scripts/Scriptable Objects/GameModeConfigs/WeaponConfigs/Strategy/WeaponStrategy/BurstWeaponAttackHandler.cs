using System;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;


public class BurstWeaponAttackHandler : IWeaponAttackHandler, IDisposable
{
    private BurstWeaponConfig _config;
    private ObjectPool<Bullet> _bulletPool;
    private ObjectPool<ShootExplosion> _shotEffectPool;
    private CancellationTokenSource _cts;
    private CancellationTokenSource _linkedCts;
    private int _currentAmmo;
    private bool _isReloading;
    private bool _isShooting;
    public BurstWeaponAttackHandler(BurstWeaponConfig config, ObjectPool<Bullet> bulletPool, ObjectPool<ShootExplosion> effectPrefab)
    {
        _config = config;
        _bulletPool = bulletPool;
        _shotEffectPool = effectPrefab;
        _currentAmmo = config.BurstSize;
        _cts = new CancellationTokenSource();
    }

    public void Attack(Weapon weapon)
    {
        if (_isReloading || _isShooting) return;

        _isShooting = true;
        _linkedCts = CancellationTokenSource.CreateLinkedTokenSource(_cts.Token, weapon.destroyCancellationToken);

        ShootLoop(weapon, _linkedCts.Token).Forget();
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
            side: weapon.Side,
            hitForce: _config.HitForce
        );

        Vector2 recoilDirection = -weapon.FirePoint.right;
        float recoilForce = _config.RecoilForce;
        
        weapon.TriggerRecoil(recoilDirection, recoilForce);
        PlayShootSound(weapon.FirePoint.position);
    }

    private void CreateShootEffect(Weapon weapon)
    {
        var effect = _shotEffectPool.Get();

        effect.transform.position = weapon.FirePoint.position;
        effect.transform.rotation = weapon.FirePoint.rotation;
    }

    private void PlayShootSound(Vector3 position)
    {
        if (_config.ShootSound == null) return;
        AudioSource.PlayClipAtPoint(_config.ShootSound, position, _config.ShootVolume);
    }
    private async UniTask Shoot(Weapon weapon, CancellationToken token)
    {
        CreateBullet(weapon);
        CreateShootEffect(weapon);
        _currentAmmo--;

        await UniTask.Delay((int)(_config.BulletDelay * 1000), cancellationToken: token);
    }

    private async UniTaskVoid ShootLoop(Weapon weapon, CancellationToken token)
    {
        try
        {
            float firstDelay = UnityEngine.Random.Range(0f, _config.BurstDelay);
            await UniTask.Delay((int)(firstDelay * 1000), cancellationToken: token);

            while (!token.IsCancellationRequested)
            {
                for (int i = 0; i < _config.BurstSize; i++)
                {
                    if (_currentAmmo <= 0) break;
                    await Shoot(weapon, token);
                }

                if (_currentAmmo <= 0)
                {
                    await Reload(token);
                }
                else
                {
                    await UniTask.Delay((int)(_config.BurstDelay * 1000), cancellationToken: token);
                }
            }
        }
        finally
        {
            _isShooting = false;
        }

    }

    private async UniTask Reload(CancellationToken token)
    {
        _isReloading = true;
        await UniTask.Delay((int)(_config.BurstDelay * 1000), cancellationToken: token);

        _currentAmmo = _config.BurstSize;
        _isReloading = false;
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

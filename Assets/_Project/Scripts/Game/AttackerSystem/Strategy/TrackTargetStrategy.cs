using Reflex.Attributes;
using UnityEngine;

public class TrackTargetStrategy : ITargetStrategy
{
    private BallRegistry _ballRegistry;
    private SideConfig _side;
    private BallPresent _currentTarget;
    private float _angleVelocity;
    private float _smoothTime;
    public BallPresent CurrentTarget => _currentTarget;
    public TrackTargetStrategy(BallRegistry ballRegistry, SideConfig side, float smoothTime)
    {
        _ballRegistry = ballRegistry;
        _side = side;
        _smoothTime = smoothTime;

        _ballRegistry.OnEnemyRemoved += OnEnemyRemoved;
        _ballRegistry.OnEnemyAdded += OnEnemySpawned;

        _currentTarget = _ballRegistry.FindAnyEnemy(_side);
    }


    public void SetTarget(BallPresent target)
    {
        _currentTarget = target;
    }

    public void UpdateTarget(Attacker attacker)
    {
        if (_currentTarget == null) return;

        Vector2 direction = (_currentTarget.transform.position - attacker.transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        float newAngle = Mathf.SmoothDampAngle(attacker.transform.eulerAngles.z, angle, ref _angleVelocity, _smoothTime);
        attacker.transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward);
    }

    private void OnEnemyRemoved(BallPresent enemy)
    {
        if (enemy == _currentTarget)
        {
            var newTarget = _ballRegistry.FindAnyEnemy(_side);
            if (newTarget != null)
            {
                _currentTarget = newTarget;
            }
            else
            {
                _currentTarget = null;
            }
        }
    }

    private void OnEnemySpawned(BallPresent enemy)
    {
        if (_currentTarget == null && enemy.GetSide != _side)
        {
            _currentTarget = enemy;
        }
    }

    public void Dispose()
    {
        _ballRegistry.OnEnemyRemoved -= OnEnemyRemoved;
        _ballRegistry.OnEnemyAdded -= OnEnemySpawned;
    }
}

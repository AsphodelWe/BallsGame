using R3;
using UnityEngine;


public class BallPhysics : MonoBehaviour
{
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _ballLayer;
    private BallPhysicsConfig _physicsConfig;
    private Rigidbody2D rb;
    private const float MIN_SPEED_THRESHOLD = 0.01f;
    private float _currentSpeedMultiplier;

    public void Initialize(BallPhysicsConfig physicsConfig)
    {
        rb = GetComponent<Rigidbody2D>();
        _physicsConfig = physicsConfig;
        _currentSpeedMultiplier = _physicsConfig.SpeedMultiplier;
        SetRandomStartDirection();
    }

    private void SetRandomStartDirection()
    {
        rb.linearVelocity = Random.insideUnitCircle.normalized * _physicsConfig.BaseSpeed;
    }

    void FixedUpdate()
    {
        MaintainSpeed();
        ApplyDecay();
    }

    private void MaintainSpeed()
    {
        if (rb.linearVelocity.magnitude < MIN_SPEED_THRESHOLD) return;

        float targetSpeed = _physicsConfig.BaseSpeed * _currentSpeedMultiplier;
        if (rb.linearVelocity.magnitude < targetSpeed * _physicsConfig.SpeedThreshold)
        {
            rb.AddForce(rb.linearVelocity.normalized * _physicsConfig.BoostForce, ForceMode2D.Force);
        }
    }

    private void ApplyDecay()
    {
        _currentSpeedMultiplier = Mathf.Max(_physicsConfig.MinSpeed,
            _currentSpeedMultiplier - Time.fixedDeltaTime * _physicsConfig.SpeedDecayRate);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (IsInLayerMask(collision.gameObject, _wallLayer))
        {
            SetCollisonSpeed(collision, _physicsConfig.WallBoostMultiplier);
            AddRandomWallLowKick(collision);
        }
        else if (IsInLayerMask(collision.gameObject, _ballLayer))
        {
            SetCollisonSpeed(collision, _physicsConfig.BallBoostMultiplier);
        }
    }

    private void SetCollisonSpeed(Collision2D collision, float _speedObjectMultiplier)
    {
        _currentSpeedMultiplier = Mathf.Min(_physicsConfig.MaxSpeedMultiplier, _currentSpeedMultiplier + _speedObjectMultiplier);
        Vector2 bump = collision.contacts[0].normal * _physicsConfig.BounceImpulse;
        rb.AddForce(bump, ForceMode2D.Impulse);
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }

    private void AddRandomWallLowKick(Collision2D collision)// Поворачиваем нормаль на случайный угол, чтобы избежать циклов
    {

        Vector2 normal = collision.contacts[0].normal;
        Vector2 tangent = new Vector2(-normal.y, normal.x);

        float direction = Mathf.Sign(Vector2.Dot(rb.linearVelocity, tangent));

        float randomForce = Random.Range(0, _physicsConfig.RandomWallKickForce);
        rb.AddForce(tangent * direction * randomForce, ForceMode2D.Impulse);
    }
}

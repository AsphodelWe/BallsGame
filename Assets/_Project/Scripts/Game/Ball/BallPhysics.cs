using R3;
using Reflex.Attributes;
using UnityEngine;


public class BallPhysics : MonoBehaviour
{
    [Inject] private IMapScaleProvider _mapScaleProvider;
    [SerializeField] private LayerMask _wallLayer;
    [SerializeField] private LayerMask _ballLayer;
    private BallPhysicsConfig _physicsConfig;
    private Rigidbody2D rb;
    private const float MIN_SPEED_THRESHOLD = 0.01f;
    private float _currentSpeedMultiplier;
    private float _lastCollisionTime;

    public void Initialize(BallPhysicsConfig physicsConfig)
    {
        rb = GetComponent<Rigidbody2D>();
        _physicsConfig = physicsConfig;
        _currentSpeedMultiplier = _physicsConfig.SpeedMultiplier;
        SetRandomStartDirection();
    }

    private float GetMultiplier() => _physicsConfig.ForceCurve.Evaluate(_mapScaleProvider.ScaleMultiplier);
    private float GetSpeedFactor() => _physicsConfig.SpeedFactorCurve.Evaluate(_mapScaleProvider.ScaleMultiplier);
    private float GetDecayFactor() => _physicsConfig.DecayFactorCurve.Evaluate(_mapScaleProvider.ScaleMultiplier);

    private void SetRandomStartDirection()
    {
        float multiplier = GetMultiplier();
        rb.linearVelocity = Random.insideUnitCircle.normalized * _physicsConfig.BaseSpeed * multiplier;
    }

    void FixedUpdate()
    {
        float multiplier = GetMultiplier();
        float speedFactor = GetSpeedFactor();
        float decayFactor = GetDecayFactor();

        MaintainSpeed(multiplier);
        ApplyDecay(speedFactor, decayFactor);
    }

    private void MaintainSpeed(float multiplier)
    {
        if (rb.linearVelocity.magnitude < MIN_SPEED_THRESHOLD) return;

        float boostForce = _physicsConfig.BoostForce * multiplier;
        float targetSpeed = _physicsConfig.BaseSpeed * _currentSpeedMultiplier;

        if (rb.linearVelocity.magnitude < targetSpeed * _physicsConfig.SpeedThreshold)
        {
            rb.AddForce(rb.linearVelocity.normalized * boostForce, ForceMode2D.Force);
        }
    }

    private void ApplyDecay(float speedFactor, float decayFactor)
    {
        float minSpeed = _physicsConfig.MinSpeed * speedFactor;
        float decayRate = _physicsConfig.SpeedDecayRate * decayFactor;

        _currentSpeedMultiplier = Mathf.Max(minSpeed,
            _currentSpeedMultiplier - Time.fixedDeltaTime * decayRate);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        float multiplier = GetMultiplier();
        float speedMultiplier = _physicsConfig.WallBoostMultiplier * multiplier;
        float bounceMultiplier = multiplier;

        if (IsInLayerMask(collision.gameObject, _wallLayer))
        {
            SetCollisonSpeed(collision, speedMultiplier, bounceMultiplier);
            AddRandomWallLowKick(collision, multiplier);
        }
        else if (IsInLayerMask(collision.gameObject, _ballLayer))
        {
            SetCollisonSpeed(collision, speedMultiplier, bounceMultiplier);
        }
    }

    private void SetCollisonSpeed(Collision2D collision, float speedMultiplier, float bounceMultiplier)
    {
        _currentSpeedMultiplier = Mathf.Min(_physicsConfig.MaxSpeedMultiplier,
            _currentSpeedMultiplier + speedMultiplier);

        Vector2 normal = collision.contacts[0].normal;
        Vector2 bump = normal * _physicsConfig.BounceImpulse * bounceMultiplier;

        rb.AddForce(bump, ForceMode2D.Impulse);
    }

    private void AddRandomWallLowKick(Collision2D collision, float multiplier)
    {
        Vector2 normal = collision.contacts[0].normal;
        Vector2 tangent = new Vector2(-normal.y, normal.x);
        float direction = Mathf.Sign(Vector2.Dot(rb.linearVelocity, tangent));
        float randomForce = Random.Range(0, _physicsConfig.RandomWallKickForce * multiplier);

        rb.AddForce(tangent * direction * randomForce, ForceMode2D.Impulse);
    }

    private bool IsInLayerMask(GameObject obj, LayerMask mask)
    {
        return (mask.value & (1 << obj.layer)) != 0;
    }

    public void ApplyForce(Vector2 direction, float force)
    {
        rb.AddForce(direction * force, ForceMode2D.Impulse);
    }
}

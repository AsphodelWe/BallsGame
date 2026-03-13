using UnityEngine;

public abstract class ShootWeapon : Attacker
{
    [SerializeField] private Transform _firePoint;
    private ObjectPool<Bullet> _bulletPool;
    private float _angleVelocity;
    private GameObject _bulletPrefab;
    private float _bulletSpeed;

    public virtual void Initialize(SideConfig side, int damage, GameObject bulletPrefab, float bulletSpeed)
    {
        //base.Initialize(side, damage);
        _bulletPrefab = bulletPrefab;
        _bulletSpeed = bulletSpeed;
    }





    void Update()
    {
        /*         if (Target != null) MoveTarget(); */
    }

    public void MoveTarget()
    {
        /*         if (Target == null) return;

                Vector2 direction = (Target.transform.position - transform.position).normalized;

                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                float currentAngle = transform.eulerAngles.z;
                float newAngle = Mathf.SmoothDampAngle(currentAngle, angle, ref _angleVelocity, SmoothTime);

                transform.rotation = Quaternion.AngleAxis(newAngle, Vector3.forward); */
    }
}

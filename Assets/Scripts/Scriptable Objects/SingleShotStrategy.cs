using UnityEngine;

[CreateAssetMenu(fileName = "SingleShotStrategy", menuName = "Scriptable Objects/SingleShotStrategy")]
public class SingleShotStrategy : ScriptableObject
{
    public GameObject BulletPrefab;
    public float BulletSpeed;

        public void Attack(Weapon weapon)
    {
        var bullet = Instantiate(BulletPrefab, weapon.FirePoint.position, Quaternion.identity);
        //bullet.GetComponent<Bullet>().Initialize(weapon.Damage, weapon.Side, weapon.FirePoint.right);
    }
}

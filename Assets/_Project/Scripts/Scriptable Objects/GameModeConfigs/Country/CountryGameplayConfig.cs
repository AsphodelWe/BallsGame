using UnityEngine;

[CreateAssetMenu(fileName = "CountryGameplayConfig", menuName = "Scriptable Objects/CountryGameplayConfig")]
public class CountryGameplayConfig : ScriptableObject
{
    [SerializeField] private int _maxHealth;
    [SerializeField] private int _damage;
    [SerializeField] private float _cooldownWeapon;
    [SerializeField] private float _baseSpeed;

    public int MaxHealth => _maxHealth;
    public int Damage => _damage;
    public float CooldownWeapon => _cooldownWeapon;
    public float BaseSpeed => _baseSpeed;
}


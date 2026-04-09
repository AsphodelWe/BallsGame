using System;
using Unity.VisualScripting;
using UnityEngine;


public class BallHealth : MonoBehaviour
{
    private int _maxHealth;
    private int _currentHealth;
    public Action OnDied;
    public Action<int> OnDamaged;
    public Action<int, int> OnHealthChanged;
    public void Initialize(int maxHealth)
    {
        _maxHealth = maxHealth;
        _currentHealth = _maxHealth;
    }

    public void GetDamage(int damage)
    {
        _currentHealth -= damage;

        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            OnDied?.Invoke();
        }
        
        OnHealthChanged?.Invoke(_currentHealth, _maxHealth);
        OnDamaged?.Invoke(damage);
    }

    public int GetMaxHealth { get => _maxHealth; }
    public int GetCurrentHealth { get => _currentHealth; }
}

using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CountryGameplayConfig", menuName = "Scriptable Objects/CountryGameplayConfig")]
public class CountryGameplayConfig : ScriptableObject
{
    [NonSerialized] public int MaxHealth;
    public float BaseSpeed;
    public int DefaultMaxHealth;
}


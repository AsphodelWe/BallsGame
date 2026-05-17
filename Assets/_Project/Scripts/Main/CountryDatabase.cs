using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CountryDatabase", menuName = "Scriptable Objects/CountryDatabase")]
public class CountryDatabase : ScriptableObject
{
    public List<CountryConfig> AllCountries = new();
}

using UnityEngine;

public class CountryComponent : MonoBehaviour
{
    [SerializeField] private CountryConfig _countryConfig;
    public CountryConfig CountryConfig => _countryConfig;
}

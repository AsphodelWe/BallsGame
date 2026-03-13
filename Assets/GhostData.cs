using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GhostData
{
    //public CountryConfig _countryCongig;
    public Vector3 _position;
    public GhostData(CountryConfig countryCongig, Vector3 position)
    {
        //_countryCongig = countryCongig;
        _position = position;
    }
}

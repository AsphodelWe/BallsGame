using UnityEngine;

public class GhostFactory
{
    public Ghost Create(CountryConfig country)
    {
        GameObject obj = Object.Instantiate(country.BallPrefabPrew);
        Ghost ghost = obj.GetComponent<Ghost>();
        ghost.Initialize(country);
        return ghost;
    }
}

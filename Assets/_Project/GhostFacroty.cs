using UnityEngine;

public class GhostFacroty
{
    public Ghost Create(CountryConfig country, float alpha)
    {
        GameObject obj = Object.Instantiate(country.BallPrefabPrew);
        Ghost ghost = obj.GetComponent<Ghost>();
        ghost.Initialize(country, alpha);
        return ghost;
    }
}

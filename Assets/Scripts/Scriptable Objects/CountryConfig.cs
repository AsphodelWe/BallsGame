using UnityEngine;

[CreateAssetMenu(fileName = "CountryConfig", menuName = "Scripts/Scriptable Objects/CountryConfig")]
public class CountryConfig : ScriptableObject
{
    public string CountryName;
    public GameObject BallPrefabPrew;
    public GameObject BallPrefabBattle;

    [Header("Configs")]
    public SideConfig Side; 
    public CountryGameplayConfig CountryGameplayConfig;
    public BallPhysicsConfig PhysicsConfig;
}

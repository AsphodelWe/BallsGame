using UnityEngine;

[CreateAssetMenu(fileName = "BattleUISettings", menuName = "Scriptable Objects/BattleUISettings")]
public class BattleUISettings : ScriptableObject
{
    [Range(150, 250)]
    public float BaseBarWidth = 200f;

    [Range(250, 300)]
    public float MediumBarWidth = 250f;

    [Range(300, 850)]
    public float BigBarWidth = 300f;
}
